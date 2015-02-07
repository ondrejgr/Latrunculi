using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    /// <summary>
    /// Souřadnice
    /// </summary>
    public struct Coord: IEquatable<Coord>
    {
        private char _x;
        public char x
        {
            get
            {
                return _x;
            }
            private set
            {
                _x = value;
            }
        }

        private byte _y;
        public byte y
        {
            get
            {
                return _y;
            }
            private set
            {
                _y = value;
            }
        }

        public void Set(char x, byte y)
        {
            if (!IsValidCoord(x, y))
                throw new ArgumentException("Neplatné zadání souřadnice.");

            x = Char.ToUpper(x);
            _x = x;
            _y = y;
        }

        public void Set(string coord)
        {
            if (string.IsNullOrWhiteSpace(coord) || coord.Length != 2)
                throw new ArgumentException("Neplatné zadání souřadnice.", "coord");

            byte y = 0;
            if (!byte.TryParse(string.Format("{0}", coord[1]), out y))
                throw new ArgumentException("Neplatné zadání souřadnice.", "coord");

            char x = Char.ToUpper(coord[0]);

            if (!IsValidCoord(x, y))
                throw new ArgumentException("Neplatné zadání souřadnice.", "coord");
            
            _x = x;
            _y = y;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is Coord))
                return false;
            else
                return (x == ((Coord)obj).x) && 
                       (y == ((Coord)obj).y);
        }

        public bool Equals(Coord other)
        {
            return (x == other.x) &&
                   (y == other.y);
        }        
        
        public override int GetHashCode()
        {
            return y * 100 + (byte)x;
        }

        public override string ToString()
        { 
            return string.Format("{0}{1}", _x, _y);
        }

        /// <summary>
        /// Pro overeni rozsahu hodnot v methodach Set
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static private bool IsValidCoord(char x, byte y)
        {
            if (!Char.IsUpper(x))
                x = Char.ToUpper(x);
            return x >= 'A' && x <= 'Z' &&
                   y >= 1 && y <= 9;
        }

        /// <summary>
        /// Vytvorit Coord
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static public Coord Create(char x, byte y)
        {
            Coord c = new Coord();
            c.Set(x, y);
            return c;
        }

        /// <summary>
        /// Vytvorit Coord z retezce.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        static public Coord Parse(string coord)
        {
            Coord c = new Coord();
            c.Set(coord);
            return c;
        }
    }
}
