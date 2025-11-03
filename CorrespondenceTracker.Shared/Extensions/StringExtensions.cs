using System.Web;

namespace CorrespondenceTracker.Shared.Extensions
{
    public static class StringExtensions
    {

        public static string ReplaceIfStartsWith(this string input, string oldValue, string newValue)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (oldValue == null)
                throw new ArgumentNullException(nameof(oldValue));

            if (newValue == null)
                throw new ArgumentNullException(nameof(newValue));

            if (input.StartsWith(oldValue))
            {
                return newValue + input.Substring(oldValue.Length);
            }

            return input;
        }
        public static string ToHtmlWithLineBreaks(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            // First, HTML encode the string to escape special characters
            string escaped = HttpUtility.HtmlEncode(input);

            // Then, replace newline characters with <br> tags
            return escaped.Replace("\n", "<br/>")
                          .Replace("\r\n", "<br/>")  // For Windows-style line endings
                          .Replace("\r", "<br/>");   // For Mac OS 9 and earlier
        }
    }
}
