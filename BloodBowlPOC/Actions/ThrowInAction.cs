using BloodBowlPOC.Utils;
using System.Collections.Generic;
using BloodBowlPOC.Boards;

namespace BloodBowlPOC.Actions
{
    //Represents a Throwin from out of bounds
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

                            //  if throwin is out of bound
                            //  compute last inbound square
                            //  throw in again

                            BounceAction subAction = new BounceAction
                            {
                                Coordinate = target,
                                Occurrence = 1,
                                Probability = Probability * DistanceProbabilities[distance] / 3
                            };
                            subActions.Add(subAction);
                        }
            }

            return subActions;
        }

        protected FieldCoordinate GetLastInboundOnPath(FieldCoordinate origin, FieldCoordinate target, Board theBoard)
        {
            if (theBoard.IsInbound(target)){
                return target;
            }
            //Obviously ca fait pas encore ce que ca doit.
            //Faut tracer la ligne genre Bresenham et choisir le dernier square inbound
            return target;
        }
    }
}
