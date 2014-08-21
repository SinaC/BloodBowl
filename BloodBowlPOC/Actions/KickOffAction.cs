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
                    for (int distance = 1; distance < 6; distance++)
                    {
                        BounceAction subAction = new BounceAction
                        {
                            Coordinate = new FieldCoordinate(Target.X + distance*Board.DirectionsX[direction],
                                Target.Y + distance*Board.DirectionsY[direction]
                                ),
                            Occurrence = 1,
                            Probability = 1
                        };
                        subActions.Add(subAction);
                    }
            }

            return subActions;
        }
    }
}
