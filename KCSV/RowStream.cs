using System;
using System.IO;
using KCSV.Models;

namespace KCSV
{
    /// <summary>A forward-only stream of parsed CSV rows from a file.</summary>
    public class RowStream : IDisposable
    {
        /// <summary>Has the stream reached the end?</summary>
        public bool EndOfStream => reader == null || reader.EndOfStream;

        internal int rowCount;
        private StreamReader reader;
        private readonly Delimiters delimiter;

        /// <summary>
        /// Create a forward-only stream of parsed CSV rows from a file.
        /// </summary>
        internal RowStream(string filename, Delimiters delimiter = Delimiters.Comma)
        {
            reader = new StreamReader(filename);
            rowCount = 0;
            this.delimiter = delimiter;
        }

        /// <summary>
        /// Returns the next parsed CSV row from a file, or null.
        /// </summary>
        /// <returns></returns>
        public Row NextRow()
        {
            if (EndOfStream) return null;

            rowCount += 1;
            return new Row(rowCount, delimiter, reader.ReadLine());
        }

        /// <summary>Ensures the underlying file stream is closed.</summary>
        public void Dispose()
        {
            try
            {
                reader.Dispose();
            }
            catch { }

            reader = null;
        }
    }
}
