using Journeys;
using NUnit.Framework;
using SuccincT.Unions;
using System.Collections.Generic;
using System.Linq;
using static JourneysTest.TestFileSystem;
using static NUnit.Framework.Assert;

namespace JourneysTest
{
    [TestFixture]
    public static class FullJourneyTests
    {
        private static class TestDataProvider
        {
            public static IEnumerable<Either<Journey, ParserError>> TestCases
                => new[] {JourneyFileReader.ParseJourneyFile(TestFiles, @"c:\testfile3").ToList()[1]};
        }

        [TestCaseSource(typeof(TestDataProvider),nameof(TestDataProvider.TestCases))]
        public static void MikeHadlowThreeExampleJourneysAreAllValid(Either<Journey, ParserError> parseResult)
        {
            IsTrue(parseResult.IsJourney());
            
            var journey = parseResult.AsJourney();
            var actualEnd = JourneyTaker.MakeJourney(journey.Start, journey.Commands);

            Multiple(
                () => {
                    AreEqual(journey.End.X, actualEnd.X);
                    AreEqual(journey.End.Y, actualEnd.Y);
                    AreEqual(journey.End.Facing, actualEnd.Facing);
                });
        }
    }
}