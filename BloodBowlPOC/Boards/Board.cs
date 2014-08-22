using BloodBowlPOC.Utils;
using System;
namespace BloodBowlPOC.Boards
{
    public class Board
    {
        //Upper Left is 0,0                N, NE,  E, SE,  S, SW,  W, NW
        public static int[] DirectionsX = { 0, 1, 1, 1, 0, -1, -1, -1 };
        public static int[] DirectionsY = { -1, -1, 0, 1, 1, 1, 0, -1 };

        public static readonly  FieldCoordinate[] NorthThrowIn = { //From Down to Up
                                                           new FieldCoordinate(-1, -1),
                                                           new FieldCoordinate(0,-1),
                                                           new FieldCoordinate(1,1)
                                                       };
        public static readonly FieldCoordinate[] SouthThrowIn = { //From Up to Down
                                                           new FieldCoordinate(1,1),
                                                           new FieldCoordinate(0,1),
                                                           new FieldCoordinate(-1,1)
                                                       };
        public static readonly FieldCoordinate[] EastThrowIn = {  //To West
                                                           new FieldCoordinate(-1,1),
                                                           new FieldCoordinate(-1,0),
                                                           new FieldCoordinate(-1,-1)
                                                       };
        public static readonly FieldCoordinate[] WestThrowIn = { //To East
                                                           new FieldCoordinate(1,-1),
                                                           new FieldCoordinate(1,0),
                                                           new FieldCoordinate(1,1)
                                                       };

        public int SizeX { get; private set; }
        public int SizeY { get; private set; }
        public double[,] Probabilities { get; set; }

        public Board(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            Probabilities = new double[SizeX,SizeY];
        }

        public void Reset()
        {
            for (int y = 0; y < SizeY; y++)
                for (int x = 0; x < SizeX; x++)
                    Probabilities[x, y] = 0;
        }

        public FieldCoordinate[] GetThrowinRuler(FieldCoordinate coordinate)
        {
            if(coordinate.X<0){
                return WestThrowIn;
            }else if (coordinate.X >= SizeX) {
                return EastThrowIn;
            }else if (coordinate.Y < 0) { //the Above 2 prioritize the sieline
                return NorthThrowIn;
            }else if (coordinate.Y >= SizeY) {
                return SouthThrowIn;
            }

            throw new ArgumentException("Coordinate didn't make sense.","coordinate");
        }

        //TODO: make a boardUtility class of some sort maybe?
        public bool IsInbound(FieldCoordinate theSquare){
            return theSquare.X > 0 
                || theSquare.Y > 0 
                || theSquare.X < SizeX 
                || theSquare.Y < SizeY;
        }

        public FieldCoordinate GetLastInboundOnPath(FieldCoordinate theOrigin, FieldCoordinate theTarget)
        {
            if (IsInbound(theTarget)) {
                return theTarget;
            }

            var slope = (theTarget.Y - theOrigin.Y) / (double)(theTarget.X - theOrigin.X);

            //0, X, ou SizeX
            var xToUse = Math.Min(SizeX, Math.Max(0, theTarget.X));

            //0, Y calculé en X, ou SizeY
            var lastY = Math.Min(SizeY, Math.Max(0, (int)Math.Round((xToUse - theOrigin.X) * slope)));

            var lastCoord = new FieldCoordinate(xToUse, lastY);

            return lastCoord;
        }
    }
}
