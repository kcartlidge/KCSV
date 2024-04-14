using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KCSV.Models
{
    /// <summary>CSV as a collection of rows and cells.</summary>
    public class Table
    {
        /// <summary>The individual rows of the CSV.</summary>
        public ReadOnlyCollection<Row> Rows => RowList.AsReadOnly();

        /// <summary>The number of rows found in the CSV.</summary>
        public int RowCount { get; internal set; }

        /// <summary>The cell count for the narrowest row.</summary>
        public int MinCellCount { get; internal set; }

        /// <summary>The cell count for the widest row.</summary>
        public int MaxCellCount { get; internal set; }

        /// <summary>Do any rows have less cells than others?</summary>
        public bool IsJagged { get; internal set; }

        /// <summary>The individual rows of the CSV (editable).</summary>
        internal List<Row> RowList { get; set; } = new List<Row>();

        /// <summary>
        /// Ensures all rows have the same amount of cells
        /// by adding extra (empty) ones where necessary.
        /// In effect, removes the jaggedness.
        /// </summary>
        public void SquareOff()
        {
            var max = MaxCellCount;
            var empty = new Cell(false, "");
            foreach (var row in RowList)
            {
                // Skip rows that are already full.
                var shortfall = max - row.CellCount;
                if (shortfall == 0) continue;

                for (var i = 0; i < shortfall; i++)
                    row.Cells.Add(empty);
            }
            UpdateStats();
        }

        /// <summary>
        /// Recalculates min/max cells per row and whether
        /// the table has jagged rows.
        /// </summary>
        internal void UpdateStats()
        {
            if (RowList == null || RowList.Count == 0)
            {
                MinCellCount = MaxCellCount = 0;
                IsJagged = false;
                return;
            }
            MinCellCount = RowList.Min(row => row.CellCount);
            MaxCellCount = RowList.Max(row => row.CellCount);
            IsJagged = RowList.Any(row => row.CellCount < MaxCellCount);
        }
    }
}
