using KCSV.Models;

namespace KCSV;

/// <summary>
/// Parses CSV into collections of rows and cells.
/// </summary>
public class Parser
{
    /// <summary>The individual rows of the CSV.</summary>
    public List<Row> Rows { get; set; } = [];

    /// <summary>The number of rows found in the CSV.</summary>
    public int RowCount => Rows.Count;

    /// <summary>Create a new CSV parser for a file.</summary>
    /// <param name="filename">The file to read in.</param>
    public Parser(string filename)
    {
        using var rdr = new StreamReader(filename);
        while (rdr.EndOfStream == false)
        {
            // Parse each line as it is read in.
            Rows.Add(new Row(RowCount + 1, rdr.ReadLine()));
        }
        rdr.Close();
    }
}
