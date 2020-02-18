using System;

namespace Journeys
{
    public readonly struct Position
    {
        public int X { get; }
        public int Y { get; }
        public Direction Facing { get; }

        public override bool Equals(object? obj) => obj is Position other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y, (int)Facing);

        public Position(int x, int y, Direction facing) => (X, Y, Facing) = (x, y, facing);

        public static bool operator ==(Position p1, Position p2)
            => p1.X == p2.X && p1.Y == p2.Y && p1.Facing == p2.Facing;

        public static bool operator !=(Position p1, Position p2) => !(p1 == p2);

        public override string ToString() => $"{X}, {Y}, {Facing}";
    }
}
