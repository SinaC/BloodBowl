using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBowlPOC.Utils
{
    /// <summary>
    /// Class representing a square coordinate.
    /// </summary>
    public struct FieldCoordinate
    {
        public readonly int X;
        public readonly int Y;

        public FieldCoordinate(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
