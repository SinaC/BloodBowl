using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BloodBowlPOC.Boards;
using BloodBowlPOC.Utils;

namespace BloodBowlTest
{
    [TestClass]
    public class BoardTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DeploymentItem("BloodBowlTest\\LastIBSquareTest.XML")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                   "LastIBSquareTest.xml",
                   "Row",
                    DataAccessMethod.Sequential)]
        public void DataGetLastInoundOnPathTest()
        {
            int width = Int32.Parse((string) TestContext.DataRow["Width"]);
            int height = Int32.Parse((string)TestContext.DataRow["Height"]);

            FieldCoordinate from = new FieldCoordinate(
                    Int32.Parse((string)TestContext.DataRow["FromX"]),
                    Int32.Parse((string)TestContext.DataRow["FromY"])
                );

            FieldCoordinate to = new FieldCoordinate(
                    Int32.Parse((string)TestContext.DataRow["ToX"]),
                    Int32.Parse((string)TestContext.DataRow["ToY"])
                );

            FieldCoordinate expected = new FieldCoordinate(
                Int32.Parse((string)TestContext.DataRow["ExpectedX"]),
                Int32.Parse((string)TestContext.DataRow["ExpectedY"])
                );

            var result = DoCall(width, height, from, to);

            Assert.IsTrue(FieldCoordinate.AreEqual(expected, result), 
                String.Format("From {0} To {1}. Expected {2}, got {3}", from, to, expected, result));

        }

        private FieldCoordinate DoCall(int width, int height, FieldCoordinate from, FieldCoordinate to)
        {
            Board theBoard = new Board(width, height);

            return theBoard.GetLastInboundOnPath_CohenSutherland(from, to);
        }

        
        // TO REMOVE: included in automated tests above
        //[TestMethod]
        //public void SingleGetLastInoundOnPathTest()
        //{
        //    FieldCoordinate from = new FieldCoordinate(3,2);
        //    FieldCoordinate to = new FieldCoordinate(2,-1);
        //    FieldCoordinate expected = new FieldCoordinate(2,0);
        //    int width = 4;
        //    int height = 4;

        //    var result = DoCall(width, height, from, to);

        //    Assert.IsTrue(FieldCoordinate.AreEqual(expected, result), 
        //        String.Format("Expected {0}, got {1}", expected, result));
        //}

    //    [TestMethod]
    //    public void GetLastInboundOnPathTest_Inbounds()
    //    {
    //        Board theBoard = new Board(4, 4);

    //        FieldCoordinate from = new FieldCoordinate(3, 3);
    //        FieldCoordinate to = new FieldCoordinate(2, 2);
    //        FieldCoordinate expectedResult = new FieldCoordinate(2, 2);

    //        FieldCoordinate result = theBoard.GetLastInboundOnPath(from, to);

    //        Assert.IsTrue(FieldCoordinate.AreEqual(expectedResult, result), 
    //            String.Format("Expected {0}, got {1}", expectedResult, result));
    //    }

    //    [TestMethod]
    //    public void GetLastInboundOnPathTest_OutOfBounds()
    //    {
    //        Board theBoard = new Board(4, 4);

    //        FieldCoordinate from = new FieldCoordinate(3, 3);
    //        FieldCoordinate to = new FieldCoordinate(-1, -1);
    //        FieldCoordinate expectedResult = new FieldCoordinate(0, 0);

    //        FieldCoordinate result = theBoard.GetLastInboundOnPath(from, to);

    //        Assert.IsTrue(FieldCoordinate.AreEqual(expectedResult, result), 
    //            String.Format("Expected {0}, got {1}", expectedResult, result));
    //    }

    //    [TestMethod]
    //    public void GetLastInboundOnPathTest_OutOfBounds2()
    //    {
    //        Board theBoard = new Board(6, 6);

    //        FieldCoordinate from = new FieldCoordinate(2, 3);
    //        FieldCoordinate to = new FieldCoordinate(6, 0);
    //        FieldCoordinate expectedResult = new FieldCoordinate(5, 0);

    //        FieldCoordinate result = theBoard.GetLastInboundOnPath(from, to);

    //        Assert.IsTrue(FieldCoordinate.AreEqual(expectedResult, result), 
    //            String.Format("Expected {0}, got {1}", expectedResult, result));
    //    }

    //    [TestMethod]
    //    public void GetLastInboundOnPathTest_OutOfBounds3()
    //    {
    //        Board theBoard = new Board(4, 4);

    //        FieldCoordinate from = new FieldCoordinate(3, 1);
    //        FieldCoordinate to = new FieldCoordinate(5, -1);
    //        FieldCoordinate expectedResult = new FieldCoordinate(3, 1);

    //        FieldCoordinate result = theBoard.GetLastInboundOnPath(from, to);

    //        Assert.IsTrue(FieldCoordinate.AreEqual(expectedResult, result), 
    //            String.Format("Expected {0}, got {1}", expectedResult, result));
    //    }

    //    [TestMethod]
    //    public void GetLastInboundOnPathTest_OutOfBounds4()
    //    {
    //        Board theBoard = new Board(4, 4);

    //        FieldCoordinate from = new FieldCoordinate(3, 1);
    //        FieldCoordinate to = new FieldCoordinate(4, -4);
    //        FieldCoordinate expectedResult = new FieldCoordinate(5, 1);

    //        FieldCoordinate result = theBoard.GetLastInboundOnPath(from, to);

    //        Assert.IsTrue(FieldCoordinate.AreEqual(expectedResult, result), 
    //            String.Format("Expected {0}, got {1}", expectedResult, result));
    //    }

    //    [TestMethod]
    //    public void GetLastInboundOnPathTest_OutOfBounds5()
    //    {
    //        Board theBoard = new Board(6, 6);

    //        FieldCoordinate from = new FieldCoordinate(0, 0);
    //        FieldCoordinate to = new FieldCoordinate(10, -2);
    //        FieldCoordinate expectedResult = new FieldCoordinate(2, 0);

    //        FieldCoordinate result = theBoard.GetLastInboundOnPath(from, to);

    //        Assert.IsTrue(FieldCoordinate.AreEqual(expectedResult, result), 
    //            String.Format("Expected {0}, got {1}", expectedResult, result));
    //    }
    }
}
