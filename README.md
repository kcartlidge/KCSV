# KCSV

A simple-to-use reliable CSV-parsing library.

[Available on Nuget](https://www.nuget.org/packages/KCSV/)

## Contents

- [Features](#features)
- [Usage](#usage)
  - [Loading CSV/Tab content from a file](#loading-csvtab-content-from-a-file)
  - [Streaming CSV/Tab content from a file](#streaming-csvtab-content-from-a-file)
  - [Passing in CSV/Tab content from strings](#passing-in-csvtab-content-from-strings)
  - [The Table model, methods, and hierarchy](#the-table-model-methods-and-hierarchy)
- [For developers working on KCSV itself](#for-developers-working-on-kcsv-itself)
  - [Running the tests](#running-the-tests)
  - [Creating a new version for Nuget](#creating-a-new-version-for-nuget)
  - [Forcing another project to get the latest from Nuget](#forcing-another-project-to-get-the-latest-from-nuget)
- [CHANGELOG](./CHANGELOG.md)
- [MIT License](./LICENSE.txt)

## Features

- **Uses NetStandard 2.0 for high compatibility**
- **Loads or streams content from a file**
  - Both **CSV** and **Tabs** supported
  - Works with both Linux/Mac/Unix and Windows line endings
- **Accepts content as an array of rows as strings**
  - Both **CSV** and **Tabs** supported
- Splits into a *collection of rows* with:
  - Original source row number
  - A *collection of cells* with each having:
    - A flag for if there were double-quotes in the input
    - The content, raw and pre-formatted with any quotes
- Checks for *jaggedness* in the parsed table
  - Defined as rows with less cells than the widest row
  - Optionally removes jaggedness by padding with empty cells
- Optional *pre-formatted* row *output*
  - Both CSV and Tabs supported
  - Ready for writing back out, complete with quoting
- **Flexible and accurate**
  - Understands double-quoted and plain cells
  - Both CSV and Tabs supported
  - Allows embedded commas where quoted
  - Accepts unquoted full text (stops at a comma/tab)
  - *Includes comprehensive tests*

## Usage

Remember that despite the name Tab-delimited is also supported.
Specifying the delimiter is optional; Comma is the default.

### Loading CSV/Tab content from a file

This loads all the rows in one hit.

``` cs
var table = Parser.LoadTable("file.csv", Delimiters.Comma);
var table = Parser.LoadTable("file.tab", Delimiters.Tab);
```

### Streaming CSV/Tab content from a file

This uses a forward-only stream of rows fetched one at
a time from a file on demand.  Lighter on resources.

``` cs
using (var stream = Parser.StreamTable("file.csv", Delimiters.Comma))
{
  var rowNumber = 0;
  while(stream.EndOfStream == false)
  {
    var row = stream.NextRow();
    Console.WriteLine($"Row {++rowNumber} has {row.CellCount} cell(s)")
  }
}
```

### Passing in CSV/Tab content from strings

``` cs
var csv = new string[]
{
  "ID,Name",
  "1, \"Davey Jones\"",
  "2, \"Don Atello\"",
};
var table = Parser.FromString(csv, Delimiters.Comma);
```

### The Table model, methods, and hierarchy

- Use `LoadTable(...)`, `StreamTable`, or `FromString(...)`
  - See the examples above for details
- All data whether loaded or taken from strings ends in a `Table`
- The `Table` has row stats and a collection of `Rows`
- A `Row` has cell stats and contents

``` text
Table
  Rows          - The individual rows of the CSV
  RowCount      - The number of rows found in the CSV
  MinCellCount  - The cell count for the narrowest row
  MaxCellCount  - The cell count for the widest row
  IsJagged      - Do any rows have less cells than others?

  SquareOff()   - Ensures all rows have the same amount of cells by adding extra (empty) ones where necessary
```

`Rows` is a readonly collection of type `Row`.

``` text
Row
  RowNumber   - The original CSV row number
  Cells       - The parsed cells from the row
  CellCount   - The number of cells in the row

  AsCSV()     - Returns the row as a CSV string, with cells quoted as per the original file
  AsTabbed()  - Returns the row as a Tab-delimited string, with cells quoted as per the original file
```

`Cells` is a readonly collection of type `Cell`.

``` text
Cell
  IsQuoted   - Were there double-quotes in the input CSV?
  Text       - The text, with any quoting from the original CSV content removed
  Formatted  - The text, wrapped in quotes if the originalCSV content included them
```

---

## For developers working on KCSV itself

If you only intend making use of KCSV in your own projects read no further.

### Running the tests

This library is NetStandard 2 for compatibility.
*The tests however were created under DotNet 8 and no guarantees are offered for earlier versions.*

- Open a terminal/command prompt
- Navigate into the *solution* folder
- Run `dotnet test`
- Run `dotnet test -v n` for more detail

### Creating a new version for Nuget

The `KCSV/KCSV.csproj` file contains Nuget settings. Within that file, update the version number then create the Nuget package:

``` shell
cd KCSV
dotnet build -c Release
dotnet pack -c Release
```

The `pack` command shouldn't be needed; the `build` should also create the package.
The output from `build` and `pack` will advise where the `.nupkg` package file is.
This package file is what gets uploaded in the Nuget UI.

### Forcing another project to get the latest from Nuget

It sometimes takes a while for a new version to be available after pushing. You may be able to speed up the process:

``` shell
cd <other-project>
dotnet restore --no-cache
```
