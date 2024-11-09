using Mappix.Data;
using System.Text.RegularExpressions;

namespace Mappix.Actions
{
    internal class StringManipulations
    {
        public SettingsProfile Profile { get; set; }
        public char es = '\r';

        public string DeleteSpaces(string text)
        {
            if (Profile.DeleteSpaces)
                return Regex.Replace(text, @"^[ \t]+", "", RegexOptions.Multiline);
            else
                return text;
        }

        public string DeleteEnters(string text)
        {
            if (Profile.DeleteEnters)
                return Regex.Replace(text, @"[\n\r\v]+$", "", RegexOptions.Multiline);
            else
                return text;
        }

        public string DeleteEntersAdvanced(string text)
        {
            //if (Profile.DeleteEnters)
            //{
                string pattern = @"(?![^\(\[]*[\)\]])[^\(\[\n\r\v]*(?<=\([^\)]*\)|\[[^\]]*\]|^)[\n\r\v]+";

                return Regex.Replace(text, pattern, m =>
                {
                    return "";
                });
            //}
           // else
                //return text;
        }

        public string DeleteComments(string text)
        {
            if (Profile.DeleteComments)
                return Regex.Replace(text, @"//.*", "");
            else
                return text;
        }

        public static bool StrBool(string? text)
        {
            if (text == null) return false;
                Int32 i;

            try { i = Int32.Parse(text); } catch (Exception) { return false; }


            if (i > 0)
                return true;
            else
                return false;
        }
    }
}
