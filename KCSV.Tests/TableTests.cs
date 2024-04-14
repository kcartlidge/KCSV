namespace KCSV.Tests;

public class TableTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Table_CountsRowsCorrectly()
    {
        var csv = new string[] { "1,2", "1,2", "1,2", "1,2" };
        var table = Parser.FromStrings(csv);

        Assert.That(table.Rows.Count, Is.EqualTo(4));
        Assert.That(table.RowCount, Is.EqualTo(4));
    }

    [Test]
    public void Table_HasMinCellCount()
    {
        var csv = new string[] { "1,2,3", "1,2", "1,2,3,4", "1,2,3" };
        var table = Parser.FromStrings(csv);

        Assert.That(table.MinCellCount, Is.EqualTo(2));
    }

    [Test]
    public void Table_HasMaxCellCount()
    {
        var csv = new string[] { "1,2,3", "1,2", "1,2,3,4", "1,2,3" };
        var table = Parser.FromStrings(csv);

        Assert.That(table.MaxCellCount, Is.EqualTo(4));
    }

    [Test]
    public void Table_KnowsWhenIsJagged()
    {
        var csv = new string[] { "1,2,3", "1,2", "1,2,3,4", "1,2,3" };
        var table = Parser.FromStrings(csv);

        Assert.That(table.IsJagged, Is.True);
    }

    [Test]
    public void Table_KnowsWhenIsNotJagged()
    {
        var csv = new string[] { "1,2,3", "1,2,3", "1,2,3", "1,2,3" };
        var table = Parser.FromStrings(csv);

        Assert.That(table.IsJagged, Is.False);
    }

    [Test]
    public void Table_SquareOff_RemovesJaggedness()
    {
        var csv = new string[] { "1,2,3", "1,2", "1,2,3,4", "1,2,3" };
        var table = Parser.FromStrings(csv);
        table.SquareOff();

        Assert.That(table.IsJagged, Is.False);
    }
}
