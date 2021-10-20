using SuccincT.Unions;

namespace Journeys;

public static class JourneyFileReader
{
    public static IEnumerable<Either<Journey, ParserError>> ParseJourneyFile(
        IFileSystem fileSystem,
        string filePath)
    {
        try
        {
            var lines = fileSystem.File.ReadAllLines(filePath);
            return new ParserResultsEnumerable(lines);
        }
        catch (IOException)
        {
            return new[] {
                new Either<Journey, ParserError>(new ParserError(0, $"Could not read the file '{filePath}'."))
            };
        }
    }

    public static bool IsJourney(this Either<Journey, ParserError> either) => either.IsLeft;

    public static Journey AsJourney(this Either<Journey, ParserError> either) => either.Left;

    public static ParserError AsParserError(this Either<Journey, ParserError> either) => either.Right;

}


