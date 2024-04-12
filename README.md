# KCSV

A simple-to-use reliable CSV-parsing library.

- Loads CSV from a file
  - Is fine with both Linux/Mac/Unix and Windows line endings
  - Can also be fed arrays of CSV rows as strings
- Splits into a collection of rows with:
  - Original CSV row number
  - A collection of cells with each having:
    - A flag for if there were double-quotes in the input
    - The content, raw and pre-formatted with any quotes
    - Can provide row content pre-formatted for writing back out
- Flexible and accurate
  - Understands double-quoted and plain cells
  - Allows embedded commas where quoted
  - Accepts unquoted full text (stops at a comma)
  - Includes comprehensive tests
- *TODO:*
  - Supports both CSV *and* tab-delimited rows
  - Returns errors with input row and character position

## Running the tests

- Open a terminal/command prompt
- Navigate into the *solution* folder
- Run `dotnet test`
- Run `dotnet test -v n` for more detail

---

- [ChANGELOG](./CHANGELOG.md)
- [MIT License](./LICENSE.txt)
