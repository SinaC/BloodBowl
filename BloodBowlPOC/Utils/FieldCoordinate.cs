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

        public static bool AreEqual(FieldCoordinate a, FieldCoordinate b){
            return a.X == b.X && a.Y == b.Y;
        }

        public override string ToString()
        {
            return string.Format("[{0},{1}]", X,Y);
        }
    }
}
