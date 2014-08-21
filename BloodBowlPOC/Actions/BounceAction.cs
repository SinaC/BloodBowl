using System.Collections.Generic;
using BloodBowlPOC.Boards;
using BloodBowlPOC.Utils;

namespace BloodBowlPOC.Actions
{
    // Current standing problem with the bounce class
    // Bounce and scatter both use this class but utilmately the behave differently
    // Bounces offer catch opportunities and are throw-in on the first OOB square
    // while Scatter must finish all 3 repetitions before deciding a throwing and does
    // not offer catch opportunities, only at the end of the 3 moves.
    public class BounceAction : ActionBase
    {
        public FieldCoordinate Coordinate { get; set; }
        public int Occurrence { get; set; }
        public double Probability { get; set; }

        public override List<ActionBase> Perform(Board board)
        {
            List<ActionBase> subActions = new List<ActionBase>();


            //System.Diagnostics.Debug.WriteLine("Dequeue Action:{0},{1} -> {2},{3} | {4} | {5}", FromX, FromY, toX, toY, Occurrence, Probability);

            if (Coordinate.X < 0 || Coordinate.X >= board.SizeX || Coordinate.Y < 0 || Coordinate.Y >= board.SizeY)
            {
                // Out of board
                //System.Diagnostics.Debug.WriteLine("Out of board");
            }
            else
            {
                // Create a new action for each direction/distance if occurrence > 0
                if (Occurrence > 0)
                {
                    for (int direction = 0; direction < 8; direction++)
                    {
                        BounceAction subAction = new BounceAction
                        {
                            Coordinate = new FieldCoordinate(Coordinate.X + Board.DirectionsX[direction],
                                Coordinate.Y + Board.DirectionsY[direction]
                                ),
                            Occurrence = Occurrence - 1,
                            Probability = Probability/(8.0)
                        };
                        subActions.Add(subAction);
                    }
                }
                else
                    // Bounce done
                    board.Probabilities[Coordinate.X, Coordinate.Y] += Probability;
            }
            return subActions;
        }
    }
}
