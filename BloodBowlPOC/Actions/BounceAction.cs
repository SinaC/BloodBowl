using System.Collections.Generic;
using BloodBowlPOC.Boards;
using BloodBowlPOC.Utils;

namespace BloodBowlPOC.Actions
{
    // Current standing problem with the bounce class Bounce and scatter both use this 
    // class but utilmately they behave differently.
    // Bounces offer catch opportunities and are thrown-in on the first OOB square
    // while Scatter must finish all 3 repetitions before deciding if a throw-in happen
    // and does not offer catch opportunities until after all scatters are done.
    public class BounceAction : ActionBase
    {
        public FieldCoordinate Coordinate { get; set; }
        public FieldCoordinate LastKnownInBound { get; set; }
        public int MaxDistance { get; set; }
        public int BounceLeft { get; set; }
        public double Probability { get; set; }

        List<ActionBase> subActions = new List<ActionBase>();

        public BounceAction()
        {
            Probability = 1.0; // starts with 100%
            MaxDistance = 1;
        }

        public override List<ActionBase> Perform(Board board)
        {
            //System.Diagnostics.Debug.WriteLine("Dequeue Action:{0},{1} -> {2},{3} | {4} | {5}", FromX, FromY, toX, toY, BounceLeft, Probability);

            if (Coordinate.X < 0 || Coordinate.X >= board.SizeX || Coordinate.Y < 0 || Coordinate.Y >= board.SizeY)
            {
                OutOfBoundBehaviour(board);
            }
            else
            {
                ContinueBounce(board);
            }
            return subActions;
        }

        protected virtual void OutOfBoundBehaviour(Board board)
        {
            //List<ActionBase> subActions = new List<ActionBase>();
            // Out of board
            //System.Diagnostics.Debug.WriteLine("Out of board");
            subActions.Add(new ThrowInAction {
                Coordinate = Coordinate,
                LastInboundSquare = LastKnownInBound,
                Probability = Probability
            });
        }

        protected void ContinueBounce(Board board)
        {
            // Create a new action for each direction/distance if occurrence > 0
            if (BounceLeft > 0) {
                for (int distance = 1; distance <= MaxDistance; distance++)
                    for (int direction = 0; direction < 8; direction++) {
                        BounceAction subAction = childBounceConstructor(board, direction);
                        subActions.Add(subAction);
                    }
            }
            else
                // Bounce done
                board.Probabilities[Coordinate.X, Coordinate.Y] += Probability;
        }

        protected virtual BounceAction childBounceConstructor(Board board, int direction)
        {
            return new BounceAction {
                Coordinate = new FieldCoordinate(Coordinate.X + Board.DirectionsX[direction],
                    Coordinate.Y + Board.DirectionsY[direction]
                    ),
                MaxDistance = MaxDistance,
                LastKnownInBound = Coordinate,
                BounceLeft = BounceLeft - 1,
                Probability = Probability / (8.0 * MaxDistance)
            };
        }
    }

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

        protected override BounceAction childBounceConstructor(Board board, int direction)
        {
            return new ScatterAction{
                Coordinate = new FieldCoordinate(Coordinate.X + Board.DirectionsX[direction],
                    Coordinate.Y + Board.DirectionsY[direction]
                    ),
                MaxDistance = MaxDistance,
                LastKnownInBound = board.IsInbound(Coordinate) ? Coordinate : LastKnownInBound,
                BounceLeft = BounceLeft - 1,
                Probability = Probability / (8.0 * MaxDistance)
            };
        }

    }
}
