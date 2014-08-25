using System.Collections.Generic;
using BloodBowlPOC.Boards;
using BloodBowlPOC.Utils;

namespace BloodBowlPOC.Actions
{
    // Bounces offer catch opportunities and are thrown-in on the first OOB square
    public class BounceAction : ActionBase
    {
        public FieldCoordinate Coordinate { get; set; }
        public FieldCoordinate LastKnownInBound { get; set; }
        public int BounceLeft { get; set; }
        public double Probability { get; set; }

        protected readonly List<ActionBase> SubActions = new List<ActionBase>();

        public BounceAction()
        {
            Probability = 1.0; // starts with 100% if nothing else is provided.
        }

        public override List<ActionBase> Perform(Board board)
        {
            //System.Diagnostics.Debug.WriteLine("Dequeue Action:{0},{1} -> {2},{3} | {4} | {5}", LastKnownInBound.X, LastKnownInBound.Y, Coordinate.X, Coordinate.Y, BounceLeft, Probability);
            if (board.EpsilonProba > Probability ) {
                return SubActions;
            }

            if (!board.IsInbound(Coordinate)) {
                OutOfBoundBehaviour(board);
            }
            else {
                ContinueBounce(board);
            }
            return SubActions;
        }

        protected virtual void OutOfBoundBehaviour(Board board)
        {
            // Out of board
            //System.Diagnostics.Debug.WriteLine("Out of board");
            SubActions.Add(new ThrowInAction {
                Coordinate = Coordinate,
                LastInboundSquare = LastKnownInBound,
                Probability = Probability
            });
        }

        protected void ContinueBounce(Board board)
        {
            // Create a new action for each direction/distance if occurrence > 0
            if (BounceLeft > 0) {
                for (int direction = 0; direction < 8; direction++) {
                    BounceAction subAction = ChildBounceConstructor(board, direction);
                    SubActions.Add(subAction);
                }
            }
            else
                // Bounce done
                board.Probabilities[Coordinate.X, Coordinate.Y] += Probability;
        }

        protected virtual BounceAction ChildBounceConstructor(Board board, int direction)
        {
            return new BounceAction {
                Coordinate = new FieldCoordinate(Coordinate.X + Board.DirectionsX[direction],
                    Coordinate.Y + Board.DirectionsY[direction]
                    ),
                LastKnownInBound = Coordinate,
                BounceLeft = BounceLeft - 1,
                Probability = Probability / 8.0
            };
        }
    }
}
