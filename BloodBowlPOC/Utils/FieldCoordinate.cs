namespace BloodBowlPOC.Utils
{
    /// <summary>
    /// Class representing a square coordinate.
    /// </summary>
    public struct FieldCoordinate
    {
        public readonly int X;
        public readonly int Y;

        public FieldCoordinate(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
