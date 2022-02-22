using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Utilities;
using VRChat.Docs;

class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.GitHubWikiToReadmeDotCom);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    
    [Parameter("Get it from https://dash.readme.com/project/vrchat/{version}/api-key")]
    private string _readmeApiKey = "";
    private string _readmeDocsVersion = "v2022.1.1";
    
    private AbsolutePath _markdownSourceDir = RootDirectory.Parent / "docs";
    private AbsolutePath _readmeDocsDir = RootDirectory / "_temp_Readme";
    private AbsolutePath _githubTempDir = RootDirectory / "_temp_GHWiki";

    /// <summary>
    /// This works for now - we could instead to a prepass of all files to build this from each page's Front Matter
    /// </summary>
    private Dictionary<string, string> _slugLookupWikiToReadme = new Dictionary<string, string>()
    {
        { "Input", "osc-as-input-controller" },
        { "Avatar-Parameters", "osc-avatar-parameters" },
        { "Debugging", "osc-debugging" },
        { "DIY", "osc-diy" },
        { "Home", "osc-overview" }
    };
    
    private async Task SendPageToReadme(string path)
    {
        ReadmeDoc doc = new ReadmeDoc(path);
        string docAsString = JsonConvert.SerializeObject(doc, Formatting.None, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri($"https://dash.readme.com/api/v1/docs/{doc.slug}"),
            Headers =
            {
                { "Accept", "application/json" },
                { "x-readme-version", _readmeDocsVersion },
                { "Authorization", $"Basic {_readmeApiKey}" },
            },
            Content = new StringContent(docAsString)
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            }
        };
        Serilog.Log.Information($"Sending {doc.slug}");
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Posted {doc.slug}");
        }
    }

    /// <summary>
    /// Just remove frontmatter for now, since we specifically write for GH Wiki syntax
    /// </summary>
    Target ProcessFilesForWiki => _ => _
        .Executes(() =>
        {
            FileSystemTasks.EnsureExistingDirectory(_githubTempDir);
            
            var files = new DirectoryInfo(_markdownSourceDir).GetFiles("*.md");
            foreach (FileInfo info in files)
            {
                string contents = File.ReadAllText(info.FullName);
                if (contents.IsNullOrWhiteSpace())
                {
                    Serilog.Log.Error($"Got empty file {info.Name}");
                    continue;
                }
                
                // remove all frontmatter
                contents = Utilities.GetMarkdownWithoutFrontMatter(contents);

                string targetPath = _githubTempDir / info.Name;
                File.WriteAllText(targetPath, contents);
            }
        });

    /// <summary>
    /// Use frontmatter to change page slugs
    /// </summary>
    Target GitHubWikiToReadmeDotCom => _ => _
        .Executes(() =>
        {
            FileSystemTasks.EnsureExistingDirectory(_readmeDocsDir);
            
            var files = new DirectoryInfo(_markdownSourceDir).GetFiles("*.md");
            foreach (FileInfo info in files)
            {
                string contents = File.ReadAllText(info.FullName);
                if (contents.IsNullOrWhiteSpace())
                {
                    Serilog.Log.Error($"Got empty file {info.Name}");
                    continue;
                }
                // replace all wiki slugs with readme slugs
                foreach (var pair in _slugLookupWikiToReadme)
                {
                    if (contents.Contains(pair.Key))
                    {
                        contents = contents.Replace($"]({pair.Key})", $"]({pair.Value})");
                        Serilog.Log.Information($"Replaced {pair.Key} with {pair.Value} in {info.Name}");
                    }
                }
                string targetPath = _readmeDocsDir / info.Name;
                File.WriteAllText(targetPath, contents);
            }
        });

    Target PushReadmeUpdates => _ => _
        .DependsOn(GitHubWikiToReadmeDotCom)
        .Executes(async () =>
        {
            var files = new DirectoryInfo(_readmeDocsDir).GetFiles("*.md");
            
            // go through every file in _temp_Readme
            foreach (FileInfo info in files)
            {
                await SendPageToReadme(info.FullName);
            }
        });
}