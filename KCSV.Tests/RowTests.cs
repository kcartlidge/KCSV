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
}
