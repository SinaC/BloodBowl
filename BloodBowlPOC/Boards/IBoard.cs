using System;
namespace BloodBowlPOC.Boards
{
    interface IBoard
    {
        void ComputeBounceProbabilities(BloodBowlPOC.Utils.FieldCoordinate point, int maxBounces, string selectedAction);
        BloodBowlPOC.Utils.FieldCoordinate GetLastInboundOnPath(BloodBowlPOC.Utils.FieldCoordinate theOrigin, BloodBowlPOC.Utils.FieldCoordinate theTarget);
        BloodBowlPOC.Utils.FieldCoordinate[] GetThrowinRuler(BloodBowlPOC.Utils.FieldCoordinate coordinate);
        bool IsInbound(BloodBowlPOC.Utils.FieldCoordinate theSquare);
        void Reset();
    }
}
