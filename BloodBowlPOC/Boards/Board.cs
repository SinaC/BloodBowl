using System;
using System.Collections.Generic;
using BloodBowlPOC.Actions;
using BloodBowlPOC.Utils;
using BloodBowlPOC.ViewModels;

namespace BloodBowlPOC.Boards
{
    public class Board : IBoard
    {
        //Upper Left is 0,0                N, NE,  E, SE,  S, SW,  W, NW
        public static int[] DirectionsX = {0, 1, 1, 1, 0, -1, -1, -1};
        public static int[] DirectionsY = {-1, -1, 0, 1, 1, 1, 0, -1};

        public static readonly FieldCoordinate[] NorthThrowIn =
            {
                //From Down to Up
                new FieldCoordinate(-1, -1),
                new FieldCoordinate(0, -1),
                new FieldCoordinate(1, 1)
            };
        public static readonly FieldCoordinate[] SouthThrowIn =
            {
                //From Up to Down
                new FieldCoordinate(1, 1),
                new FieldCoordinate(0, 1),
                new FieldCoordinate(-1, 1)
            };
        public static readonly FieldCoordinate[] EastThrowIn =
            {
                //To West
                new FieldCoordinate(-1, 1),
                new FieldCoordinate(-1, 0),
                new FieldCoordinate(-1, -1)
            };
        public static readonly FieldCoordinate[] WestThrowIn =
            {
                //To East
                new FieldCoordinate(1, -1),
                new FieldCoordinate(1, 0),
                new FieldCoordinate(1, 1)
            };

        public int SizeX { get; private set; }
        public int SizeY { get; private set; }
        public double[,] Probabilities { get; set; }
        public double EpsilonProba { get; private set; }

        public Board(int sizeX, int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            Probabilities = new double[SizeX,SizeY];
            EpsilonProba = 0.000001;
        }

        public void Reset()
        {
            for (int y = 0; y < SizeY; y++)
                for (int x = 0; x < SizeX; x++)
                    Probabilities[x, y] = 0;
        }

        public void ComputeBounceProbabilities(FieldCoordinate point, int maxBounces, SelectableActions mode)
        {
            ActionBase startAction;
            // Throw the ball
            switch (mode)
            {
                case SelectableActions.Pass:
                    startAction = new BounceAction
                        {
                            Coordinate = point,
                            LastKnownInBound = point,
                            BounceLeft = maxBounces,
                        };
                    break;
                case SelectableActions.KickOff:
                    startAction = new KickOffAction
                        {
                            Target = point
                            //                LastKnownInBound = point,
                            //              BounceLeft = maxBounces,
                        };
                    break;
                default:
                    throw new ArgumentException(@"Mode selected didn't make sense", "mode");
            }

            Queue<ActionBase> actions = new Queue<ActionBase>();
            actions.Enqueue(startAction);

            // Perform actions
            int iterations = 0;
            while (actions.Count > 0)
            {
                ActionBase action = actions.Dequeue();
                iterations++;
                //
                List<ActionBase> subActions = action.Perform(this);
                subActions.ForEach(actions.Enqueue);
            }
            System.Diagnostics.Debug.WriteLine("Iterations:{0}", iterations);
        }

        public FieldCoordinate[] GetThrowinRuler(FieldCoordinate coordinate)
        {
            if (coordinate.X < 0)
                return WestThrowIn;
            if (coordinate.X >= SizeX)
                return EastThrowIn;
            if (coordinate.Y < 0) //the Above 2 prioritize the sieline
                return NorthThrowIn;
            if (coordinate.Y >= SizeY)
                return SouthThrowIn;

            throw new ArgumentException(@"Coordinate didn't make sense.", "coordinate");
        }

        //TODO: make a boardUtility class of some sort maybe?
        public bool IsInbound(FieldCoordinate theSquare)
        {
            return theSquare.X >= 0
                   && theSquare.Y >= 0
                   && theSquare.X < SizeX
                   && theSquare.Y < SizeY;
        }

