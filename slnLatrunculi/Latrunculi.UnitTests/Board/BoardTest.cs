using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Latrunculi.UnitTests
{
    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void CreateBoard()
        {
            Board board = new Latrunculi.Impl.LatrunculiBoard();
            Assert.IsFalse(board.IsInitiaized);
        }

        [TestMethod]
        public void InitBoard()
        {
            Board board = new Latrunculi.Impl.LatrunculiBoard();
            board.Init();
            Assert.IsTrue(board.IsInitiaized);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AccessUninitedBoard1()
        {
            Board b = new Latrunculi.Impl.LatrunculiBoard();
            b[Coord.Parse("A1")] = Pieces.pcBlack;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AccessUninitedBoard2()
        {
            Board b = new Latrunculi.Impl.LatrunculiBoard();
            Pieces p = b[Coord.Parse("A1")];
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void AccessBoardOutOfRange1()
        {
            Board b = new Latrunculi.Impl.LatrunculiBoard();
            b.Init();
            Pieces p = b[Coord.Parse("I4")];
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void AccessBoardOutOfRange2()
        {
            Board b = new Latrunculi.Impl.LatrunculiBoard();
            b.Init();
            b[Coord.Parse("I4")] = Pieces.pcWhite;
        }

        [TestMethod]
        public void ApplyMove1()
        {
            Board b = new Latrunculi.Impl.LatrunculiBoard();
            b.Init();

            Coord c1 = Coord.Parse("A1");
            Coord c2 = Coord.Parse("A2");
            Coord c3 = Coord.Parse("B1");
            b[c1] = Pieces.pcBlack;
            b[c3] = Pieces.pcBlack;

            Move m = new Move(c1, c2, Pieces.pcNone, Pieces.pcBlack);

            Assert.AreEqual(Pieces.pcBlack, b[c1]);
            Assert.AreEqual(Pieces.pcBlack, b[c3]);
 
            b.ApplyMove(m);
            Assert.AreEqual(Pieces.pcNone, b[c1]);
            Assert.AreEqual(Pieces.pcBlack, b[c2]);
            Assert.AreEqual(Pieces.pcBlack, b[c3]);

            m.RemovedPiecesCoords.Add(c3);
            b.ApplyMove(m);
            Assert.AreEqual(Pieces.pcNone, b[c1]);
            Assert.AreEqual(Pieces.pcBlack, b[c2]);
            Assert.AreEqual(Pieces.pcNone, b[c3]);
        }

        [TestMethod]
        public void AccessBoard1()
        {
            Board b = new Latrunculi.Impl.LatrunculiBoard();
            b.Init();

            Coord c = Coord.Parse("F2");
            Pieces p = Pieces.pcBlack;
            b[c] = p;
            Assert.AreEqual(p, b[c]);
            
            c = Coord.Parse("C4");
            b[c] = p;
            Assert.AreEqual(p, b[c]);

            c = Coord.Parse("H7");
            b[c] = p;
            Assert.AreEqual(p, b[c]);

            c = Coord.Parse("A1");
            b[c] = p;
            Assert.AreEqual(p, b[c]);
        }

        [TestMethod]
        public void LatrunculiBoardCoordRangeCheck()
        {
            Board b = new Latrunculi.Impl.LatrunculiBoard();
            Coord c = new Coord();

            Assert.IsFalse(b.IsCoordValid(c));

            c.Set("J8");
            Assert.IsFalse(b.IsCoordValid(c));

            c.Set("H8");
            Assert.IsFalse(b.IsCoordValid(c));

            c.Set("A5");
            Assert.IsTrue(b.IsCoordValid(c));
        }

        [TestMethod]
        public void LatrunculiInitedBoard()
        {
            Board b = new Latrunculi.Impl.LatrunculiBoard();
            b.Init();

            Coord c1 = Coord.Parse("A1");
            Pieces p = Pieces.pcWhite;
            b[c1] = p;

            Coord c2 = Coord.Parse("H7");
            b[c2] = p;

            Coord c3 = Coord.Parse("G5");
            b[c3] = p;

            b.Init();

            Assert.AreEqual(Pieces.pcWhite, b[c1]);
            Assert.AreEqual(Pieces.pcBlack, b[c2]);
            Assert.AreEqual(Pieces.pcNone, b[c3]);
        }

        [TestMethod]
        public void GetRelativeCoordTest1()
        {
            Board b = new Latrunculi.Impl.LatrunculiBoard();
            b.Init();

            Coord? result;

            Coord c = Coord.Parse("B2");
            Coord c_f = Coord.Parse("B3");
            Coord c_a = Coord.Parse("B1");
            Coord c_l = Coord.Parse("A2");
            Coord c_r = Coord.Parse("C2");

            result = b.GetRelativeCoord(c, CoordDirectionEnum.deForward);
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(c_f, result.Value);

            result = b.GetRelativeCoord(c, CoordDirectionEnum.deAft);
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(c_a, result.Value);

            result = b.GetRelativeCoord(c, CoordDirectionEnum.deLeft);
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(c_l, result.Value);

            result = b.GetRelativeCoord(c, CoordDirectionEnum.deRight);
            Assert.AreEqual(c_r, result.Value);
        }

        [TestMethod]
        public void GetRelativeCoordTest2()
        {
            Board b = new Latrunculi.Impl.LatrunculiBoard();
            b.Init();

            Coord? result;

            Coord c = Coord.Parse("A1");
            Coord c_f = Coord.Parse("A2");
            Coord c_r = Coord.Parse("B1");

            result = b.GetRelativeCoord(c, CoordDirectionEnum.deForward);
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(c_f, result.Value);

            result = b.GetRelativeCoord(c, CoordDirectionEnum.deAft);
            Assert.IsFalse(result.HasValue);

            result = b.GetRelativeCoord(c, CoordDirectionEnum.deLeft);
            Assert.IsFalse(result.HasValue);

            result = b.GetRelativeCoord(c, CoordDirectionEnum.deRight);
            Assert.AreEqual(c_r, result.Value);
        }
    }
}
