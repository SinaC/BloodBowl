using System.Collections.Generic;
using BloodBowlPOC.Boards;
using BloodBowlPOC.Utils;
using BloodBowlPOC.Actions;

namespace BloodBowlPOC
{
    // Scatter must finish all 3 repetitions before deciding if a throw-in happen
    // and does not offer catch opportunities until after all scatters are done.
    public class ScatterAction : BounceAction
    {
        protected override void OutOfBoundBehaviour(Board board)
        {
            if (BounceLeft > 0) {
                ContinueBounce(board);
            }
            else {
                base.OutOfBoundBehaviour(board);
            }
        }

        protected override BounceAction ChildBounceConstructor(Board board, int direction)
        {
            return new ScatterAction{
                Coordinate = new FieldCoordinate(Coordinate.X + Board.DirectionsX[direction],
                    Coordinate.Y + Board.DirectionsY[direction]
                    ),
                LastKnownInBound = board.IsInbound(Coordinate) ? Coordinate : LastKnownInBound,
                BounceLeft = BounceLeft - 1,
                Probability = Probability / 8.0
            };
        }
    }
}
