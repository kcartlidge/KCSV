using System;
using System.IO;
using KCSV.Models;

namespace KCSV.Tests;

public class RowTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Row_ToCSV_WithNoContent_ReturnsValidCSV()
    {
        var csv = new string[] { "" };
        var parser = new Parser(csv);

        Assert.That(parser.RowCount, Is.EqualTo(1));
        Assert.That(parser.Rows[0].AsCSV(), Is.EqualTo(""));
    }

    [Test]
    public void Row_ToCSV_WithUnquotedCells_ReturnsValidCSV()
    {
        var csv = new string[] { " 1 ,  2, 3  " };
        var parser = new Parser(csv);

        Assert.That(parser.RowCount, Is.EqualTo(1));
        Assert.That(parser.Rows[0].AsCSV(), Is.EqualTo("1,2,3"));
    }

    [Test]
    public void Row_ToCSV_WithQuotedCells_RetainsQuotedWhitespace()
    {
        var csv = new string[] { "\" 1\" , \" 2\", \"3\"  " };
        var parser = new Parser(csv);

        Assert.That(parser.RowCount, Is.EqualTo(1));
        Assert.That(parser.Rows[0].AsCSV(), Is.EqualTo("\" 1\",\" 2\",\"3\""));
    }

    [Test]
    public void Row_ToCSV_WithSomeQuotedCells_ReturnsValidCSV()
    {
        var csv = new string[] { " 1 , \" 2\", 3  " };
        var parser = new Parser(csv);

        Assert.That(parser.RowCount, Is.EqualTo(1));
        Assert.That(parser.Rows[0].AsCSV(), Is.EqualTo("1,\" 2\",3"));
    }

    [Test]
    public void Row_ToCSV_WithTabDelimiters_ReturnsAllCellsQuoted()
    {
        var tabbed = new string[] { " 1 \t \" 2\"\t 3  " };
        var parser = new Parser(tabbed, Delimiters.Tab);

        Assert.That(parser.RowCount, Is.EqualTo(1));
        Assert.That(parser.Rows[0].AsCSV(), Is.EqualTo("\"1\",\" 2\",\"3\""));
    }

    [Test]
    public void Row_ToTabbed_WithCSV_ReturnsWithEmbeddedTabsEscaped()
    {
        var csv = new string[] { " 1 , \" \t2\", 3  " };
        var parser = new Parser(csv);

        Assert.That(parser.RowCount, Is.EqualTo(1));
        Assert.That(parser.Rows[0].AsTabbed(), Is.EqualTo("1\t\" \\t2\"\t3"));
    }

    [Test]
    public void Row_ToTabbed_WithTabDelimited_ReturnsWithOriginalQuoting()
    {
        var tabbed = new string[] { " 1 \t \" 2\" \t 3  " };
        var parser = new Parser(tabbed, Delimiters.Tab);

        Assert.That(parser.RowCount, Is.EqualTo(1));
        Assert.That(parser.Rows[0].AsTabbed(), Is.EqualTo("1\t\" 2\"\t3"));
    }
}
