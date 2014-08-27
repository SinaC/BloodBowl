using System.Collections.Generic;
using System.Linq;
using BloodBowlPOC.Actions;
using BloodBowlPOC.Boards;
using BloodBowlPOC.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BloodBowlTest.ActionTests
{
    [TestClass]
    public class KickOffActionTest
    {
        public Board TestBoard { get; set; }

        [TestMethod]
        public void KickOffDoNothingTest()
        {
            TestBoard = new Board(10, 10);

            var initalSquare = new FieldCoordinate(-3, -3);

            var bounce = new KickOffAction {
                Target = initalSquare
            };

            List<ActionBase> subActions = bounce.Perform(TestBoard);

            Assert.AreEqual(0, subActions.Count,"We kicked OOB, we shouldn't have anything here");
        }

        [TestMethod]
        public void KickOffCenterTest()
        {
            TestBoard = new Board(13, 13); //Juste assez grand pour que tout soit inbound

            var initalSquare = new FieldCoordinate(6,6);

            var bounce = new KickOffAction{
                Target = initalSquare
            };

            List<ActionBase> subActions = bounce.Perform(TestBoard);

            Assert.AreEqual(48, subActions.Count, "Unexpected subActions count generated");
            Assert.AreEqual(48, subActions.OfType<BounceAction>().Count(), "subActions Type not of the right Type");

            var bounceActions = subActions.Cast<BounceAction>().ToList();

            Assert.IsTrue(bounceActions.All(x => TestBoard.IsInbound(x.Coordinate)),"Incorrect bounce coordinate");
            Assert.IsTrue(Math.Abs(bounceActions.Sum(x => x.Probability)-1) < 0.000000000000001, "Lost some probas");
            Assert.AreEqual(1, bounceActions.Select(x => x.Probability).Distinct().Count(),"All result should have same proba");
        }

        [TestMethod]
        public void KickOffCornerTest()
        {
            TestBoard = new Board(13, 13); //Assez grand pour que tout soit inbound

            var initalSquare = new FieldCoordinate(0,0);

            var bounce = new KickOffAction {
                Target = initalSquare
            };

            List<ActionBase> subActions = bounce.Perform(TestBoard);

            Assert.AreEqual(18, subActions.Count, "Unexpected subActions count generated");
            Assert.AreEqual(18, subActions.OfType<BounceAction>().Count(), "subActions Type not of the right Type");

            var bounceActions = subActions.Cast<BounceAction>().ToList();

            Assert.IsTrue(bounceActions.All(x => TestBoard.IsInbound(x.Coordinate)), "Incorrect bounce coordinate");
        }
    }
}
