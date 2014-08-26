using BloodBowlPOC.Actions;
using BloodBowlPOC.Boards;
using BloodBowlPOC.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBowlTest.ActionTests
{
    [TestClass]
    public class ThrowInActionTest
    {
        public Board TestBoard { get; set; }

        [TestMethod]
        public void TestPerformInBound()
        {
            TestBoard = new Board(10, 10);

            var initalSquare = new FieldCoordinate(3, 3);

            var bounce = new BounceAction {
                BounceLeft = 1,
                Coordinate = initalSquare,
                Probability = 1
            };
            List<ActionBase> subActions = bounce.Perform(TestBoard);

            Assert.AreEqual(8, subActions.Count);
            Assert.IsTrue(subActions.All(x => x.GetType() == typeof(BounceAction)));
            Assert.IsTrue(subActions.All(x => (x as BounceAction).Probability == 1 / 8.0));
            Assert.IsTrue(subActions.All(x => (x as BounceAction).BounceLeft == 0));
            Assert.IsTrue(subActions.All(x =>
                FieldCoordinate.AreEqual(initalSquare,(x as BounceAction).LastKnownInBound )
                ));
        }

        [TestMethod]
        public void TestPerformOOB()
        {
            TestBoard = new Board(5,5);
            
            var bounce = new BounceAction {
                BounceLeft = 1,
                Coordinate = new FieldCoordinate(-1, 2),
                LastKnownInBound=new FieldCoordinate(0,2),
                Probability = 1
            };
            List<ActionBase> subActions = bounce.Perform(TestBoard);

            Assert.AreEqual(1, subActions.Count);
            Assert.AreEqual(1, subActions.OfType<ThrowInAction>().ToList().Count);

            var oobAction = subActions[0] as ThrowInAction;
            var expectedCoord = new FieldCoordinate(-1,2);
            var expectedLastIB = new FieldCoordinate(0,2);

            Assert.IsTrue(FieldCoordinate.AreEqual(expectedCoord, oobAction.Coordinate));
            Assert.IsTrue(FieldCoordinate.AreEqual(expectedLastIB, oobAction.LastInboundSquare));

        }
    }
}
