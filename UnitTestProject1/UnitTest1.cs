using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BloodBowlPOC.Boards;
using BloodBowlPOC.Utils;

namespace UnitTestProject1
{
    [TestClass]
    public class BoardTest
    {
        Board theBoard;

        [TestMethod]
        
        public void GetLastInoundOnPathTest()
        {
            theBoard = new Board(4,4);

            FieldCoordinate from = new FieldCoordinate(3,3);
            FieldCoordinate to = new FieldCoordinate(-1,-1);
            FieldCoordinate ExpectedResult = new FieldCoordinate(0,0);


            FieldCoordinate result = theBoard.GetLastInboundOnPath(from, to);

            Assert.IsTrue(FieldCoordinate.AreEqual(ExpectedResult, result)
                , String.Format("Expected {0}, got {1}",ExpectedResult,result));
        }
    }
}
