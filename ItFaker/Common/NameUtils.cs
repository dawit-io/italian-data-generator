using System.Globalization;

namespace ItFaker.Common
{
    /// <summary>
    /// Utility class for string formatting operations
    /// </summary>
    public static class StringFormatting
    {
        /// <summary>
        /// Formats an Italian name with proper capitalization
        /// </summary>
        /// <param name="name">The name to format</param>
        /// <returns>The properly formatted name</returns>
        public static string FormatItalianName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            // Ensure proper capitalization for Italian names
            TextInfo textInfo = new CultureInfo("it-IT", false).TextInfo;
            return textInfo.ToTitleCase(name.ToLower());
        }
    }
}