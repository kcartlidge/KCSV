using System.IO;
using KCSV.Models;

namespace KCSV
{
    /// <summary>
    /// Parses CSV into a table with collections of rows and cells.
    /// </summary>
    public static class Parser
    {
        /// <summary>Loads and parses CSV from a file.</summary>
        /// <param name="filename">The file to read in.</param>
        /// <param name="delimiter">Separator for cells in rows.</param>
        /// <returns>
        /// CSV as a collection of rows and cells.
        /// </returns>
        public static Table LoadTable(string filename, Delimiters delimiter = Delimiters.Comma)
        {
            var table = new Table();
            using (var rdr = new StreamReader(filename))
            {
                while (rdr.EndOfStream == false)
                {
                    // Parse each line as it is read in.
                    table.RowCount += 1;
                    table.RowList.Add(new Row(table.RowCount, delimiter, rdr.ReadLine()));
                }
                rdr.Close();
            }

            table.UpdateStats();
            return table;
        }

        /// <summary>Parses CSV from a collection of strings.</summary>
        /// <param name="rows">String content to parse.</param>
        /// <param name="delimiter">Separator for cells in rows.</param>
        public static Table FromStrings(string[] rows, Delimiters delimiter = Delimiters.Comma)
        {
            var table = new Table();
            foreach (var row in rows)
            {
                // Parse each line as it is accessed.
                table.RowCount += 1;
                table.RowList.Add(new Row(table.RowCount, delimiter, row));
            }

            table.UpdateStats();
            return table;
        }
    }
}
