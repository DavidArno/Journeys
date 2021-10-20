namespace Journeys;

public enum Command { Forward, Left, Right }

public enum Direction { North, East, South, West }

public readonly record struct Journey(Position Start, IEnumerable<Command> Commands, Position End);

public readonly record struct ParserError(int LineNumber, string Error);

public readonly record struct Position(int X, int Y, Direction Facing)
{
    public override string ToString() => $"{X}, {Y}, {Facing}";
}