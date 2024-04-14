using System;

namespace KCSV
{
    /// <summary>
    /// A CSV parser exception, with row/character position.
    /// </summary>
    public class CSVException : Exception
    {
        /// <summary>1-based original row number.</summary>
        public int Row;

        /// <summary>1-based offset into original line.</summary>
        public int Character;

        /// <summary>
        /// A CSV parsing exception, with position.
        /// </summary>
        /// <param name="rowNumber">1-based original row number.</param>
        /// <param name="index">0-based offset into the original row.</param>
        /// <param name="message">
        /// The descriptive portion (only) of the message.
        /// The rowNumber and index will be embedded automatically,
        /// and the index will become a public 1-based Character field.
        /// </param>
        public CSVException(int rowNumber, int index, string message)
            : base($"CSV {rowNumber}:{index + 1}  {message}")
        {
            Row = rowNumber;
            Character = index + 1;
        }
    }
}
