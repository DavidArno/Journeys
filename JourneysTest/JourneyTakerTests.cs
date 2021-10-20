using Journeys;
using NUnit.Framework;
using static Journeys.Command;
using static Journeys.Direction;
using static NUnit.Framework.Assert;

namespace JourneysTest;

[TestFixture]
public static class JourneyTakerTests
{
    [TestCase(1, 1, East, 2, 1, East)]
    [TestCase(1, 1, South, 1, 0, South)]
    [TestCase(1, 1, West, 0, 1, West)]
    [TestCase(1, 1, North, 1, 2, North)]
    public static void JourneyStartingAsStartX_Y_Direction_WithForward_EndsAsEndX_Y_Direction(
        int startX,
        int startY,
        Direction startDirection,
        int endX,
        int endY,
        Direction endDirection)
    {
        var endPosition =
            JourneyTaker.MakeJourney(new Position(startX, startY, startDirection), new[] { Forward });

        Multiple(() => {
            AreEqual(endX, endPosition.X);
            AreEqual(endY, endPosition.Y);
            AreEqual(endDirection, endPosition.Facing);
        });
    }

    [TestCase(South, West)]
    [TestCase(East, South)]
    [TestCase(North, East)]
    [TestCase(West, North)]
    public static void JourneyStartingAs1_1_E_WithRight_EndsAs1_1_S(
        Direction direction,
        Direction expected)
    {
        var endPosition = JourneyTaker.MakeJourney(new Position(1, 1, direction), new[] { Right });

        Multiple(() => {
            AreEqual(1, endPosition.X);
            AreEqual(1, endPosition.Y);
            AreEqual(expected, endPosition.Facing);
        });
    }

    [TestCase(South, East)]
    [TestCase(East, North)]
    [TestCase(North, West)]
    [TestCase(West, South)]
    public static void JourneyStartingAs1_1_TestCaseDirection_WithLeft_EndsAs1_1_ExpectedResult(
        Direction direction,
        Direction expected)
    {
        var endPosition = JourneyTaker.MakeJourney(new Position(1, 1, direction), new[] { Left });

        Multiple(() => {
            AreEqual(1, endPosition.X);
            AreEqual(1, endPosition.Y);
            AreEqual(expected, endPosition.Facing);
        });
    }

    [Test]
    public static void JourneyStartingAs1_1_E_With_RFRFRFRF_EndsAs1_1_E()
    {
        var endPosition = JourneyTaker.MakeJourney(
            new Position(1, 1, East),
            new[] {
                Right,
                Forward,
                Right,
                Forward,
                Right,
                Forward,
                Right,
                Forward
            });

        Multiple(() => {
            AreEqual(1, endPosition.X);
            AreEqual(1, endPosition.Y);
            AreEqual(East, endPosition.Facing);
        });
    }

    [Test]
    public static void JourneyStartingAs3_2_N_With_FRRFLLFFRRLL_EndsAs3_3_N()
    {
        var endPosition = JourneyTaker.MakeJourney(
            new Position(3, 2, North),
            new[] {
                Forward,
                Right,
                Right,
                Forward,
                Left,
                Left,
                Forward,
                Forward,
                Right,
                Right,
                Forward,
                Left,
                Left
            });

        Multiple(() => {
            AreEqual(3, endPosition.X);
            AreEqual(3, endPosition.Y);
            AreEqual(North, endPosition.Facing);
        });
    }
}

