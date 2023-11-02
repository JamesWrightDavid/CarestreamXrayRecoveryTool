using System.IO;
using System.Text.RegularExpressions;

namespace Recovery.Core.Helpers
{
    /// <summary>
    /// Provides methods for sanitizing names and strings.
    /// </summary>
    public class PathSanitizer
    {
        /// <summary>
        /// Sanitizes the provided name to remove any characters that are invalid for file/directory names.
        /// </summary>
        /// <param name="name">The name to be sanitized.</param>
        /// <returns>The sanitized name without any invalid characters.</returns>
        public string SanitizeFileName(string name)
        {
            // Construct a regex pattern to match all invalid file name characters.
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegex = $"[{invalidChars}]";

            // Remove all invalid characters from the name.
            return Regex.Replace(name, invalidRegex, "");
        }
    }
}
