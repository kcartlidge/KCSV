using System.Text;

namespace KCSV.Models;

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
    public readonly List<Cell> Cells = [];

    /// <summary>The number of cells in the line.</summary>
    public int CellCount => Cells.Count;

    /// <summary>
    /// Create a new row with the given contents.
    /// The text will be parsed into cells as the Row
    /// instance is created.
    /// The cells are strings with a flag for if there
    /// were quotes originally.
    /// </summary>
    /// <param name="rowNumber">Original CSV row number.</param>
    /// <param name="text">A line of CSV text.</param>
    public Row(int rowNumber, string? text)
    {
        RowNumber = rowNumber;
        Cells.AddRange(ExtractCells(text));
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
    private List<Cell> ExtractCells(string? text)
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
                else if (ch == ',' && !isQuoted)
                {
                    // If we are in an un-quoted cell then this is a field delimiter.
                    // Wind it back as the logic expects the ',' to still be there.
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
            if (scanner.HasMore && scanner.Peek() != ',')
                throw new CSVException(RowNumber, scanner.Position, "Expected a cell delimiter");
            scanner.Skip();
        }

        return cells;
    }
}
