using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KCSV.Models
{
    /// <summary>
    /// Holds a single parsed row from the CSV input.
    /// The cells are contains text and a flag for whether they
    /// were originally quoted.
    /// </summary>
    public class Row
    {
        /// <summary>The original CSV row number.</summary>
        public readonly int RowNumber = 0;

        /// <summary>The parsed cells from the row.</summary>
        public readonly List<Cell> Cells = new List<Cell>();

        /// <summary>The number of cells in the row.</summary>
        public int CellCount => Cells.Count;

        /// <summary>Separator between cells.</summary>
        private readonly Delimiters Delimiter;

        /// <summary>The separator for cells in a row.</summary>
        private readonly char Separator = ',';

        /// <summary>
        /// Create a new row with the given contents.
        /// The text will be parsed into cells as the Row
        /// instance is created.
        /// The cells are strings with a flag for if there
        /// were quotes originally.
        /// </summary>
        /// <param name="rowNumber">Original CSV row number.</param>
        /// <param name="text">A line of CSV text.</param>
        public Row(int rowNumber, Delimiters delimiter, string text)
        {
            RowNumber = rowNumber;
            Delimiter = delimiter;
            switch (delimiter)
            {
                case Delimiters.Tab:
                    Separator = '\t';
                    break;
                case Delimiters.Comma:
                default:
                    Separator = ',';
                    break;
            }
            Cells.AddRange(ExtractCells(text));
        }

        /// <summary>
        /// Returns the row as a CSV string, with cells
        /// quoted as per the original file.
        /// The output will be well-formed and regular
        /// so it may not exactly match the original in
        /// terms of whitespace.
        /// WARNING: If the source was Tab-delimited then
        /// ALL cells will be quoted. Rows parsed with
        /// Tab delimiters may contain unexpected
        /// characters when rendered as CSV as CSV is
        /// less flexible in what it supports. Automatic
        /// escaping is NOT performed as it may lead to
        /// issues if the content was already escaped.
        /// </summary>
        public string AsCSV()
        {
            var content = new List<string>();
            if (Delimiter == Delimiters.Comma)
                content = Cells.Select(c => $"{c.Formatted}").ToList();
            else
                content = Cells.Select(c => $"\"{c.Text}\"").ToList();
            return string.Join(",", content);
        }

        /// <summary>
        /// Returns the row as a Tab-delimited string,
        /// with cells quoted as per the original file.
        /// The output will be well-formed and regular
        /// so it may not exactly match the original in
        /// terms of whitespace.
        /// WARNING: If the source was CSV then embedded
        /// tabs within CSV cells may break the output
        /// when Tabs are used as delimiters. Therefore
        /// ALL tabs within the content itself will be
        /// escaped (replaced with "\t").
        /// </summary>
        public string AsTabbed()
        {
            var content = Cells
                .Select(c => $"{c.Formatted.Replace("\t", "\\t")}")
                .ToList();
            return string.Join("\t", content);
        }

        override public string ToString()
        {
            return $"Row {RowNumber}: {CellCount} cell(s)";
        }

        /// <summary>
        /// Splits text into cells using a Scanner.
        /// It will handle double-quotes and embedded commas
        /// within quoted content.
        /// </summary>
        /// <param name="text">A line of CSV text.</param>
        /// <returns>A collection of extracted cells.</returns>
        /// <remarks>
        /// This is the method with the logic for splitting text
        /// into CSV using of the Scanner.
        /// </remarks>
        private List<Cell> ExtractCells(string text)
        {
            var cells = new List<Cell>();
            var scanner = new Scanner(text);

            // One iteration captures one cell.
            while (scanner.HasMore)
            {
                // Start capturing after any leading spaces.
                var cell = new StringBuilder();
                scanner.SkipOver(' ');

                // Remove any opening quote.
                var isQuoted = scanner.Peek() == '"';
                if (isQuoted) scanner.Skip();

                // Gather until the end of the cell.
                var last = (char)0;
                while (scanner.HasMore)
                {
                    var ch = scanner.Scan();
                    if (ch == '"')
                    {
                        // Quotes should only appear for a quoted cell.
                        if (isQuoted)
                        {
                            // If it isn't escaped, it's the end of the cell.
                            if (last != '\\') break;  // while
                        }
                        else throw new CSVException(RowNumber, scanner.Position, "Unexpected quote character");
                    }
                    else if (ch == Separator && !isQuoted)
                    {
                        // If we are in an un-quoted cell then this is a field delimiter.
                        // Wind it back as the logic expects the delimiter to still be there.
                        scanner.Back();
                        break;  // while
                    }

                    cell.Append(ch);
                    last = ch;
                }

                // Add cells, retaining whitespace if quoted.
                if (isQuoted) cells.Add(new Cell(isQuoted, cell.ToString()));
                else cells.Add(new Cell(isQuoted, cell.ToString().Trim()));

                // Skip any whitespace.
                scanner.SkipOver(' ');

                // If there's more content it must be a delimiter.
                if (scanner.HasMore && scanner.Peek() != Separator)
                    throw new CSVException(RowNumber, scanner.Position, "Expected a cell delimiter");
                scanner.Skip();
            }

            return cells;
        }
    }
}
