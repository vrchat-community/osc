using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace VRChat.Docs
{
    [System.Serializable]
    public class ReadmeDoc
    {
        public bool hidden;
        // public int order;
        public string title;
        public string type = "basic";
        public string body;
        public string category;
        public string parentDoc;
        public string slug;

        public ReadmeDoc(string title, string category, string body)
        {
            this.title = title;
            this.category = category;
            this.body = body;
        }

        public ReadmeDoc(string path)
        {
            if (!File.Exists(path))
            {
                Serilog.Log.Error($"Can't open file at {path}");
            }

            string contents = File.ReadAllText(path);
            string yaml = Utilities.GetYamlFromString(contents);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();

            var fm = deserializer.Deserialize<OSCDocsFrontMatter>(yaml);
            this.title = fm.title;
            this.category = fm.category;
            this.body = Utilities.GetMarkdownWithoutFrontMatter(contents);
            this.parentDoc = string.IsNullOrWhiteSpace(fm.parentDoc) ? null : fm.parentDoc;
            this.slug = fm.slug;
        }
    }
}