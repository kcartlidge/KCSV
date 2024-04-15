using System;
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
            try
            {
                using (var rdr = new StreamReader(filename))
                {
                    while (rdr.EndOfStream == false)
                    {
                        // Parse each line as it is read in.
                        table.RowCount += 1;
                        table.RowList.Add(new Row(table.RowCount, delimiter, rdr.ReadLine()));
                    }
                }
            }
            catch (CSVException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CSVException(table.RowCount, -1, ex.Message);
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
            try
            {
                foreach (var row in rows)
                {
                    // Parse each line as it is accessed.
                    table.RowCount += 1;
                    table.RowList.Add(new Row(table.RowCount, delimiter, row));
                }
            }
            catch (CSVException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CSVException(table.RowCount, -1, ex.Message);
            }

            table.UpdateStats();
            return table;
        }

        /// <summary>
        /// Returns a stream for consuming rows of parsed CSV from a file.
        /// This reduces overhead as the whole file is NOT read in advance.
        /// </summary>
        /// <param name="filename">The file to read in.</param>
        /// <param name="delimiter">Separator for cells in rows.</param>
        /// <returns>
        /// A stream of rows, where each is read only as it is requested.
        /// The usual Table structure is not available because the extra
        /// information it provides (eg RowCount) cannot be known as all
        /// rows have not been consumed.
        /// </returns>
        public static RowStream StreamTable(string filename, Delimiters delimiter = Delimiters.Comma)
        {
            RowStream stream = null;
            try
            {
                return new RowStream(filename, delimiter);
            }
            catch (CSVException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var rowNum = stream == null ? 0 : stream.rowCount;
                throw new CSVException(rowNum, -1, ex.Message);
            }
        }
    }
}
