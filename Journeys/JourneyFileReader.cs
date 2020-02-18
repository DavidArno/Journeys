using SuccincT.Parsers;
using SuccincT.Unions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using static Journeys.Command;
using static Journeys.Direction;
using static SuccincT.Options.Option;

namespace Journeys
{
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

        private class ParserResultsEnumerable : IEnumerable<Either<Journey, ParserError>>
        {
            private readonly ParserEnumerator _enumerator;

            public ParserResultsEnumerable(string[] lines) => _enumerator = new ParserEnumerator(lines);

            public IEnumerator<Either<Journey, ParserError>> GetEnumerator() => _enumerator;

            IEnumerator IEnumerable.GetEnumerator() => _enumerator;
        }

        private class ParserEnumerator : IEnumerator<Either<Journey, ParserError>>
        {
            private readonly string[] _lines;

            private LineMode _lineMode = LineMode.StartLine;
            private int _currentLine;

            public ParserEnumerator(string[] lines) => _lines = lines;

            public bool MoveNext()
            {
                if (_currentLine >= _lines.Length) return false;

                var journeyBuilder = new JourneyBuilder();

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
                        if (line.Trim().Length == 0)
                        {
                            _lineMode = LineMode.StartLine;
                            continue;
                        }

                        switch (_lineMode)
                        {
                            case LineMode.ParseFailed: break;

                            case LineMode.StartLine:
                                journeyBuilder.Start = ParsePositionLine(line, "start");
                                _lineMode = LineMode.CommandLine;
                                break;

                            case LineMode.CommandLine:
                                journeyBuilder.Commands = ParseCommandLine(line).ToList();
                                _lineMode = LineMode.EndLine;
                                break;

                            case LineMode.EndLine:
                                journeyBuilder.End = ParsePositionLine(line, "end");
                                Current = new Either<Journey, ParserError>(journeyBuilder.Build());
                                _lineMode = LineMode.StartLine;
                                return true;
                        }
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

            object? IEnumerator.Current => Current;

            public void Dispose() {}

            public void Reset() {}

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

            private static int ParseNumericValue(string rawValue, string axis)
                => rawValue.TryParseInt() switch {
                    (Some, var value) => value,
                    _ => throw new ParseException($"Invalid number '{rawValue}' found for {axis} coordinate.")
                };

            private static Direction ParseDirection(string directionCharacter)
                => directionCharacter switch {
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

            private struct JourneyBuilder
            {
                public Position Start { get; set; }
                public IEnumerable<Command> Commands { get; set; }
                public Position End { get; set; }

                public Journey Build() => new Journey(Start, Commands!, End);
            }

            private enum LineMode { StartLine, CommandLine, EndLine, ParseFailed }
        }
    }
}
