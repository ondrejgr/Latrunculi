using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Latrunculi.UnitTests
{
    [TestClass]
    public class MoveTest
    {
        [TestMethod]
        public void CreateMove()
        {
            Coord c1 = Coord.Parse("a1");
            Coord c2 = Coord.Parse("b6");
            Pieces s = Pieces.pcWhite, t = Pieces.pcBlack;
            
            Move m = new Move(c1, c2, s, t);
            Assert.AreEqual(c1, m.Source);
            Assert.AreEqual(c2, m.Target);

            Assert.AreEqual(s, m.SourcePiece);
            Assert.AreEqual(t, m.TargetPiece);
        }

        [TestMethod]
        [ExpectedException(typeof(MoveInvalidException))]
        public void ParseMoveFail1()
        {
            Move.Parse(null, Pieces.pcNone);
        }

        [TestMethod]
        [ExpectedException(typeof(MoveInvalidException))]
        public void ParseMoveFail2()
        {
            Move.Parse("ababee", Pieces.pcNone);
        }

        [TestMethod]
        [ExpectedException(typeof(MoveInvalidException))]
        public void ParseMoveFail3()
        {
            Move.Parse("00a1", Pieces.pcNone);
        }

        [TestMethod]
        public void MoveEqual1()
        {
            Coord c1 = Coord.Parse("a1");
            Coord c2 = Coord.Parse("b6");
            Pieces s = Pieces.pcWhite, t = Pieces.pcBlack;

            Move m1 = new Move(c1, c2, s, t);
            Move m2 = new Move(c1, c2, s, t);

            c2.Set("b7");
            m2 = new Move(c1, c2, s, t);
            Assert.AreNotEqual(m1, m2);
        }

        [TestMethod]
        public void MoveEqual2()
        {
            Coord c1 = Coord.Parse("a1");
            Coord c2 = Coord.Parse("b6");
            Pieces s = Pieces.pcWhite, t = Pieces.pcBlack;

            Move m1 = new Move(c1, c2, s, t);
            Move m2 = new Move(c1, c2, s, t);

            m2 = new Move(c1, c2, s, t);
            Assert.AreEqual(m1, m2);

            // zamerne - RemovedPiecesCoords se nepouziva pro porovnani tahu
            m1.RemovedPiecesCoords.Add(Coord.Parse("c1"));
            Assert.AreEqual(m1, m2);

            m2.RemovedPiecesCoords.Add(Coord.Parse("c1"));
            Assert.AreEqual(m1, m2);
        }
    }
}
