using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Latrunculi.UnitTests
{
    /// <summary>
    /// Summary description for CoordTest
    /// </summary>
    [TestClass]
    public class CoordTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CoordEmpty()
        {
            Coord x = Coord.Parse("  ");
        }
             
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CoordInvalid1()
        {
            Coord x = Coord.Parse("abc");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CoordInvalid2()
        {
            Coord x = Coord.Parse("ax");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CoordInvalid3()
        {
            Coord x = Coord.Parse("!5");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CoordInvalid4()
        {
            Coord x = Coord.Parse("!99");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CoordInvalid5()
        {
            Coord x = Coord.Parse("A99");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CoordInvalid6()
        {
            Coord x = Coord.Parse("ZZ1");
        }

        [TestMethod]
        public void CoordValid()
        {
            Coord c;

            c = Coord.Parse("a1");
            c.Set("B4");

            for (char x = 'A'; x <= 'Z'; x++)
            {
                for (byte y = 1; y <= 9; y++)
                {
                    c.Set(string.Format("{0}{1}", x, y));
                    Assert.AreEqual(x, c.x);
                    Assert.AreEqual(y, c.y);
                }
            }
            for (char x = 'A'; x <= 'Z'; x++)
            {
                for (byte y = 1; y <= 9; y++)
                {
                    c.Set(x, y);
                    Assert.AreEqual(x, c.x);
                    Assert.AreEqual(y, c.y);
                }
            }
        }
    }
}
