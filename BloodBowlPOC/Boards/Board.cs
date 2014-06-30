using System.Collections.Generic;
using BloodBowlPOC.Actions;

namespace BloodBowlPOC.Boards
{
    public class Board
    {
        public static int DirectionsCount = 8;
        //                                  N, NE,  E, SE,  S, SW,  W, NW
        public static int[] DirectionsX = {0, 1, 1, 1, 0, -1, -1, -1};
        public static int[] DirectionsY = {-1, -1, 0, 1, 1, 1, 0, -1};

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

        public void ComputeBounceProbabilities(int x, int y, int maxDistance, int maxBounces)
        {
            // Throw the ball
            BounceAction startAction = new BounceAction
                {
                    X = x,
                    Y = y,
                    MaxDistance = maxDistance,
                    BounceLeft = maxBounces,
                };
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
    }
}
