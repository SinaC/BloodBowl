using System;

namespace BloodBowlPOC.Utils
{
    //http://csharpindepth.com/Articles/General/Singleton.aspx#lazy
    public sealed class Dice
    {
        private static readonly Lazy<Dice> Lazy = new Lazy<Dice>(() => new Dice());
        private readonly Random _random;

        public static Dice Instance { get { return Lazy.Value; } }

        private Dice()
        {
            _random = new Random();
        }

        public int Roll(int faceCount, int rollCount)
        {
            int total = 0;
            for (int i = 0; i < rollCount; i++)
                total += _random.Next(1, faceCount + 1);
            return total;
        }
    }
}
