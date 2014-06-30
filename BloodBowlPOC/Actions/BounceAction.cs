using System.Collections.Generic;
using BloodBowlPOC.Boards;

namespace BloodBowlPOC.Actions
{
    public class BounceAction : ActionBase
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int MaxDistance { get; set; }
        public int BounceLeft { get; set; }
        public double Probability { get; private set; }

        public BounceAction()
        {
            Probability = 1.0; // starts with 100%
        }

        public override List<ActionBase> Perform(Board board)
        {
            List<ActionBase> actions = new List<ActionBase>();
            //
            //System.Diagnostics.Debug.WriteLine("Dequeue Action:{0},{1} | {4} | {5}", X, Y, BounceLeft, Probability);

            if (X < 0 || X >= board.SizeX || Y < 0 || Y >= board.SizeY)
            {
                // Out of board
                //System.Diagnostics.Debug.WriteLine("Out of board");
            }
            else
            {
                // Create a new action for each direction/distance if occurrence > 0
                if (BounceLeft > 0)
                {
                    for (int distance = 1; distance <= MaxDistance; distance++)
                        for (int direction = 0; direction < Board.DirectionsCount; direction++)
                        {
                            int toX = X + Board.DirectionsX[direction]*distance;
                            int toY = Y + Board.DirectionsY[direction]*distance;
                            BounceAction subAction = new BounceAction
                                {
                                    X = toX,
                                    Y = toY,
                                    MaxDistance = MaxDistance,
                                    BounceLeft = BounceLeft - 1,
                                    Probability = Probability/(8.0*MaxDistance)
                                };
                            actions.Add(subAction);
                        }
                }
                else
                    // Bounce done
                    board.Probabilities[X, Y] += Probability;
            }
            return actions;
        }
    }
}
