namespace KCSV.Models
{
    /// <summary>A single item from a CSV row.</summary>
    public class Cell
    {
        /// <summary>
        /// Were there double-quotes in the input CSV?
        /// </summary>
        public readonly bool IsQuoted = false;

        /// <summary>
        /// The text, with any quoting from the original
        /// CSV content removed.
        /// </summary>
        public readonly string Text = "";

        /// <summary>
        /// The text, wrapped in quotes if the original
        /// CSV content included them.
        /// </summary>
        public readonly string Formatted = "";

        /// <summary>
        /// Create a new cell from the given content.
        /// </summary>
        /// <param name="isQuoted">Were there quotes in the CSV?</param>
        /// <param name="text">The content, excluding any quotes.</param>
        public Cell(bool isQuoted, string text)
        {
            IsQuoted = isQuoted;
            Text = text;

            var q = isQuoted ? "\"" : "";
            Formatted = $"{q}{Text}{q}";
        }

        override public string ToString()
        {
            var q = IsQuoted ? "[QUOTED] " : "";
            return $"{q}{Formatted}";
        }
    }
}
