using Mappix.Data;
using System.Text.RegularExpressions;

namespace Mappix.Actions
{
    public class RegionMapper
    {
        public SettingsProfile Profile;

        public static Region GetRegion(string text, RegionRule rule)
        {
            Region region = new();

            region.StartIndex = text.IndexOf(rule.StartPoint);
            region.EndIndex = text.IndexOf(rule.EndPoint) + rule.EndPoint.Length;

            if (region.EndIndex > text.Length)
                region.EndIndex = text.Length;

            return region;
        }

        public string RemoveRegion(Region region, string text)
        {
            if (Profile.DeleteRegions == false) { return text; }
            if (region.StartIndex < 0 || region.EndIndex < 0) { return text; }
            //if (region.EndIndex < region.StartIndex) { return ";(stop)"; }

            text = text.Remove(region.StartIndex, region.EndIndex - region.StartIndex);

            return text;
        }

        public static List<string> FindCommentsRegex(string text)
        {
            List<string> comments = [];
            var matches = Regex.Matches(text, @"/\*.*?\*/", RegexOptions.Singleline);

            foreach (Match match in matches)
            {
                comments.Add(match.Value);
            }
            return comments;
        }

        public static string RemoveAllCommentsRegex(string text)
        {
            return Regex.Replace(text, @"/\*.*?\*/", "", RegexOptions.Singleline);
        }
    }
}
