using System;
using System.IO.Abstractions;

namespace Journeys
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: Journeys <journey file>\n");
                Environment.Exit(0);
            }

            foreach (var result in JourneyFileReader.ParseJourneyFile(new FileSystem(), args[0]))
            {
                if (result.IsJourney())
                {
                    var journey = result.AsJourney();
                    var actualEndLocation = JourneyTaker.MakeJourney(journey.Start, journey.Commands);

                    Console.Write(
                        $"Robot starts at {journey.Start}\n" +
                        $"Takes route {string.Join(',', journey.Commands)}.\n" +
                        $"Expected to end at {journey.End}, ");

                    Console.Write(
                        journey.End == actualEndLocation
                            ? "which it does. Journey is valid.\n\n"
                            : $"but it actually ends at {actualEndLocation}. Invalid journey.\n\n");
                }
                else
                {
                    var error = result.AsParserError();

                    Console.WriteLine(
                        error.LineNumber == 0
                            ? $"Error: {error.Error}"
                            : $"Error on line {error.LineNumber}: {error.Error}\n");
                }
            }
        }
    }
}
