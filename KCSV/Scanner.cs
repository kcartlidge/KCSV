using System.Linq;

namespace KCSV
{
    /// <summary>
    /// Provides safe access to a string of text.
    /// Using this allows easy scan-forwards, peek-ahead,
    /// and status flags, without the risk of accessing
    /// via an index out of bounds.
    /// </summary>
    internal class Scanner
    {
        /// <summary>Returned if consumed beyond the end.</summary>
        public readonly char Zero = (char)0;

        /// <summary>Is there more to be consumed?</summary>
        public bool HasMore => Pos < Max;

        /// <summary>Is there nothing left to be consumed?</summary>
        public bool AllDone => HasMore == false;

        private readonly string Text = "";
        private int Pos = 0;
        private int Max = 0;

        /// <summary>
        /// Create a new scanner to scan across some text.
        /// </summary>
        /// <param name="text">The text to scan across.</param>
        public Scanner(string text)
        {
            Text = text ?? "";
            Pos = 0;
            Max = Text.Length;
        }

        /// <summary>Gets the current position in the input.</summary>
        /// <returns>The current 0-based offset within the text.</returns>
        public int Position => Pos;

        /// <summary>
        /// Consume a single character and move to the next.
        /// If nothing to consume, the Zero character is returned.
        /// </summary>
        /// <returns>The character consumed, or Zero.</returns>
        public char Scan()
        {
            if (AllDone) return Zero;
            return Text[Pos++];
        }

        /// <summary>
        /// Move past the next character (if there is one).
        /// </summary>
        public void Skip()
        {
            if (HasMore) Pos++;
        }

        /// <summary>
        /// Move back a character (if there is one).
        /// </summary>
        public void Back()
        {
            if (Pos > 0) Pos--;
        }

        /// <summary>
        /// Skip over all available characters from the current
        /// location until a character is reached which is NOT
        /// one of the characters passed in.
        /// The scanner will be left positioned at the character
        /// it matched.
        /// </summary>
        /// <param name="unwanted">
        /// A collection of characters any of which will be
        /// skipped over and ignored.
        /// </param>
        public void SkipOver(params char[] unwanted)
        {
            while (HasMore)
            {
                if (unwanted.Contains(Peek()) == false) return;
                Skip();
            }
        }

        /// <summary>
        /// Scan a single character and DO NOT move to the next.
        /// This allows actions based on upcoming characters whilst
        /// leaving them available in the scanner.
        /// If nothing is left, the Zero character is returned.
        /// </summary>
        /// <returns>The character found or Zero.</returns>
        public char Peek()
        {
            if (AllDone) return Zero;
            return Text[Pos];
        }
    }
}
