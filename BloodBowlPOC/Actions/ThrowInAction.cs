using BloodBowlPOC.Utils;
using System.Collections.Generic;
using BloodBowlPOC.Boards;
using System.Linq;

namespace BloodBowlPOC.Actions
{
    //Represents a Throw-in from out of bounds
    //Do we give the OOB Coordinate or the closest inbound position ?
    //If so what with direction ?
    //For now I'm gonna expect the OOB position, I think it makes a lot of things easier.
    class ThrowInAction : ActionBase
    {
        public FieldCoordinate Coordinate { get; set; }
        public FieldCoordinate LastInboundSquare { get; set; }
        public double Probability { get; set; }
                     
        //TODO: Make a Dice or Proba class to handle this kind of shit                   
        protected static double[] DistanceProbabilities = 
                { 0, 0, 1.0/36.0, 2.0/36.0, 3.0/36.0, 4.0/36.0, 5.0/36.0, 6.0/36.0, 5.0/36.0, 4.0/36.0, 3.0/36.0, 2.0/36.0, 1.0/36.0 };
              //  0  1     2         3         4         5         6         7         8         9        10         11        12

        public override List<ActionBase> Perform(Board board)
        {
            List<ActionBase> subActions = new List<ActionBase>(1);
            if (Probability < 0.00001) {
                return subActions;
            }

            if (board.IsInbound(Coordinate)) 
            {
                //Inbounds while we should be out of bounds for this action
                //System.Diagnostics.Debug.WriteLine("Throw-In from Inbounds");
            }
            else
            {
                //ok now how to get direction
                var ruler = board.GetThrowinRuler(Coordinate);

                for (int direction = 0; direction < 3; direction++) {
                    for (int distance = 2; distance < 13; distance++) {
                        var target = Coordinate = new FieldCoordinate(LastInboundSquare.X + (distance-1) * ruler[direction].X,
                                                                 LastInboundSquare.Y + (distance-1) * ruler[direction].Y
                                                            );
                        if( board.EpsilonProba > Probability * DistanceProbabilities[distance] / 3.0 ){
                            return subActions;
                        }

                        if (!board.IsInbound(target)) {
                            //  if throwin is out of bound, compute new last inbound square
                            //  throw in again
                            LastInboundSquare = StepByStepOOB(LastInboundSquare, target, ruler[direction], board);
                            //TODO: Should be replaced by correct implementation of below function
                            //LastInboundSquare = board.GetLastInboundOnPath(LastInboundSquare, target);

                            ThrowInAction subAction = new ThrowInAction {
                                Coordinate = target,
                                LastInboundSquare = LastInboundSquare,
                                Probability = Probability * DistanceProbabilities[distance] / 3.0
                            };

                            //The sad thing here is that all the sub throwins from the same lastinbound square are the same
                            //So if we can identify them we could do them only once with a sum of probas
                            subActions.Add(subAction);
                        }
                        else {
                            BounceAction subAction = new BounceAction {
                                Coordinate = target,
                                BounceLeft = 1,
                                Probability = Probability * DistanceProbabilities[distance] / 3.0
                            };
                            subActions.Add(subAction);
                        }
                    }
                }
            }

            return subActions;
        }

        /// <summary>
        /// Function that gets the last inbound on a path. Will step in direction, only fork for orthogonal or exactly diagonal.
        /// </summary>
        /// <param name="from">Last known inbound saquare</param>
        /// <param name="to">Target Square</param>
        /// <param name="theDirection">Step direction</param>
        /// <param name="board">reference to the board</param>
        /// <returns>Last inbound square on path.</returns>
        private FieldCoordinate StepByStepOOB(FieldCoordinate from, FieldCoordinate to, FieldCoordinate theDirection, Board board)
        {
            if (board.IsInbound(to)) {
                return to;
            }

            FieldCoordinate lastInbound= from;
            FieldCoordinate aCoordinate = from;
            while(board.IsInbound(aCoordinate)){
                lastInbound = aCoordinate;
                aCoordinate = new FieldCoordinate(aCoordinate.X + theDirection.X, aCoordinate.Y + theDirection.Y);
            }

            return lastInbound;
        }
    }
}
