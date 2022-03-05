using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notion.Client;
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

    [Parameter("Database from which to read OSC Resources")]
    private string _notionDB;
    [Parameter("Secret for Integration")]
    private string _notionSecret;
    
    static JObject _localSecrets;
    static JObject LocalSecrets {
        get
        {
            if (_localSecrets == null)
            {
                string json = File.ReadAllText(BuildProjectDirectory / "secrets.json");
                _localSecrets = JObject.Parse(json);
            }
            return _localSecrets;
        }
    }

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
        .DependsOn(UpdateResourcesFromNotion)
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
        .DependsOn(UpdateResourcesFromNotion)
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
    
    
    
    Target LoadSecrets => _ => _
        .Executes(() =>
        {
            if (IsLocalBuild)
            {
                _notionSecret = LocalSecrets["NOTION_SECRET"]?.ToString();
                _notionDB = LocalSecrets["NOTION_DB"]?.ToString();
            }
        });

    Target UpdateResourcesFromNotion => _ => _
        .DependsOn(LoadSecrets)
        .Executes(async () =>
        {
            var client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = _notionSecret
            });
            
            var selectFilter = new SelectFilter("Status", equal: "Approved");
            var filter = new CompoundFilter(and: new List<Filter>(){selectFilter});
            var queryParams = new DatabasesQueryParameters { Filter = filter};
            var pages = await client.Databases.QueryAsync(_notionDB, queryParams);
            
            // Turn database into SortedDictionary
            var resources = new SortedDictionary<string, List<string>>();
            foreach (Page page in pages.Results)
            {
                var nameValue = page.Properties["Name"] as TitlePropertyValue;
                if (nameValue == null || nameValue.Title.Count == 0)
                {
                    Console.WriteLine($"Couldn't read name of {page.Id}");
                    continue;
                }
                var name = nameValue.Title[0].PlainText;
                
                var descriptionValue = page.Properties["Description"] as RichTextPropertyValue;
                if (descriptionValue == null || descriptionValue.RichText.Count == 0)
                {
                    Console.WriteLine($"Couldn't read description of {name}");
                    continue;
                }
                var description = descriptionValue.RichText[0].PlainText;
                
                var linkValue = page.Properties["Link"] as UrlPropertyValue;
                if (linkValue == null)
                {
                    Console.WriteLine($"Couldn't read Link of {name}");
                    continue;
                }

                var link = linkValue.Url;

                var languagesValue = page.Properties["Language"] as MultiSelectPropertyValue;
                if (languagesValue == null)
                {
                    Console.WriteLine($"Couldn't read Languages of {name}");
                    continue;
                }
                var languages =
                    languagesValue.MultiSelect.Select(entry => entry.Name);
                var languageString = string.Join(",",languages);

                var publicCategoryValue = page.Properties["Public Category"] as SelectPropertyValue;
                if (publicCategoryValue == null)
                {
                    Console.WriteLine($"Couldn't read Public Category of {name}");
                    continue;
                }
                var publicCategory = publicCategoryValue.Select.Name;

                string entry = $"* [{name}]({link}): {description} ({languageString})";

                if (resources.ContainsKey(publicCategory))
                {
                    resources[publicCategory].Add(entry);
                }
                else
                {
                    resources.Add(publicCategory, new List<string>(){entry});
                }
            }
            
            // Combine categories and entries into string
            StringBuilder builder = new StringBuilder();
            foreach (var category in resources)
            {
                // Add category header
                builder.AppendLine($"# {category.Key}");
                
                // Add all items
                foreach (string entry in category.Value)
                {
                    builder.AppendLine(entry);
                }

                builder.AppendLine(); // Adding a space at end of each category
            }

            // Write to target page
            var targetFile = _markdownSourceDir / "Resources.md";
            var contents = File.ReadAllText(targetFile);
            contents = contents.Replace("{{Resources}}", builder.ToString());
            File.WriteAllText(targetFile, contents);
        });
}