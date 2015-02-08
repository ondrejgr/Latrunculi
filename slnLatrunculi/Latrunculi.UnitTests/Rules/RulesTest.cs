using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Latrunculi.Impl;

namespace Latrunculi.UnitTests
{
    [TestClass]
    public class RulesTest
    {
        [TestMethod]
        public void IsMoveValid1()
        {
            Board b = new LatrunculiBoard();
            b.Init();
            Rules rules = new LatrunculiRules(b);

            Coord c1 = new Coord();
            Coord c2 = new Coord();

            c1.Set("a1");
            c2.Set("a2");
            Move m = new Move(c1, c2, Pieces.pcNone, Pieces.pcWhite);
            Assert.IsFalse(rules.IsMoveValid(m, GameColorsEnum.plrWhite));
        }

        [TestMethod]
        public void IsMoveValid2()
        {
            Board b = new LatrunculiBoard();
            b.Init();
            Rules rules = new LatrunculiRules(b);

            Coord c1 = new Coord();
            Coord c2 = new Coord();

            c1.Set("a2");
            c2.Set("a3");
            Move m = new Move(c1, c2, Pieces.pcNone, Pieces.pcWhite);
            Assert.IsTrue(rules.IsMoveValid(m, GameColorsEnum.plrWhite));
        }

        [TestMethod]
        public void IsMoveValid3()
        {
            Board b = new LatrunculiBoard();
            b.Init();
            Rules rules = new LatrunculiRules(b);

            Coord c1 = new Coord();
            Coord c2 = new Coord();

            c1.Set("b6");
            c2.Set("b5");
            Move m = new Move(c1, c2, Pieces.pcNone, Pieces.pcWhite);
            Assert.IsFalse(rules.IsMoveValid(m, GameColorsEnum.plrWhite));
        }

        [TestMethod]
        public void IsMoveValid4()
        {
            Board b = new LatrunculiBoard();
            b.Init();
            Rules rules = new LatrunculiRules(b);

            Coord c1 = new Coord();
            Coord c2 = new Coord();

            c1.Set("b2");
            c2.Set("c3");
            Move m = new Move(c1, c2, Pieces.pcNone, Pieces.pcWhite);
            Assert.IsFalse(rules.IsMoveValid(m, GameColorsEnum.plrWhite));
        }


        [TestMethod]
        public void RemovePieces1()
        {
            Board b = new LatrunculiBoard();
            b.Init();
            Rules rules = new LatrunculiRules(b);

            b[Coord.Parse("b6")] = Pieces.pcNone;
            b[Coord.Parse("b4")] = Pieces.pcBlack;

            b[Coord.Parse("a2")] = Pieces.pcNone;
            b[Coord.Parse("a3")] = Pieces.pcWhite;

            b[Coord.Parse("c2")] = Pieces.pcNone;
            b[Coord.Parse("c3")] = Pieces.pcWhite;

            // dobrovolne vstoupeni mezi dva nepratele
            Coord c1 = new Coord();
            Coord c2 = new Coord();
            c1.Set("b4");
            c2.Set("b3");
            Move m = new Move(c1, c2, Pieces.pcNone, Pieces.pcBlack);
            Assert.IsTrue(rules.IsMoveValid(m, GameColorsEnum.plrBlack));
            rules.SetPiecesToBeRemoved(m);
            b.ApplyMove(m);
            Assert.AreEqual(Pieces.pcBlack, b[c2]);
        }

        [TestMethod]
        public void RemovePieces2()
        {
            Board b = new LatrunculiBoard();
            b.Init();
            Rules rules = new LatrunculiRules(b);

            b[Coord.Parse("b6")] = Pieces.pcNone;
            b[Coord.Parse("b3")] = Pieces.pcBlack;

            b[Coord.Parse("a2")] = Pieces.pcNone;
            b[Coord.Parse("a3")] = Pieces.pcWhite;

            // sebrani cerneho bilym
            Coord c1 = new Coord();
            Coord c2 = new Coord();
            c1.Set("c2");
            c2.Set("c3");
            Move m = new Move(c1, c2, Pieces.pcNone, Pieces.pcWhite);
            Assert.IsTrue(rules.IsMoveValid(m, GameColorsEnum.plrWhite));
            rules.SetPiecesToBeRemoved(m);
            b.ApplyMove(m);
            Assert.AreEqual(Pieces.pcNone, b[Coord.Parse("b3")]);
        }

        [TestMethod]
        public void RemovePieces3()
        {
            Board b = new LatrunculiBoard();
            b.Init();
            Rules rules = new LatrunculiRules(b);

            b[Coord.Parse("b6")] = Pieces.pcNone;
            b[Coord.Parse("a1")] = Pieces.pcBlack;

            b[Coord.Parse("a2")] = Pieces.pcNone;

            // obkliceni v rohu
            Coord c1 = new Coord();
            Coord c2 = new Coord();
            c1.Set("b2");
            c2.Set("a2");
            Move m = new Move(c1, c2, Pieces.pcNone, Pieces.pcWhite);
            Assert.IsTrue(rules.IsMoveValid(m, GameColorsEnum.plrWhite));
            rules.SetPiecesToBeRemoved(m);
            b.ApplyMove(m);
            Assert.AreEqual(Pieces.pcNone, b[Coord.Parse("a1")]);
        }

        [TestMethod]
        public void WinnerTest1()
        {
            Board b = new LatrunculiBoard();
            b.Init();
            Rules rules = new LatrunculiRules(b);

            Coord c1 = new Coord();

            c1.Set("a2");
            b[c1] = Pieces.pcNone;

            Assert.IsTrue(rules.GetWinner().HasValue);
            Assert.AreEqual(GameColorsEnum.plrBlack, rules.GetWinner().Value);
        }

        [TestMethod]
        public void WinnerTest2()
        {
            Board b = new LatrunculiBoard();
            b.Init();
            Rules rules = new LatrunculiRules(b);

            Coord c1 = new Coord();

            c1.Set("a6");
            b[c1] = Pieces.pcNone;

            Assert.IsTrue(rules.GetWinner().HasValue);
            Assert.AreEqual(GameColorsEnum.plrWhite, rules.GetWinner().Value);
        }

        [TestMethod]
        public void WinnerTest3()
        {
            Board b = new LatrunculiBoard();
            b.Init();
            Rules rules = new LatrunculiRules(b);


            Assert.IsFalse(rules.GetWinner().HasValue);
        }
    }
}
