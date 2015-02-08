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
    public class Move: IEquatable<Move>
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

        private readonly List<Coord> _removedPiecesCoords = new List<Coord>();
        public List<Coord> RemovedPiecesCoords
        {
            get
            {
                return _removedPiecesCoords;
            }
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

        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is Move))
                return false;
            else
            {
                Move m = (Move)obj;
                return Source.Equals(m.Source) &&
                       Target.Equals(m.Target) &&
                       (SourcePiece == m.SourcePiece) &&
                       (TargetPiece == m.TargetPiece);
                       //RemovedPiecesCoords.SequenceEqual(m.RemovedPiecesCoords);
            }
        }

        public bool Equals(Move other)
        {
            return (other is Move) &&
                       Source.Equals(other.Source) &&
                       Target.Equals(other.Target) &&
                       (SourcePiece == other.SourcePiece) &&
                       (TargetPiece == other.TargetPiece);
                       //RemovedPiecesCoords.SequenceEqual(other.RemovedPiecesCoords);
        }

        public override int GetHashCode()
        {
            return Source.GetHashCode() + 1000 * Target.GetHashCode()
                    + 1000000 * SourcePiece.GetHashCode() + 10000000 * TargetPiece.GetHashCode();
                    //+ 100000000 * RemovedPiecesCoords.GetHashCode(); // zamerne NE - RemovedPiecesCoords nema vliv na porovnani
        }
    }
}
