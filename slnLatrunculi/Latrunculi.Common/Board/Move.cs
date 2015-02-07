using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    /// <summary>
    /// Tah
    /// </summary>
    public class Move
    {
        public Coord Source
        {
            get;
            private set;
        }

        public Coord Target
        {
            get;
            private set;
        }

        public Pieces SourcePiece
        {
            get;
            private set;
        }

        public Pieces TargetPiece
        {
            get;
            private set;
        }

        public Move(Coord source, Coord target, Pieces sourcePiece, Pieces targetPiece)
        {
            Source = source;
            Target = target;

            SourcePiece = sourcePiece;
            TargetPiece = targetPiece;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", Source, Target);
        }

        /// <summary>
        /// Vytvoř tah ze zadání od lidského hráče.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public Move Parse(string str, Pieces targetPiece)
        {
            Move move = null;
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                    throw new ArgumentException("Řetězec je prázdný !");

                str = str.ToUpper();
                if (str.Length != 4)
                    throw new ArgumentException("Neplatná délka řetězce !");

                Coord source, target;
                source = Coord.Parse(str.Substring(0, 2));
                target = Coord.Parse(str.Substring(2, 2));

                move = new Move(source, target, Pieces.pcNone, targetPiece);
            }
            catch (Exception exc)
            {
                throw new MoveInvalidException(str, exc);
            }

            return move;
        }
    }
}
