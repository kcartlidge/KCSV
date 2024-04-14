using System;
using System.IO;
using KCSV.Models;

namespace KCSV.Tests;

/// <summary>
/// Class that provides a parsed Table backed by a temporary
/// file created using the provided line endings.
/// Using this factory ensures the created file is cleared
/// down again automatically upon dispose.
/// </summary>
internal class TableMaker : IDisposable
{
    private string Filename = "";

    /// <summary>
    /// Returns a parsed Table backed by a temporary file
    /// with the provided line endings. The file is cleared
    /// down again automatically upon dispose.
    /// </summary>
    public Table UsingTemporaryFile(string lineEndings)
    {
        // Get the contents and destination.
        var sourceFilename = Path.Combine("Fixtures", "ok-well-formed") + ".csv";
        var sourceText = File.ReadAllLines(sourceFilename);
        Filename = Path.GetTempFileName();

        // Write using the specified line endings.
        using var w = File.OpenWrite(Filename);
        using var writer = new StreamWriter(w);
        foreach (var line in sourceText)
            writer.Write(line + lineEndings);
        writer.Close();  // Not strictly needed but advisable here.
        w.Close();  // Not strictly needed but advisable here.
        return Parser.LoadTable(Filename);
    }

    public void Dispose()
    {
        try
        {
            // Attempt to clear up when done.
            File.Delete(Filename);
        }
        catch (Exception ex)
        {
            // Don't let this derail tests as temprary files
            // are small and eventually cleaned up by other
            // mechanisms anyway.
            Console.WriteLine("Warning: " + ex.Message);
        }
    }
}
