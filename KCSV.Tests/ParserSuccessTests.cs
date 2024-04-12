using System;
using System.IO;
using KCSV.Models;

namespace KCSV.Tests;

public class ParserSuccessTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Parser_HandlesRowsOfText()
    {
        var csv = new string[] { "1,2", "1,2", "1,2", "1,2" };
        var parser = new Parser(csv);

        Assert.That(parser.RowCount, Is.EqualTo(4));
    }

    [Test]
    public void Parser_LoadsFile()
    {
        var parser = NewParser("ok-well-formed");

        Assert.That(parser.RowCount, Is.EqualTo(4));
    }

    [Test]
    public void Parser_RowsAreNumbered_FromOne()
    {
        var parser = NewParser("ok-well-formed");

        Assert.That(parser.RowCount, Is.EqualTo(4));
        for (var i = 0; i < parser.RowCount; i++)
            Assert.That(parser.Rows[i].RowNumber, Is.EqualTo(i + 1),
                        $"Found in row {i}");
    }

    [Test]
    public void Parser_RowsAreSplit_IntoCells()
    {
        var parser = NewParser("ok-well-formed");

        Assert.That(parser.RowCount, Is.EqualTo(4));
        for (var i = 0; i < parser.RowCount; i++)
            Assert.That(parser.Rows[i].CellCount, Is.EqualTo(7),
                        $"Found in row {i}");
    }

    [Test]
    public void Parser_WellFormedCells_AreCorrect()
    {
        var parser = NewParser("ok-well-formed");
        var cell = -1;

        Assert.That(parser.RowCount, Is.EqualTo(4));
        Assert.That(Check(parser.Rows[0], out cell, "ID", "CODE", "NAME", "CAPITAL", "LATITUDE", "LONGITUDE", "COMMENT"), $"Row 1 has incorrect items (cell {cell})");
        Assert.That(Check(parser.Rows[1], out cell, "1", "USA", "North America", "Washington", "47.751076", "-120.740135", "The USA and capital."), $"Row 2 has incorrect items (cell {cell})");
        Assert.That(Check(parser.Rows[2], out cell, "2", "UK", "United Kingdom", "London", "51.507351", "-0.127758", "The UK - whose capital is London."), $"Row 3 has incorrect items (cell {cell})");
        Assert.That(Check(parser.Rows[3], out cell, "3", "FR", "France", "Paris", "48.856613", "2.352222", "France - home of the city of Paris."), $"Row 4 has incorrect items (cell {cell})");
    }

    [Test]
    public void Parser_EmbeddedCommas_AreAllowed()
    {
        var parser = NewParser("ok-embedded-commas");
        var cell = -1;

        Assert.That(parser.RowCount, Is.EqualTo(4));
        Assert.That(Check(parser.Rows[0], out cell, "ID", "CODE", "NAME", "CAPITAL", "LATITUDE", "LONGITUDE", "COMMENT"), $"Row 1 has incorrect items (cell {cell})");
        Assert.That(Check(parser.Rows[1], out cell, "1", "USA", "North America", "Washington", "47.751076", "-120.740135", "The USA, and capital."), $"Row 2 has incorrect items (cell {cell})");
        Assert.That(Check(parser.Rows[2], out cell, "2", "UK", "United Kingdom", "London", "51.507351", "-0.127758", "The UK, whose capital is London."), $"Row 3 has incorrect items (cell {cell})");
        Assert.That(Check(parser.Rows[3], out cell, "3", "FR", "France", "Paris", "48.856613", "2.352222", "France, home of the city of Paris."), $"Row 4 has incorrect items (cell {cell})");
    }

    [Test]
    public void Parser_EmbeddedQuotes_AreAllowed()
    {
        var parser = NewParser("ok-embedded-quotes");
        var cell = -1;

        Assert.That(parser.RowCount, Is.EqualTo(4));
        Assert.That(Check(parser.Rows[0], out cell, "ID", "CODE", "NAME", "CAPITAL", "LATITUDE", "LONGITUDE", "COMMENT"), $"Row 1 has incorrect items (cell {cell})");
        Assert.That(Check(parser.Rows[1], out cell, "1", "USA", "North America", "Washington", "47.751076", "-120.740135", "The \\\"USA\\\" and capital."), $"Row 2 has incorrect items (cell {cell})");
        Assert.That(Check(parser.Rows[2], out cell, "2", "UK", "United Kingdom", "London", "51.507351", "-0.127758", "The \\\"UK\\\" - whose capital is \\\"London\\\"."), $"Row 3 has incorrect items (cell {cell})");
        Assert.That(Check(parser.Rows[3], out cell, "3", "FR", "France", "Paris", "48.856613", "2.352222", "\\\"France\\\" - home of the city of \\\"Paris\\\"."), $"Row 4 has incorrect items (cell {cell})");
    }

    [Test]
    public void Parser_UnquotedCells_AreHandled()
    {
        var parser = NewParser("ok-unquoted");
        var cell = -1;

        Assert.That(parser.RowCount, Is.EqualTo(4));
        Assert.That(Check(parser.Rows[0], out cell, "ID", "CODE", "NAME", "CAPITAL", "LATITUDE", "LONGITUDE", "COMMENT"), $"Row 1 has incorrect items (cell {cell})");
        Assert.That(Check(parser.Rows[1], out cell, "1", "USA", "North America", "Washington", "47.751076", "-120.740135", "The USA and capital."), $"Row 2 has incorrect items (cell {cell})");
        Assert.That(Check(parser.Rows[2], out cell, "2", "UK", "United Kingdom", "London", "51.507351", "-0.127758", "The UK - whose capital is London."), $"Row 3 has incorrect items (cell {cell})");
        Assert.That(Check(parser.Rows[3], out cell, "3", "FR", "France", "Paris", "48.856613", "2.352222", "France - home of the city of Paris."), $"Row 4 has incorrect items (cell {cell})");
    }

    [Test]
    public void Parser_EmptyCells_AreAllowed()
    {
        var parser = NewParser("ok-empty-cells");
        var cell = -1;

        Assert.That(parser.RowCount, Is.EqualTo(1));
        Assert.That(Check(parser.Rows[0], out cell, "1", "2", "", "4", "", "", "7"), $"Row 1 has incorrect items (cell {cell})");
    }

    [Test]
    public void Parser_Whitespace_IsHandled()
    {
        var parser = NewParser("ok-whitespace");
        var cell = -1;

        Assert.That(parser.RowCount, Is.EqualTo(1));
        Assert.That(Check(parser.Rows[0], out cell, "1", "2", "", "  4 ", "", "", "7"), $"Row 1 has incorrect items (cell {cell})");
    }

    [Test]
    public void Parser_EmptyFile_IsHandled()
    {
        var parser = NewParser("ok-empty");

        Assert.That(parser.RowCount, Is.EqualTo(0));
    }

    [Test]
    public void Parser_EmptyRows_AreHandled()
    {
        var parser = NewParser("ok-empty-rows");

        Assert.That(parser.RowCount, Is.EqualTo(6));
        Assert.That(parser.Rows[0].CellCount, Is.EqualTo(7), "Row 1 should have 7 cells");
        Assert.That(parser.Rows[1].CellCount, Is.EqualTo(0), "Row 2 should have 0 cells");
        Assert.That(parser.Rows[2].CellCount, Is.EqualTo(7), "Row 3 should have 7 cells");
        Assert.That(parser.Rows[3].CellCount, Is.EqualTo(0), "Row 4 should have 0 cells");
        Assert.That(parser.Rows[4].CellCount, Is.EqualTo(7), "Row 5 should have 7 cells");
        Assert.That(parser.Rows[5].CellCount, Is.EqualTo(7), "Row 6 should have 7 cells");
    }

    [Test]
    public void Parser_JaggedRows_AreHandled()
    {
        var parser = NewParser("ok-jagged-rows");

        Assert.That(parser.RowCount, Is.EqualTo(5));
        Assert.That(parser.Rows[0].CellCount, Is.EqualTo(7), "Row 1 should have 7 cells");
        Assert.That(parser.Rows[1].CellCount, Is.EqualTo(3), "Row 2 should have 3 cells");
        Assert.That(parser.Rows[2].CellCount, Is.EqualTo(4), "Row 3 should have 4 cells");
        Assert.That(parser.Rows[3].CellCount, Is.EqualTo(1), "Row 4 should have 1 cells");
        Assert.That(parser.Rows[4].CellCount, Is.EqualTo(7), "Row 5 should have 7 cells");
    }

    /// <summary>
    /// Checks all a row's cells, comparing each against the
    /// equivalent item in the expected parameter list.
    /// Populates the `cell` out parameter if a particular
    /// cell does not match.
    /// Throws an exception if the caller doesn;t provide
    /// enough expectetions for the cells.
    /// </summary>
    private bool Check(Row row, out int cell, params string[] expected)
    {
        cell = -1;
        if (expected.Length != row.CellCount)
            throw new Exception($"Incorrect 'expected' in 'Check' call (wanted {row.CellCount}, got {expected.Length}).");

        for (var i = 0; i < 7; i++)
        {
            cell = i + 1;
            if (expected[i] != row.Cells[i].Text) return false;
        }
        return true;
    }

    /// <summary>
    /// Creates a new Parser instance, converting the
    /// provided fixture name into a filename to load
    /// the contents.
    /// </summary>
    private static Parser NewParser(string fixtureName)
    {
        var filename = Path.Combine("Fixtures", fixtureName) + ".csv";
        return new Parser(filename);
    }

}
