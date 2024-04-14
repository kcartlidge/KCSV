using KCSV.Models;

namespace KCSV.Tests;

public class ParserTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Parser_Optionally_HandlesTabDelimiters()
    {
        var tabbed = new string[] { "1\t2\t 3 \t 4,4.5 as one\t5" };
        var table = Parser.FromStrings(tabbed, Delimiters.Tab);

        Assert.That(table.RowCount, Is.EqualTo(1));
        Assert.That(table.Rows[0].CellCount, Is.EqualTo(5));
        Assert.That(table.Rows[0].AsCSV(), Is.EqualTo("\"1\",\"2\",\"3\",\"4,4.5 as one\",\"5\""));
    }

    [Test]
    public void Parser_LoadsFile_LF_NoneWindows()
    {
        using var parserMaker = new TableMaker();
        var table = parserMaker.UsingTemporaryFile("\n");

        Assert.That(table.RowCount, Is.EqualTo(4));
    }

    [Test]
    public void Parser_LoadsFile_CRLF_Windows()
    {
        using var parserMaker = new TableMaker();
        var table = parserMaker.UsingTemporaryFile("\r\n");

        Assert.That(table.RowCount, Is.EqualTo(4));
    }

    [Test]
    public void Parser_RowsAreNumbered_FromOne()
    {
        var table = Helper.LoadTable("ok-well-formed");

        Assert.That(table.RowCount, Is.EqualTo(4));
        for (var i = 0; i < table.RowCount; i++)
            Assert.That(table.Rows[i].RowNumber, Is.EqualTo(i + 1),
                        $"Found in row {i}");
    }

    [Test]
    public void Parser_RowsAreSplit_IntoCells()
    {
        var table = Helper.LoadTable("ok-well-formed");

        Assert.That(table.RowCount, Is.EqualTo(4));
        for (var i = 0; i < table.RowCount; i++)
            Assert.That(table.Rows[i].CellCount, Is.EqualTo(7),
                        $"Found in row {i}");
    }

    [Test]
    public void Parser_WellFormedCells_AreCorrect()
    {
        var table = Helper.LoadTable("ok-well-formed");
        var cell = -1;

        Assert.That(table.RowCount, Is.EqualTo(4));
        Assert.That(Helper.Check(table.Rows[0], out cell, "ID", "CODE", "NAME", "CAPITAL", "LATITUDE", "LONGITUDE", "COMMENT"), $"Row 1 has incorrect items (cell {cell})");
        Assert.That(Helper.Check(table.Rows[1], out cell, "1", "USA", "North America", "Washington", "47.751076", "-120.740135", "The USA and capital."), $"Row 2 has incorrect items (cell {cell})");
        Assert.That(Helper.Check(table.Rows[2], out cell, "2", "UK", "United Kingdom", "London", "51.507351", "-0.127758", "The UK - whose capital is London."), $"Row 3 has incorrect items (cell {cell})");
        Assert.That(Helper.Check(table.Rows[3], out cell, "3", "FR", "France", "Paris", "48.856613", "2.352222", "France - home of the city of Paris."), $"Row 4 has incorrect items (cell {cell})");
    }

    [Test]
    public void Parser_EmbeddedCommas_AreAllowed()
    {
        var table = Helper.LoadTable("ok-embedded-commas");
        var cell = -1;

        Assert.That(table.RowCount, Is.EqualTo(4));
        Assert.That(Helper.Check(table.Rows[0], out cell, "ID", "CODE", "NAME", "CAPITAL", "LATITUDE", "LONGITUDE", "COMMENT"), $"Row 1 has incorrect items (cell {cell})");
        Assert.That(Helper.Check(table.Rows[1], out cell, "1", "USA", "North America", "Washington", "47.751076", "-120.740135", "The USA, and capital."), $"Row 2 has incorrect items (cell {cell})");
        Assert.That(Helper.Check(table.Rows[2], out cell, "2", "UK", "United Kingdom", "London", "51.507351", "-0.127758", "The UK, whose capital is London."), $"Row 3 has incorrect items (cell {cell})");
        Assert.That(Helper.Check(table.Rows[3], out cell, "3", "FR", "France", "Paris", "48.856613", "2.352222", "France, home of the city of Paris."), $"Row 4 has incorrect items (cell {cell})");
    }

    [Test]
    public void Parser_EmbeddedQuotes_AreAllowed()
    {
        var table = Helper.LoadTable("ok-embedded-quotes");
        var cell = -1;

        Assert.That(table.RowCount, Is.EqualTo(4));
        Assert.That(Helper.Check(table.Rows[0], out cell, "ID", "CODE", "NAME", "CAPITAL", "LATITUDE", "LONGITUDE", "COMMENT"), $"Row 1 has incorrect items (cell {cell})");
        Assert.That(Helper.Check(table.Rows[1], out cell, "1", "USA", "North America", "Washington", "47.751076", "-120.740135", "The \\\"USA\\\" and capital."), $"Row 2 has incorrect items (cell {cell})");
        Assert.That(Helper.Check(table.Rows[2], out cell, "2", "UK", "United Kingdom", "London", "51.507351", "-0.127758", "The \\\"UK\\\" - whose capital is \\\"London\\\"."), $"Row 3 has incorrect items (cell {cell})");
        Assert.That(Helper.Check(table.Rows[3], out cell, "3", "FR", "France", "Paris", "48.856613", "2.352222", "\\\"France\\\" - home of the city of \\\"Paris\\\"."), $"Row 4 has incorrect items (cell {cell})");
    }

    [Test]
    public void Parser_UnquotedCells_AreHandled()
    {
        var table = Helper.LoadTable("ok-unquoted");
        var cell = -1;

        Assert.That(table.RowCount, Is.EqualTo(4));
        Assert.That(Helper.Check(table.Rows[0], out cell, "ID", "CODE", "NAME", "CAPITAL", "LATITUDE", "LONGITUDE", "COMMENT"), $"Row 1 has incorrect items (cell {cell})");
        Assert.That(Helper.Check(table.Rows[1], out cell, "1", "USA", "North America", "Washington", "47.751076", "-120.740135", "The USA and capital."), $"Row 2 has incorrect items (cell {cell})");
        Assert.That(Helper.Check(table.Rows[2], out cell, "2", "UK", "United Kingdom", "London", "51.507351", "-0.127758", "The UK - whose capital is London."), $"Row 3 has incorrect items (cell {cell})");
        Assert.That(Helper.Check(table.Rows[3], out cell, "3", "FR", "France", "Paris", "48.856613", "2.352222", "France - home of the city of Paris."), $"Row 4 has incorrect items (cell {cell})");
    }

    [Test]
    public void Parser_UnclosedQuotedLastCell_IsAllowed()
    {
        var csv = new string[] { "\"A\", \" B "};
        var cell = -1;

        var table = Parser.FromStrings(csv);
        Assert.That(table.RowCount, Is.EqualTo(1));
        Assert.That(table.Rows[0].CellCount, Is.EqualTo(2));
        Assert.That(Helper.Check(table.Rows[0], out cell, "A", " B "), $"Row 1 has incorrect items (cell {cell})");
    }

    [Test]
    public void Parser_EmptyCells_AreAllowed()
    {
        var table = Helper.LoadTable("ok-empty-cells");
        var cell = -1;

        Assert.That(table.RowCount, Is.EqualTo(1));
        Assert.That(Helper.Check(table.Rows[0], out cell, "1", "2", "", "4", "", "", "7"), $"Row 1 has incorrect items (cell {cell})");
    }

    [Test]
    public void Parser_Whitespace_IsHandled()
    {
        var table = Helper.LoadTable("ok-whitespace");
        var cell = -1;

        Assert.That(table.RowCount, Is.EqualTo(1));
        Assert.That(Helper.Check(table.Rows[0], out cell, "1", "2", "", "  4 ", "", "", "7"), $"Row 1 has incorrect items (cell {cell})");
    }

    [Test]
    public void Parser_EmptyFile_IsHandled()
    {
        var table = Helper.LoadTable("ok-empty");

        Assert.That(table.RowCount, Is.EqualTo(0));
    }

    [Test]
    public void Parser_EmptyRows_AreHandled()
    {
        var table = Helper.LoadTable("ok-empty-rows");

        Assert.That(table.RowCount, Is.EqualTo(6));
        Assert.That(table.Rows[0].CellCount, Is.EqualTo(7), "Row 1 should have 7 cells");
        Assert.That(table.Rows[1].CellCount, Is.EqualTo(0), "Row 2 should have 0 cells");
        Assert.That(table.Rows[2].CellCount, Is.EqualTo(7), "Row 3 should have 7 cells");
        Assert.That(table.Rows[3].CellCount, Is.EqualTo(0), "Row 4 should have 0 cells");
        Assert.That(table.Rows[4].CellCount, Is.EqualTo(7), "Row 5 should have 7 cells");
        Assert.That(table.Rows[5].CellCount, Is.EqualTo(7), "Row 6 should have 7 cells");
    }

    [Test]
    public void Parser_JaggedRows_AreHandled()
    {
        var table = Helper.LoadTable("ok-jagged-rows");

        Assert.That(table.RowCount, Is.EqualTo(5));
        Assert.That(table.Rows[0].CellCount, Is.EqualTo(7), "Row 1 should have 7 cells");
        Assert.That(table.Rows[1].CellCount, Is.EqualTo(3), "Row 2 should have 3 cells");
        Assert.That(table.Rows[2].CellCount, Is.EqualTo(4), "Row 3 should have 4 cells");
        Assert.That(table.Rows[3].CellCount, Is.EqualTo(1), "Row 4 should have 1 cells");
        Assert.That(table.Rows[4].CellCount, Is.EqualTo(7), "Row 5 should have 7 cells");
    }

    [Test]
    public void Parser_Fails_ForContentAfterClosingQuote()
    {
        var csv = new string[] { "row-one", "\"a\",\"b\" !,\"c\""};

        var ex = Assert.Throws<CSVException>(() => Parser.FromStrings(csv));
        Assert.That(ex.Row, Is.EqualTo(2));
        Assert.That(ex.Character, Is.EqualTo(9));
        Assert.That(ex.Message, Contains.Substring("Expected a cell delimiter"));
    }
}
