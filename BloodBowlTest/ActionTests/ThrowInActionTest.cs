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
        public void TestThrowInDoNothing()
        {
            TestBoard = new Board(10, 10);

            var initalSquare = new FieldCoordinate(3, 3);

            var bounce = new ThrowInAction{
                Coordinate = initalSquare,
                LastInboundSquare = initalSquare,
                Probability = 1
            };

            List<ActionBase> subActions = bounce.Perform(TestBoard);

            Assert.AreEqual(0, subActions.Count);
        }

        [TestMethod]
        public void TestThrowinNormal()
        {
            TestBoard = new Board(15, 31); //Assez grand pour que tout soit inbound

            var initalSquare = new FieldCoordinate(-1, 15);
            var lastKnownInbound = new FieldCoordinate(0, 15);

            var bounce = new ThrowInAction {
                Coordinate = initalSquare,
                LastInboundSquare = lastKnownInbound,
                Probability = 1
            };

            List<ActionBase> subActions = bounce.Perform(TestBoard);

            Assert.AreEqual(33, subActions.Count);
            Assert.AreEqual(33, subActions.OfType<BounceAction>().ToList().Count);

            var bounceActions = subActions.Cast<BounceAction>();

            Assert.IsTrue(bounceActions.GroupBy(x => x.Coordinate.X).All(x => x.Count(y => true) == 3));
            Assert.IsTrue(bounceActions.GroupBy(x => x.Coordinate.X).All(x => x.ToList().Count == 3));
            Assert.AreEqual(11, bounceActions.Max(x => x.Coordinate.X));
            Assert.AreEqual(11, bounceActions.Where(x => x.Coordinate.Y == lastKnownInbound.Y).ToList().Count);
            Assert.AreEqual(22, bounceActions.Select(x => x.Coordinate.Y).Where(x => x != lastKnownInbound.Y).Distinct().ToList().Count);
        }

        [TestMethod]
        public void TestThrowinWithOOB()
        {
            TestBoard = new Board(15, 31); //Assez grand pour que tout soit inbound

            var initalSquare = new FieldCoordinate(-1, 6);
            var lastKnownInbound = new FieldCoordinate(0, 6);

            var bounce = new ThrowInAction {
                Coordinate = initalSquare,
                LastInboundSquare = lastKnownInbound,
                Probability = 1
            };

            List<ActionBase> subActions = bounce.Perform(TestBoard);

            Assert.AreEqual(33, subActions.Count);
            Assert.AreEqual(28, subActions.OfType<BounceAction>().ToList().Count);
            Assert.AreEqual(5, subActions.OfType<ThrowInAction>().ToList().Count);

            var throwIns = subActions.OfType<ThrowInAction>();
            var expectedLastIB = new FieldCoordinate(6,0);

            Assert.AreEqual(5, throwIns.Count(x => !TestBoard.IsInbound(x.Coordinate)));
            Assert.IsTrue(throwIns.Where(x => !TestBoard.IsInbound(x.Coordinate))
                        .All(x=> FieldCoordinate.AreEqual(x.LastInboundSquare,expectedLastIB)));
        }

    }
}
