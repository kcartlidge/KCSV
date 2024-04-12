namespace KCSV;

/// <summary>
/// A CSV parser exception, with line/character position.
/// </summary>
public class CSVException : Exception
{
    /// <summary>
    /// A CSV parsing exception, with position.
    /// </summary>
    /// <param name="message">
    /// The description portion of the message.
    /// Line number and Position will be inserted automatically.
    /// </param>
    public CSVException(int lineNumber, int position, string message)
        : base($"CSV {lineNumber}:{position}  {message}") { }
}
