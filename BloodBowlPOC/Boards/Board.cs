using System.Collections.Generic;
using BloodBowlPOC.Actions;
using BloodBowlPOC.Utils;
using System;
namespace BloodBowlPOC.Boards
{
    public class Board : BloodBowlPOC.Boards.IBoard
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

        public void ComputeBounceProbabilities(FieldCoordinate point, int maxBounces, string selectedAction)
        {
            ActionBase startAction = null;
            // Throw the ball
            switch (selectedAction) {
                case "Pass":
                    startAction = new BounceAction {
                        Coordinate = point,
                        LastKnownInBound = point,
                        BounceLeft = maxBounces,
                    };
                    break;
                case "KickOff":
                    startAction = new KickOffAction {
                        Target = point
                        //                LastKnownInBound = point,
                        //              BounceLeft = maxBounces,
                    };
                    break;
                default:
                    throw new ArgumentException("Mode Slected Didn't make sense", "selectedAction"); 
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
            return theSquare.X >= 0 
                && theSquare.Y >= 0 
                && theSquare.X < SizeX 
                && theSquare.Y < SizeY;
        }

        public FieldCoordinate GetLastInboundOnPath(FieldCoordinate theOrigin, FieldCoordinate theTarget)
        {
            if (theOrigin.X == theTarget.X) { // Vertical
                return new FieldCoordinate(theOrigin.X, Math.Max(0,Math.Min(theTarget.Y, SizeY-1)));
            }

            if (theOrigin.Y == theTarget.Y) { // Horizontal
                return new FieldCoordinate(Math.Max(0, Math.Min(theTarget.X, SizeX-1)), theOrigin.Y);
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
                double slope = (double)(theTarget.Y - theOrigin.Y) / (theTarget.X - theOrigin.X);
                inBoundX = borderX;
                inBoundY = (int)(theOrigin.Y + slope * (borderX - theOrigin.X));
            }
            else {
                double slope = (double)(theTarget.X - theOrigin.X) / (theTarget.Y - theOrigin.Y);
                inBoundX = (int)(theOrigin.X + slope * (borderY - theOrigin.Y));
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
    }
}
