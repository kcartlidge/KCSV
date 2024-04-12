# KCSV

A simple-to-use reliable CSV-parsing library.

- Loads content from a file
  - Both CSV and Tabs supported
  - Works with both Linux/Mac/Unix and Windows line endings
- Accepts content as an array of rows as strings
  - Both CSV and Tabs supported
- Splits into a collection of rows with:
  - Original source row number
  - A collection of cells with each having:
    - A flag for if there were double-quotes in the input
    - The content, raw and pre-formatted with any quotes
- Optional pre-formatted row output
  - Both CSV and Tabs supported
  - Ready for writing back out, complete with quoting
- Flexible and accurate
  - Understands double-quoted and plain cells
  - Both CSV and Tabs supported
  - Allows embedded commas where quoted
  - Accepts unquoted full text (stops at a comma/tab)
  - Includes comprehensive tests
- *TODO:*
  - Full errors with input row and character position

## Running the tests

- Open a terminal/command prompt
- Navigate into the *solution* folder
- Run `dotnet test`
- Run `dotnet test -v n` for more detail

---

- [ChANGELOG](./CHANGELOG.md)
- [MIT License](./LICENSE.txt)
