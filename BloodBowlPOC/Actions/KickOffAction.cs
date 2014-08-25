using BloodBowlPOC.Utils;
using System.Collections.Generic;
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
                        ActionBase subAction;
                        FieldCoordinate diceresult = new FieldCoordinate(Target.X + distance * Board.DirectionsX[direction],
                                    Target.Y + distance * Board.DirectionsY[direction]
                                    );

                        if (board.IsInbound(diceresult)) {
                            subAction = new BounceAction {
                                Coordinate = diceresult,
                                LastKnownInBound = Target,
                                BounceLeft = 1,
                                Probability = 1 / 8.0 / 6.0
                            };
                        }
                        else {
                            subAction = new ThrowInAction {
                                Coordinate = diceresult,
                                LastInboundSquare = Target,
                                Probability = 1 / 8.0 / 6.0
                            };
                        }
                        subActions.Add(subAction);
                    }
            }

            return subActions;
        }
    }
}
