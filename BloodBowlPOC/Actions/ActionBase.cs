using System.Collections.Generic;
using BloodBowlPOC.Boards;

namespace BloodBowlPOC.Actions
{
    public abstract class ActionBase
    {
        public abstract List<ActionBase> Perform(Board board);
    }
}
