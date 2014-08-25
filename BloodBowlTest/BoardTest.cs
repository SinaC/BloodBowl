using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BloodBowlPOC.Boards;
using BloodBowlPOC.Utils;
using System.Data;

namespace BloodBowlTest
{
    [TestClass]
    public class BoardTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        [DeploymentItem("BloodBowlTest\\LastIBSquareTest.XML")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                   "LastIBSquareTest.xml",
                   "Row",
                    DataAccessMethod.Sequential)]
        public void DataGetLastInoundOnPathTest()
        {
            FieldCoordinate From = new FieldCoordinate(
                    Int32.Parse((string)TestContext.DataRow["FromX"]),
                    Int32.Parse((string)TestContext.DataRow["FromY"])
                );

            FieldCoordinate To = new FieldCoordinate(
                    Int32.Parse((string)TestContext.DataRow["ToX"]),
                    Int32.Parse((string)TestContext.DataRow["ToY"])
                );

            FieldCoordinate Expected = new FieldCoordinate(
                Int32.Parse((string)TestContext.DataRow["ExpectedX"]),
                Int32.Parse((string)TestContext.DataRow["ExpectedY"])
                );

            var result = DoCall(From, To);


            Assert.IsTrue(FieldCoordinate.AreEqual(Expected, result)
                , String.Format("Expected {0}, got {1}", Expected, result));

        }

        [TestMethod]
        public void SingleGetLastInoundOnPathTest()
        {
            FieldCoordinate From = new FieldCoordinate(3,2);

            FieldCoordinate To = new FieldCoordinate(2,-1);

            FieldCoordinate Expected = new FieldCoordinate(2,0);

            var result = DoCall(From, To);


            Assert.IsTrue(FieldCoordinate.AreEqual(Expected, result)
                , String.Format("Expected {0}, got {1}", Expected, result));
        }

        private FieldCoordinate DoCall(FieldCoordinate from, FieldCoordinate to)
        {
            Board theBoard = new Board(4,4);

            //FieldCoordinate from = new FieldCoordinate(3,3);
            //FieldCoordinate to = new FieldCoordinate(-1,-1);
            //FieldCoordinate ExpectedResult = new FieldCoordinate(0,0);


            return theBoard.GetLastInboundOnPath(from, to);

        }

        [TestMethod]
        public void GetLastInboundOnPathTest_Inbounds()
        {
            Board theBoard = new Board(4, 4);

            FieldCoordinate from = new FieldCoordinate(3, 3);
            FieldCoordinate to = new FieldCoordinate(2, 2);
            FieldCoordinate expectedResult = new FieldCoordinate(2, 2);

            FieldCoordinate result = theBoard.GetLastInboundOnPath(from, to);

            Assert.IsTrue(FieldCoordinate.AreEqual(expectedResult, result)
                , String.Format("Expected {0}, got {1}", expectedResult, result));
        }

        [TestMethod]
        public void GetLastInboundOnPathTest_OutOfBounds()
        {
            Board theBoard = new Board(4, 4);

            FieldCoordinate from = new FieldCoordinate(3, 3);
            FieldCoordinate to = new FieldCoordinate(-1, -1);
            FieldCoordinate expectedResult = new FieldCoordinate(0, 0);

            FieldCoordinate result = theBoard.GetLastInboundOnPath(from, to);

            Assert.IsTrue(FieldCoordinate.AreEqual(expectedResult, result)
                , String.Format("Expected {0}, got {1}", expectedResult, result));
        }

        [TestMethod]
        public void GetLastInboundOnPathTest_OutOfBounds2()
        {
            Board theBoard = new Board(6, 6);

            FieldCoordinate from = new FieldCoordinate(2, 3);
            FieldCoordinate to = new FieldCoordinate(6, 0);
            FieldCoordinate expectedResult = new FieldCoordinate(5, 0);

            FieldCoordinate result = theBoard.GetLastInboundOnPath(from, to);

            Assert.IsTrue(FieldCoordinate.AreEqual(expectedResult, result)
                , String.Format("Expected {0}, got {1}", expectedResult, result));
        }

        [TestMethod]
        public void GetLastInboundOnPathTest_OutOfBounds3()
        {
            Board theBoard = new Board(4, 4);

            FieldCoordinate from = new FieldCoordinate(3, 1);
            FieldCoordinate to = new FieldCoordinate(5, -1);
            FieldCoordinate expectedResult = new FieldCoordinate(3, 1);

            FieldCoordinate result = theBoard.GetLastInboundOnPath(from, to);

            Assert.IsTrue(FieldCoordinate.AreEqual(expectedResult, result)
                , String.Format("Expected {0}, got {1}", expectedResult, result));
        }

        [TestMethod]
        public void GetLastInboundOnPathTest_OutOfBounds4()
        {
            Board theBoard = new Board(4, 4);

            FieldCoordinate from = new FieldCoordinate(3, 1);
            FieldCoordinate to = new FieldCoordinate(4, -4);
            FieldCoordinate expectedResult = new FieldCoordinate(3, 0);

            FieldCoordinate result = theBoard.GetLastInboundOnPath(from, to);

            Assert.IsTrue(FieldCoordinate.AreEqual(expectedResult, result)
                , String.Format("Expected {0}, got {1}", expectedResult, result));
        }
    }
}
