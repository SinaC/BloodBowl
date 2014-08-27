using System.Collections.Generic;
using BloodBowlPOC.Utils;
using BloodBowlPOC.Boards;

namespace BloodBowlPOC.Actions
{
    internal class KickOffAction : ActionBase
    {
        public FieldCoordinate Target { get; set; }

        public override List<ActionBase> Perform(Board board)
        {
            List<ActionBase> subActions = new List<ActionBase>(2);

            if (board.IsInbound(Target))
            {
                for (int direction = 0; direction < 8; direction++)
                    for (int distance = 1; distance < 7; distance++)
                    {
                        FieldCoordinate diceresult = new FieldCoordinate(Target.X + distance * Board.DirectionsX[direction],
                                    Target.Y + distance * Board.DirectionsY[direction]
                                    );

                        if (board.IsInbound(diceresult)) {
                            ActionBase subAction = new KickOffBounceAction {
                                Coordinate = diceresult,
                                LastKnownInBound = Target,
                                BounceLeft = 1,
                                Probability = 1 / 8.0 / 6.0
                            };
                            subActions.Add(subAction);
                        }
                        // else we don't do anything, that's actually a Touchback
                    }
            }

            return subActions;
        }
    }
}
