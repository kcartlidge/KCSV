using System;
using System.IO;
using KCSV.Models;

namespace KCSV.Tests;

internal static class Helper
{
    /// <summary>
    /// Checks all a row's cells, comparing each against the
    /// equivalent item in the expected parameter list.
    /// Populates the `cell` out parameter if a particular
    /// cell does not match.
    /// Throws an exception if the caller doesn;t provide
    /// enough expectetions for the cells.
    /// </summary>
    public static bool Check(Row row, out int cell, params string[] expected)
    {
        cell = -1;
        if (expected.Length != row.CellCount)
            throw new Exception($"Incorrect 'expected' in 'Check' call (wanted {row.CellCount}, got {expected.Length}).");

        for (var i = 0; i < expected.Length; i++)
        {
            cell = i + 1;
            if (expected[i] != row.Cells[i].Text) return false;
        }
        return true;
    }

    /// <summary>
    /// Creates a new parsed Table instance, converting
    /// the provided fixture name into a filename then
    /// loading the contents.
    /// </summary>
    public static Table LoadTable(string fixtureName)
    {
        var filename = Path.Combine("Fixtures", fixtureName) + ".csv";
        return Parser.LoadTable(filename);
    }
}