        public FieldCoordinate GetLastInboundOnPath(FieldCoordinate theOrigin, FieldCoordinate theTarget)
        {
            if (theOrigin.X == theTarget.X)
            {
                // Vertical
                return new FieldCoordinate(theOrigin.X, Math.Max(0, Math.Min(theTarget.Y, SizeY - 1)));
            }

            if (theOrigin.Y == theTarget.Y)
            {
                // Horizontal
                return new FieldCoordinate(Math.Max(0, Math.Min(theTarget.X, SizeX - 1)), theOrigin.Y);
            }

            // ca a l'air mieux mais j'ai qd meme pu ecrire un unit test qui passe pas.

            int borderX = Math.Min(Math.Max(0, theTarget.X), SizeX - 1);
            int borderY = Math.Min(Math.Max(0, theTarget.Y), SizeY - 1);

            if (borderX == theTarget.X && borderY == theTarget.Y) // on sort pas -> pas de calcul
                return theTarget;

            int inBoundX;
            int inBoundY;

            if (Math.Abs(theTarget.X - borderX) > Math.Abs(theTarget.Y - borderY)) // On sort en x ou y ?
            {
                double slope = (double) (theTarget.Y - theOrigin.Y)/(theTarget.X - theOrigin.X);
                inBoundX = borderX;
                inBoundY = (int) (theOrigin.Y + slope*(borderX-0.5 - theOrigin.X));
            }
            else
            {
                double slope = (double) (theTarget.X - theOrigin.X)/(theTarget.Y - theOrigin.Y);
                inBoundX = (int) (theOrigin.X + slope*(borderY-0.5 - theOrigin.Y));
                inBoundY = borderY;
            }

            return new FieldCoordinate(inBoundX, inBoundY);

            //On flip les axes pour aller toujours en positif dans les 2 axes, ya peut etre plus facile mais
            //mon cerveau ne marchait plus.

            //var flipX = theOrigin.X > theTarget.X;
            //var flipy = theOrigin.Y > theTarget.Y;

            //var newOrigin = new FieldCoordinate(
            //    flipX ? SizeX - 1 - theOrigin.X : theOrigin.X,
            //    flipy ? SizeY - 1 - theOrigin.Y : theOrigin.Y
            //    );

            //var newTarget = new FieldCoordinate(
            //    flipX ? SizeX - 1 - theTarget.X : theTarget.X,
            //    flipy ? SizeY - 1 - theTarget.Y : theTarget.Y
            //    );

            //double slope = (newTarget.Y - newOrigin.Y) / (double)(newTarget.X - newOrigin.X);

            //var intersectionAtX = new FieldCoordinate(SizeX - 1, (int)Math.Round(newOrigin.Y + (SizeX - 1 - newOrigin.X) * slope));
            //var intersectionAtY = new FieldCoordinate((int)Math.Round((SizeY - 1 - newOrigin.Y) / slope + newOrigin.X), SizeY - 1);

            //var lastSquare = new FieldCoordinate(
            //        Math.Min(intersectionAtX.X, intersectionAtY.X),
            //        Math.Min(intersectionAtX.Y, intersectionAtY.Y)
            //    );

            //var unFlipped = new FieldCoordinate(
            //    flipX ? SizeX - 1 - lastSquare.X : lastSquare.X,
            //    flipy ? SizeY - 1 - lastSquare.Y : lastSquare.Y
            //    );

            //return unFlipped;
        }

        private static int Clamp(int value, int min, int max)
        {
            return Math.Min(Math.Max(min, value), max);
        }

