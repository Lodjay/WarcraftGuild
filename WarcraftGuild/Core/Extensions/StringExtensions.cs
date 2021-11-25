using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace WarcraftGuild.Core.Helpers
{
    public static class StringExtensions
    {
        public static string Slugify(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            return str.RemoveAccent().ToLower().RemoveInvalidCharacters().RemoveExcedentSpaces().CutAndTrim().SetHyphens();
        }

        public static string RemoveAccent(this string str)
        {
            StringBuilder sbReturn = new();
            var arrayText = str.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        private static string RemoveInvalidCharacters(this string str)
        {
            return Regex.Replace(str, @"[^a-z0-9\s-]", "");
        }

        private static string RemoveExcedentSpaces(this string str)
        {
            return Regex.Replace(str, @"\s+", " ").Trim();
        }

        private static string CutAndTrim(this string str)
        {
            return str[..(str.Length <= 45 ? str.Length : 45)].Trim();
        }

        private static string SetHyphens(this string str)
        {
            return Regex.Replace(str, @"\s", "-");
        }
    }
}