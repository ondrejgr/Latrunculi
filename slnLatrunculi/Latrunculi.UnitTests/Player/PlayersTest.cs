﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Latrunculi.Impl;
using Latrunculi.Common;

namespace Latrunculi.UnitTests
{
    [TestClass]
    public class PlayersTest
    {
        [TestMethod]
        public void EmptyPlayers()
        {
            Board b = new LatrunculiBoard();
            Rules rul = new LatrunculiRules(b);
            Players p = new Players(b, rul, typeof(LatrunculiBrain));
            Assert.IsFalse(p.ArePlayersAssigned);
        }

        [TestMethod]
        public void TestSettings1()
        {
            Board b = new LatrunculiBoard();
            Rules rul = new LatrunculiRules(b);
            Players p = new Players(b, rul, typeof(LatrunculiBrain));
            p.SetFromString("H1C2");

            Assert.IsTrue(p.ArePlayersAssigned);
            Assert.IsInstanceOfType(p.Player1, typeof(HumanPlayer));
            Assert.IsInstanceOfType(p.Player2, typeof(ComputerPlayer));

            Assert.AreEqual(2, ((ComputerPlayer)p.Player2).Level);
        }

        [TestMethod]
        public void TestSettings2()
        {
            Board b = new LatrunculiBoard();
            Rules rul = new LatrunculiRules(b);
            Players p = new Players(b, rul, typeof(LatrunculiBrain));
            p.SetFromString("C1C2");

            Assert.IsTrue(p.ArePlayersAssigned);
            Assert.IsInstanceOfType(p.Player1, typeof(ComputerPlayer));
            Assert.IsInstanceOfType(p.Player2, typeof(ComputerPlayer));

            Assert.AreEqual(1, ((ComputerPlayer)p.Player1).Level);
            Assert.AreEqual(2, ((ComputerPlayer)p.Player2).Level);
        }

        [TestMethod]
        public void TestSettings3()
        {
            Board b = new LatrunculiBoard();
            Rules rul = new LatrunculiRules(b);
            Players p = new Players(b, rul, typeof(LatrunculiBrain));
            p.SetFromString("H1H2");

            Assert.IsTrue(p.ArePlayersAssigned);
            Assert.IsInstanceOfType(p.Player1, typeof(HumanPlayer));
            Assert.IsInstanceOfType(p.Player2, typeof(HumanPlayer));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSettings4()
        {
            Board b = new LatrunculiBoard();
            Rules rul = new LatrunculiRules(b);
            Players p = new Players(b, rul, typeof(LatrunculiBrain));
            p.SetFromString("xxxxx");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSettings5()
        {
            Board b = new LatrunculiBoard();
            Rules rul = new LatrunculiRules(b);
            Players p = new Players(b, rul, typeof(LatrunculiBrain));
            p.SetFromString("h0n2");
        }
    }
}
