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
            Coord c1 = Coord.Build("a1");
            Coord c2 = Coord.Build("b6");
            Pieces s = Pieces.pcWhite, t = Pieces.pcBlack;
            
            Move m = new Move(c1, c2, s, t);
            Assert.AreEqual(c1, m.Source);
            Assert.AreEqual(c2, m.Target);

            Assert.AreEqual(s, m.SourcePiece);
            Assert.AreEqual(t, m.TargetPiece);
        }
    }
}
