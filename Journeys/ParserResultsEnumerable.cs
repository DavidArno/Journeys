using SuccincT.Parsers;
using SuccincT.Unions;
using System.Collections;
using static Journeys.Command;
using static Journeys.Direction;
using static SuccincT.Options.Option;

namespace Journeys;

internal class ParserResultsEnumerable : IEnumerable<Either<Journey, ParserError>>
{
    private readonly ParserEnumerator _enumerator;

    public ParserResultsEnumerable(string[] lines) => _enumerator = new ParserEnumerator(lines);

    public IEnumerator<Either<Journey, ParserError>> GetEnumerator() => _enumerator;

    IEnumerator IEnumerable.GetEnumerator() => _enumerator;

    private class ParserEnumerator : IEnumerator<Either<Journey, ParserError>>
    {
        private readonly string[] _lines;

        private LineMode _lineMode = LineMode.StartLine;
        private int _currentLine;

        public ParserEnumerator(string[] lines) => _lines = lines;

        public bool MoveNext()
        {
            if (_currentLine >= _lines.Length) return false;

            var journey = new Journey();

            while (true)
            {
                if (_currentLine >= _lines.Length)
                {
                    if (_lineMode == LineMode.ParseFailed) return false;

                    Current = new Either<Journey, ParserError>(
                        new ParserError(_currentLine, "Incomplete journey at the end of the file."));

                    return true;
                }

                var line = _lines[_currentLine++];
                try
                {
                    if (line.Trim().Length == 0 || _lineMode == LineMode.ParseFailed)
                    {
                        _lineMode = LineMode.StartLine;
                        continue;
                    }

                    (_lineMode, journey) = _lineMode switch {
                        LineMode.StartLine => (
                            LineMode.CommandLine,
                            journey with { Start = ParsePositionLine(line, "start") }),
                        LineMode.CommandLine => (
                            LineMode.EndLine,
                            journey with { Commands = ParseCommandLine(line).ToList() }),
                        LineMode.EndLine or _ => (
                            LineMode.StartLine,
                            journey with { End = ParsePositionLine(line, "end") })
                    };

                    if (_lineMode != LineMode.StartLine) continue;

                    Current = new Either<Journey, ParserError>(journey);
                    return true;
                }
                catch (ParseException e)
                {
                    Current = new Either<Journey, ParserError>(new ParserError(_currentLine, e.Message));
                    _lineMode = LineMode.ParseFailed;
                    return true;
                }
            }
        }

        public Either<Journey, ParserError> Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose() { }

        public void Reset() { }

        private static IEnumerable<Command> ParseCommandLine(string line)
            => from character in line
                select character switch {
                    'F' => Forward,
                    'L' => Left,
                    'R' => Right,
                    var c => throw new ParseException($"Unexpected character, '{c}' found in commands.")
                };

        private static Position ParsePositionLine(string line, string positionName)
        {
            var rawPositionData = line.Split(' ');
            if (rawPositionData.Length is var count && count != 3)
            {
                throw new ParseException(
                    $"Cannot parse {positionName} position. Expected a line in the format 'X·Y·Facing', " +
                    $"but line contains {count} items.");
            }

            return new Position(
                ParseNumericValue(rawPositionData[0], "X"),
                ParseNumericValue(rawPositionData[1], "Y"),
                ParseDirection(rawPositionData[2]));
        }

        private static int ParseNumericValue(string rawValue, string axis) => rawValue.TryParseInt() switch {
            (Some, var value) => value,
            _ => throw new ParseException($"Invalid number '{rawValue}' found for {axis} coordinate.")
        };

        private static Direction ParseDirection(string directionCharacter) => directionCharacter switch {
            "E" => East,
            "S" => South,
            "W" => West,
            "N" => North,
            var c => throw new ParseException(
                $"Invalid character, '{c}' found instead of a direction character.")
        };

        private class ParseException : Exception
        {
            public ParseException(string message) : base(message) { }
        }

        private enum LineMode { StartLine, CommandLine, EndLine, ParseFailed }
    }
}

