using BloodBowlPOC.Utils;
using System.Collections.Generic;
using BloodBowlPOC.Boards;

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
                { 0, 0, 1/36, 2/36, 3/36, 4/36, 5/36, 6/36, 5/36, 4/36, 3/36, 2/36, 1/36 };
              //  0  1   2     3     4     5     6     7     8     9     10    11    12

        public override List<ActionBase> Perform(Board board)
        {
            List<ActionBase> subActions = new List<ActionBase>(1);

            if (board.IsInbound(Coordinate)) 
            {
                // Inbounds while we should be out of bounds for this action
                //System.Diagnostics.Debug.WriteLine("Throw-In from Inbounds");
            }
            else
            {
                //ok now how to get direction
                var ruler = board.GetThrowinRuler(Coordinate);

                for (int direction = 0; direction < 3; direction++)
                    for (int distance = 2; distance < 13; distance++)
                        {
                            var target = Coordinate = new FieldCoordinate(LastInboundSquare.X + distance * ruler[direction].X,
                                                                     LastInboundSquare.Y + distance * ruler[direction].Y
                                                                );

                            if (!board.IsInbound(target)) {
                                //  if throwin is out of bound, compute new last inbound square
                                //  throw in again
                                LastInboundSquare = board.GetLastInboundOnPath(LastInboundSquare, target);

                                ThrowInAction subAction = new ThrowInAction {
                                    Coordinate = target,
                                    LastInboundSquare = LastInboundSquare,
                                    Probability = Probability * DistanceProbabilities[distance] / 3
                                };

                                //The sad thing here is that all the sub throwins from the same square are the same
                                //So if we can identify them we could do them only once with a sum of probas
                                subActions.Add(subAction);
                            }
                            else {
                                BounceAction subAction = new BounceAction {
                                    Coordinate = target,
                                    BounceLeft = 1,
                                    Probability = Probability * DistanceProbabilities[distance] / 3
                                };
                                subActions.Add(subAction);
                            }
                        }
            }

            return subActions;
        }
    }
}
