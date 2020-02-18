using System.Collections.Generic;

namespace Journeys
{
    public readonly struct Journey
    {
        public Position Start { get; }
        public IEnumerable<Command> Commands { get; }
        public Position End { get; }

        public Journey(Position start, IEnumerable<Command> commands, Position end)
            => (Start, Commands, End) = (start, commands, end);
    }
}
