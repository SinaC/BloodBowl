﻿namespace BloodBowlPOC.Boards
{
    interface IBoard
    {
        void ComputeBounceProbabilities(Utils.FieldCoordinate point, int maxBounces, string selectedAction);
        Utils.FieldCoordinate GetLastInboundOnPath(Utils.FieldCoordinate theOrigin, Utils.FieldCoordinate theTarget);
        Utils.FieldCoordinate[] GetThrowinRuler(Utils.FieldCoordinate coordinate);
        bool IsInbound(Utils.FieldCoordinate theSquare);
        void Reset();
    }
}