        public FieldCoordinate GetLastInboundOnPath_Bresenham(FieldCoordinate origin, FieldCoordinate target)
        {
            if (origin.X == target.X) // Vertical
                return new FieldCoordinate(origin.X, Clamp(target.Y, 0, SizeY - 1));

            if (origin.Y == target.Y) // Horizontal
                return new FieldCoordinate(Clamp(target.X, 0, SizeX - 1), origin.Y);

            int borderX = Clamp(target.X, 0, SizeX - 1);
            int borderY = Clamp(target.Y, 0, SizeY - 1);

            if (borderX == target.X && borderY == target.Y) // on sort pas -> pas de calcul
                return target;

            // Bresenham, stop when reaching borders
            int w = target.X - origin.X;
            int h = target.Y - origin.Y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0)
                dx1 = -1;
            else if (w > 0)
                dx1 = 1;
            if (h < 0)
                dy1 = -1;
            else if (h > 0)
                dy1 = 1;
            if (w < 0)
                dx2 = -1;
            else if (w > 0)
                dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (longest <= shortest)
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0)
                    dy2 = -1;
                else if (h > 0)
                    dy2 = 1;
                dx2 = 0;
            }
            int x = origin.X;
            int y = origin.Y;
            int previousX = x;
            int previousY = y;
            int numerator = longest / 2;
            for (int i = 0; i <= longest; i++)
            {
                if (x < 0 || x >= SizeX || y < 0 || y >= SizeY)
                    break;
                previousX = x;
                previousY = y;
                numerator += shortest;
                if (numerator >= longest)
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
            return new FieldCoordinate(previousX, previousY);
        }

        const int CodeBottom = 1; 
        const int CodeTop    = 2;
        const int CodeLeft   = 4; 
        const int CodeRight  = 8;

        public byte CompOutCode(int x, int y, int minX, int minY, int maxX, int maxY)
        {
            byte code = 0;
            if (y > maxY)
                code = CodeBottom;
            else if (y < minX) 
                code = CodeTop;
            if (x > maxX) 
                code += CodeRight;
            else if (x < minX) 
                code += CodeLeft;
            return code;
        }

        public FieldCoordinate GetLastInboundOnPath_CohenSutherland(FieldCoordinate origin, FieldCoordinate target)
        {
            int minX = 0;
            int minY = 0;
            int maxX = SizeX-1;
            int maxY = SizeY-1;

            byte outCodeOrigin = CompOutCode(origin.X, origin.Y, minX, minY, maxX, maxY);
            byte outCodeTarget = CompOutCode(target.X, target.Y, minX, minY, maxX, maxY);

            int x1 = origin.X;
            int y1 = origin.Y;
            int x2 = target.X;
            int y2 = target.Y;
            
            int x = 0, y = 0;
            while (outCodeOrigin != 0 || outCodeTarget != 0) // While not Trivially Accepted
            {
                if ((outCodeOrigin & outCodeTarget) != 0) // Trivial Reject
                    break;
                // Failed both tests, so calculate the line segment to clip
                byte outCodeOut;
                if (outCodeOrigin > 0)
                    outCodeOut = outCodeOrigin; // Clip origin
                else
                    outCodeOut = outCodeTarget; // Clip target

                if ((outCodeOut & CodeBottom) == CodeBottom) // Clip the line to the bottom of viewport
                {
                    y = SizeY - 1;
                    x = x1 + ((x2 - x1)*(y - y1))/(y2 - y1);
                }
                else if ((outCodeOut & CodeTop) == CodeTop) // Clip the line to the top of viewport
                {
                    y = 0;
                    x = x1 + ((x2 - x1)*(y - y1))/(y2 - y1);
                }
                else if ((outCodeOut & CodeRight) == CodeRight) // Clip the line to the right of viewport
                {
                    x = SizeX - 1;
                    y = y1 + ((y2 - y1)*(x - x1))/(x2 - x1);
                }
                else if ((outCodeOut & CodeLeft) == CodeLeft) // Clip the line to the left of viewport
                {
                    x = 0;
                    y = y1 + ((y2 - y1)*(x - x1))/(x2 - x1);
                }

                if (outCodeOut == outCodeOrigin) // Modify origin coordinate
                {
                    x1 = x;
                    y1 = y;
                    outCodeOrigin = CompOutCode(x1, y1, minX, minY, maxX, maxY); // Recalculate outCode
                }
                else // Modify target coordinate
                {
                    x2 = x;
                    y2 = y;
                    outCodeTarget = CompOutCode(x2, y2, minX, minY, maxX, maxY); // Recalculate outCode
                }
            }

            return new FieldCoordinate(x2, y2);
        }

        public FieldCoordinate GetLastInboundOnPath_Test(FieldCoordinate origin, FieldCoordinate target)
        {
            if (origin.X == target.X) // Vertical
                return new FieldCoordinate(origin.X, Clamp(target.Y, 0, SizeY - 1));

            if (origin.Y == target.Y) // Horizontal
                return new FieldCoordinate(Clamp(target.X, 0, SizeX - 1), origin.Y);

            int borderX = Clamp(target.X, 0, SizeX - 1);
            int borderY = Clamp(target.Y, 0, SizeY - 1);

            if (borderX == target.X && borderY == target.Y) // on sort pas -> pas de calcul
                return target;

            double slopeYX = (target.Y - origin.Y) / (double)(target.X - origin.X);
            double slopeXY = (target.X - origin.X) / (double)(target.Y - origin.Y);
            double horizontal = slopeXY * (borderY - 0.5 + origin.Y);
            double vertical = slopeYX * (borderX - 0.5 + origin.X);

            return default(FieldCoordinate);
        }
    }
}