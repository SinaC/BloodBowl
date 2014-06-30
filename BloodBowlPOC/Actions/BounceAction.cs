using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloodBowlPOC.Boards;

namespace BloodBowlPOC.Actions
{
    public class BounceAction : ActionBase
    {
        public int FromX { get; set; }
        public int FromY { get; set; }
        public int DirectionX { get; set; }
        public int DirectionY { get; set; }
        public int Distance { get; set; }
        public int MaxDistance { get; set; }
        public int Occurrence { get; set; }
        public double Probability { get; set; }

        public override List<ActionBase> Perform(Board board)
        {
            List<ActionBase> actions = new List<ActionBase>();
            //
            int toX = FromX + (DirectionX * Distance);
            int toY = FromY + (DirectionY * Distance);

            //
            //System.Diagnostics.Debug.WriteLine("Dequeue Action:{0},{1} -> {2},{3} | {4} | {5}", FromX, FromY, toX, toY, Occurrence, Probability);

            if (toX < 0 || toX >= board.SizeX || toY < 0 || toY >= board.SizeY)
            {
                // Out of board
                //System.Diagnostics.Debug.WriteLine("Out of board");
            }
            else
            {
                // Create a new action for each direction/distance if occurrence > 0
                if (Occurrence > 0)
                {
                    for (int distance = 1; distance <= MaxDistance; distance++)
                        for (int direction = 0; direction < 8; direction++)
                        {
                            BounceAction subAction = new BounceAction
                            {
                                FromX = toX,
                                FromY = toY,
                                DirectionX = Board.DirectionsX[direction],
                                DirectionY = Board.DirectionsY[direction],
                                Distance = distance,
                                MaxDistance = MaxDistance,
                                Occurrence = Occurrence - 1,
                                Probability = Probability / (8.0 * MaxDistance)
                            };
                            actions.Add(subAction);
                        }
                }
                else
                    // Bounce done
                    board.Probabilities[toX, toY] += Probability;
            }
            return actions;
        }
    }
}
