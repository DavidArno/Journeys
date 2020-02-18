namespace Journeys
{
    public readonly struct ParserError
    {
        public int LineNumber { get; }
        public string Error { get; }

        public ParserError(int lineNumber, string error) => (LineNumber, Error) = (lineNumber, error);
    }
}
