using BloodBowlPOC.Utils;
using System.Collections.Generic;

namespace BloodBowlPOC.Actions
{
    class PassAction : ActionBase
    {
        public FieldCoordinate Target { get; set; }
        public FieldCoordinate Origin { get; set; }

        public override List<ActionBase> Perform(Boards.Board board)
        {
            List<ActionBase> subActions = new List<ActionBase>(1);

            double accurateProba = GetAccuracy();

            subActions.Add(new BounceAction { 
                Coordinate = Target, 
                LastKnownInBound = board.GetLastInboundOnPath(Origin,Target),
                Occurrence = 3, 
                Probability = 1-accurateProba 
            });


            board.Probabilities[Target.X, Target.Y] += accurateProba;

            return subActions;
        }

        protected double GetAccuracy()
        {
            /*
            * Always inaccurate for now. In-game player stats are a factor but we can take a baseline
            * Distance is also a factr and we should compute that and maybe display it on screen.
            * Maybe Even make squares selection clip to center.
            * TODO: Get the distance ruler
            */
            return 0; 
        }
    }
}
