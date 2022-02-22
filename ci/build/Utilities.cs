namespace VRChat.Docs
{
    public class Utilities
    {
        private static string _frontMatterStartString = "---";
        private static string _frontMatterEndString = "---";
        
        public static string GetYamlFromString(string contents)
        {
            var frontMatterStartIndex = contents.IndexOf(_frontMatterStartString);
            if(frontMatterStartIndex == -1) return null;

            var frontMatterEndIndex = contents.IndexOf(_frontMatterEndString, frontMatterStartIndex + _frontMatterStartString.Length);
            if (frontMatterEndIndex == -1) return null;

            return contents.Substring(frontMatterStartIndex + _frontMatterStartString.Length, frontMatterEndIndex - frontMatterStartIndex - _frontMatterEndString.Length);
        }

        public static string GetMarkdownWithoutFrontMatter(string contents)
        {
            // remove all frontmatter
            var frontMatterStartIndex = contents.IndexOf(_frontMatterStartString);
            if(frontMatterStartIndex == -1) return null;

            var frontMatterEndIndex = contents.IndexOf(_frontMatterEndString, frontMatterStartIndex + _frontMatterStartString.Length);
            if (frontMatterEndIndex == -1) return null;

            return contents.Substring(frontMatterEndIndex + _frontMatterEndString.Length).TrimStart();
        }
    }
}