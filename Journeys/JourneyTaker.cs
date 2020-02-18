using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static Journeys.Command;
using static Journeys.Direction;

namespace Journeys
{
    public static class JourneyTaker
    {
        public static Position MakeJourney(Position start, IEnumerable<Command> route)
            => route.Aggregate(start, ApplyCommandToPosition);

        private static Position ApplyCommandToPosition(Position position, Command command)
            => (command, position) switch {
                (Forward, { Facing: var facing, X: var x, Y: var y }) => GoForward(x, y, facing),
                (Right, { Facing: var facing, X: var x, Y: var y }) => new Position(x, y, RotateRight(facing)),
                (Left, { Facing: var facing, X: var x, Y: var y }) => new Position(x, y, RotateLeft(facing)),
                _ => throw new SwitchExpressionException()
            };

        private static Position GoForward(int x, int y, Direction facing)
            => facing switch {
                East => new Position(x + 1, y, East),
                South => new Position(x, y - 1, South),
                West => new Position(x - 1, y, West),
                _ => new Position(x, y + 1, North)
            };

        private static Direction RotateRight(Direction current)
            => current switch {
                East => South,
                South => West,
                West => North,
                _ => East
            };

        private static Direction RotateLeft(Direction current)
            => current switch {
                East => North,
                North => West,
                West => South,
                _ => East
            };
    }
}
