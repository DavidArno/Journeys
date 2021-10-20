using Journeys;
using NUnit.Framework;
using System.Linq;
using static Journeys.Command;
using static Journeys.Direction;
using static JourneysTest.TestFileSystem;
using static NUnit.Framework.Assert;

namespace JourneysTest;

[TestFixture]
public static class JourneyFileReaderTests
{
    [Test]
    public static void TestFile1CanBeCorrectlyParsed()
    {
        var journeys = JourneyFileReader.ParseJourneyFile(TestFiles, "/testfile1").ToList();
        var journey = journeys[0].AsJourney();
        var commands = journey.Commands.ToList();

        Multiple(() => {
            AreEqual(1, journeys.Count);

            AreEqual(1, journey.Start.X);
            AreEqual(1, journey.Start.Y);
            AreEqual(East, journey.Start.Facing);

            AreEqual(8, commands.Count);
            AreEqual(Right, commands[0]);
            AreEqual(Forward, commands[7]);

            AreEqual(1, journey.End.X);
            AreEqual(1, journey.End.Y);
            AreEqual(East, journey.End.Facing);
        });
    }

    [Test]
    public static void TestFile2CanBeCorrectlyParsed()
    {
        var journeys = JourneyFileReader.ParseJourneyFile(TestFiles, "/testfile2").ToList();
        var journey = journeys[0].AsJourney();
        var commands = journey.Commands.ToList();

        Multiple(() => {
            AreEqual(1, journeys.Count);

            AreEqual(3, journey.Start.X);
            AreEqual(2, journey.Start.Y);
            AreEqual(North, journey.Start.Facing);

            AreEqual(13, commands.Count);
            AreEqual(Forward, commands[0]);
            AreEqual(Left, commands[12]);

            AreEqual(3, journey.End.X);
            AreEqual(3, journey.End.Y);
            AreEqual(North, journey.End.Facing);
        });
    }

    [Test]
    public static void TestFile3ContainsThreeJourneys()
    {
        var journeys = JourneyFileReader.ParseJourneyFile(TestFiles, "/testfile3").ToList();

        Multiple(() => {
            AreEqual(3, journeys.Count);
            IsTrue(journeys[0].IsJourney());
            IsTrue(journeys[1].IsJourney());
            IsTrue(journeys[2].IsJourney());
        });
    }

    [Test]
    public static void TestFile4ContainsAParserErrorAndAJourney()
    {
        var parseResults = JourneyFileReader.ParseJourneyFile(TestFiles, "/testfile4").ToList();

        Multiple(() => {
            AreEqual(3, parseResults.Count);
            IsFalse(parseResults[0].IsJourney());
            IsTrue(parseResults[1].IsJourney());
            IsFalse(parseResults[2].IsJourney());
        });
    }

    [Test]
    public static void TestFile5ContainsNoJourneys()
    {
        var parseResults = JourneyFileReader.ParseJourneyFile(TestFiles, "/testfile5").ToList();

        AreEqual(0, parseResults.Count);
    }

    [Test]
    public static void ParsingNonExistentFileProducesParserError()
    {
        var parseResults = JourneyFileReader.ParseJourneyFile(TestFiles, @"/no-such-file").ToList();

        Multiple(() => {
            AreEqual(1, parseResults.Count);
            IsFalse(parseResults[0].IsJourney());
        });
    }
}