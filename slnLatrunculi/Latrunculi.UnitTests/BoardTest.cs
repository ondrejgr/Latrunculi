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
            b[Coord.Build("A1")] = Pieces.pcBlack;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AccessUninitedBoard2()
        {
            Board b = new Latrunculi.Impl.LatrunculiBoard();
            Pieces p = b[Coord.Build("A1")];
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void AccessBoardOutOfRange1()
        {
            Board b = new Latrunculi.Impl.LatrunculiBoard();
            b.Init();
            Pieces p = b[Coord.Build("I4")];
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void AccessBoardOutOfRange2()
        {
            Board b = new Latrunculi.Impl.LatrunculiBoard();
            b.Init();
            b[Coord.Build("I4")] = Pieces.pcWhite;
        }

        [TestMethod]
        public void AccessBoard1()
        {
            Board b = new Latrunculi.Impl.LatrunculiBoard();
            b.Init();

            Coord c = Coord.Build("F2");
            Pieces p = Pieces.pcBlack;
            b[c] = p;
            Assert.AreEqual(p, b[c]);
            
            c = Coord.Build("C4");
            b[c] = p;
            Assert.AreEqual(p, b[c]);

            c = Coord.Build("H7");
            b[c] = p;
            Assert.AreEqual(p, b[c]);

            c = Coord.Build("A1");
            b[c] = p;
            Assert.AreEqual(p, b[c]);
        }

        [TestMethod]
        public void LatrunculiInitedBoard()
        {
            Board b = new Latrunculi.Impl.LatrunculiBoard();
            b.Init();

            Coord c1 = Coord.Build("A1");
            Pieces p = Pieces.pcWhite;
            b[c1] = p;

            Coord c2 = Coord.Build("H7");
            b[c2] = p;

            Coord c3 = Coord.Build("G5");
            b[c3] = p;

            b.Init();

            Assert.AreEqual(Pieces.pcWhite, b[c1]);
            Assert.AreEqual(Pieces.pcBlack, b[c2]);
            Assert.AreEqual(Pieces.pcNone, b[c3]);
        }
    }
}
