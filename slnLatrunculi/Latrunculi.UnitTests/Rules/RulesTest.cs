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

            b[Coord.Parse("b3")] = Pieces.pcNone;

            // dobrovolne vstoupeni mezi dva nepratele - nebrat
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
            b[Coord.Parse("c3")] = Pieces.pcNone;

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
        public void RemovePieces4()
        {
            Board b = new LatrunculiBoard();
            b.Init();
            Rules rules = new LatrunculiRules(b);

            b[Coord.Parse("a5")] = Pieces.pcNone;
            b[Coord.Parse("b5")] = Pieces.pcNone;
            b[Coord.Parse("a4")] = Pieces.pcNone;
            b[Coord.Parse("b4")] = Pieces.pcNone;
            b[Coord.Parse("c3")] = Pieces.pcNone;
            
            b[Coord.Parse("c4")] = Pieces.pcBlack;
            b[Coord.Parse("b3")] = Pieces.pcBlack;

            b[Coord.Parse("c5")] = Pieces.pcWhite;
            b[Coord.Parse("a3")] = Pieces.pcWhite;
            b[Coord.Parse("c2")] = Pieces.pcWhite;

            // sebrani 2 x cerneho bilym
            //        o
            //        x
            //      ox <-o
            Coord c1 = new Coord();
            Coord c2 = new Coord();
            c1.Set("c2");
            c2.Set("c3");
            Move m = new Move(c1, c2, Pieces.pcNone, Pieces.pcWhite);
            Assert.IsTrue(rules.IsMoveValid(m, GameColorsEnum.plrWhite));
            rules.SetPiecesToBeRemoved(m);
            b.ApplyMove(m);
            Assert.AreEqual(Pieces.pcNone, b[Coord.Parse("c4")]);
            Assert.AreEqual(Pieces.pcNone, b[Coord.Parse("b3")]);
        }

        [TestMethod]
        public void RemovePieces5()
        {
            Board b = new LatrunculiBoard();
            b.Init();
            Rules rules = new LatrunculiRules(b);

            b[Coord.Parse("a5")] = Pieces.pcNone;
            b[Coord.Parse("b5")] = Pieces.pcNone;
            b[Coord.Parse("a4")] = Pieces.pcNone;
            b[Coord.Parse("b4")] = Pieces.pcNone;
            b[Coord.Parse("c3")] = Pieces.pcNone;

            b[Coord.Parse("d5")] = Pieces.pcNone;
            b[Coord.Parse("e5")] = Pieces.pcNone;
            b[Coord.Parse("d4")] = Pieces.pcNone;
            b[Coord.Parse("e4")] = Pieces.pcNone;

            b[Coord.Parse("c4")] = Pieces.pcBlack;
            b[Coord.Parse("b3")] = Pieces.pcBlack;
            b[Coord.Parse("d3")] = Pieces.pcBlack;

            b[Coord.Parse("c5")] = Pieces.pcWhite;
            b[Coord.Parse("a3")] = Pieces.pcWhite;
            b[Coord.Parse("c2")] = Pieces.pcWhite;

            // nepovedene sebrani 3 x cerneho bilym
            //        o
            //        x
            //      ox xo
            Coord c1 = new Coord();
            Coord c2 = new Coord();
            c1.Set("c2");
            c2.Set("c3");
            Move m = new Move(c1, c2, Pieces.pcNone, Pieces.pcWhite);
            Assert.IsTrue(rules.IsMoveValid(m, GameColorsEnum.plrWhite));
            rules.SetPiecesToBeRemoved(m);
            b.ApplyMove(m);
            Assert.AreEqual(Pieces.pcNone, b[Coord.Parse("c4")]);
            Assert.AreEqual(Pieces.pcNone, b[Coord.Parse("b3")]);
            Assert.AreEqual(Pieces.pcBlack, b[Coord.Parse("d3")]);
        }

        [TestMethod]
        public void RemovePieces6()
        {
            Board b = new LatrunculiBoard();
            b.Init();
            Rules rules = new LatrunculiRules(b);

            b[Coord.Parse("a5")] = Pieces.pcNone;
            b[Coord.Parse("b5")] = Pieces.pcNone;
            b[Coord.Parse("a4")] = Pieces.pcNone;
            b[Coord.Parse("b4")] = Pieces.pcNone;
            b[Coord.Parse("c3")] = Pieces.pcNone;

            b[Coord.Parse("d5")] = Pieces.pcNone;
            b[Coord.Parse("e5")] = Pieces.pcNone;
            b[Coord.Parse("d4")] = Pieces.pcNone;
            b[Coord.Parse("e4")] = Pieces.pcNone;

            b[Coord.Parse("c4")] = Pieces.pcBlack;
            b[Coord.Parse("b3")] = Pieces.pcBlack;
            b[Coord.Parse("d3")] = Pieces.pcBlack;

            b[Coord.Parse("c5")] = Pieces.pcWhite;
            b[Coord.Parse("a3")] = Pieces.pcWhite;
            b[Coord.Parse("c2")] = Pieces.pcWhite;
            b[Coord.Parse("e3")] = Pieces.pcWhite;

            // sebrani 3 x cerneho bilym
            //        o
            //        xS
            //      ox xo
            Coord c1 = new Coord();
            Coord c2 = new Coord();
            c1.Set("c2");
            c2.Set("c3");
            Move m = new Move(c1, c2, Pieces.pcNone, Pieces.pcWhite);
            Assert.IsTrue(rules.IsMoveValid(m, GameColorsEnum.plrWhite));
            rules.SetPiecesToBeRemoved(m);
            b.ApplyMove(m);
            Assert.AreEqual(Pieces.pcNone, b[Coord.Parse("c4")]);
            Assert.AreEqual(Pieces.pcNone, b[Coord.Parse("b3")]);
            Assert.AreEqual(Pieces.pcNone, b[Coord.Parse("d3")]);
        }

        [TestMethod]
        public void RemovePieces7()
        {
            Board b = new LatrunculiBoard();
            b.Init();
            Rules rules = new LatrunculiRules(b);

            b[Coord.Parse("c2")] = Pieces.pcNone;
            b[Coord.Parse("d2")] = Pieces.pcNone;
            b[Coord.Parse("b1")] = Pieces.pcNone;

            b[Coord.Parse("a1")] = Pieces.pcBlack;
            b[Coord.Parse("c1")] = Pieces.pcBlack;

            b[Coord.Parse("a2")] = Pieces.pcWhite;
            b[Coord.Parse("b2")] = Pieces.pcWhite;
            b[Coord.Parse("d1")] = Pieces.pcWhite;

            // kombinace rohu a vicenasobneho sebrani
            //    |  
            //    |oo  
            //    |x xo      
            //    ------------
            Coord c1 = new Coord();
            Coord c2 = new Coord();
            c1.Set("b2");
            c2.Set("b1");
            Move m = new Move(c1, c2, Pieces.pcNone, Pieces.pcWhite);
            Assert.IsTrue(rules.IsMoveValid(m, GameColorsEnum.plrWhite));
            rules.SetPiecesToBeRemoved(m);
            b.ApplyMove(m);
            Assert.AreEqual(Pieces.pcNone, b[Coord.Parse("a1")]);
            Assert.AreEqual(Pieces.pcNone, b[Coord.Parse("c1")]);
            Assert.AreEqual(Pieces.pcWhite, b[Coord.Parse("b1")]);
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
