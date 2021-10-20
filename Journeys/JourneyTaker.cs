
global using static Journeys.JourneyTaker;

using static Journeys.Command;
using static Journeys.Direction;

namespace Journeys;

public static class JourneyTaker
{
    public static Position MakeJourney(Position start, IEnumerable<Command> route)
        => route.Aggregate(start, ApplyCommandToPosition);

    private static Position ApplyCommandToPosition(Position position, Command command) => command switch {
            Forward => GoForward(position),
            Right => position with { Facing = RotateRight(position) },
            Left => position with { Facing = RotateLeft(position) },
            _ => throw new ArgumentOutOfRangeException(nameof(command))
        };

    private static Position GoForward(Position position) => position.Facing switch {
        East => position with { X = position.X + 1 },
        South => position with { Y = position.Y - 1 },
        West => position with { X = position.X - 1 },
        North => position with { Y = position.Y + 1 },
        _ => throw new ArgumentOutOfRangeException(nameof(position))
    };

    private static Direction RotateRight(Position position) => position.Facing switch {
        East => South,
        South => West,
        West => North,
        North => East,
        _ => throw new ArgumentOutOfRangeException(nameof(position))
    };

    private static Direction RotateLeft(Position position) => position.Facing switch {
        East => North,
        North => West,
        West => South,
        South => East,
        _ => throw new ArgumentOutOfRangeException(nameof(position))
    };
}

