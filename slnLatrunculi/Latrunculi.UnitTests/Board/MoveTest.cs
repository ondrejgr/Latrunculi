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
    }
}
