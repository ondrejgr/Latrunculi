using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi.Common
{
    public struct RemovedPiece
    {
        public Coord Coord
        {
            get;
            set;
        }

        public Pieces Piece
        {
            get;
            set;
        }

        static public RemovedPiece Create(Coord coord, Pieces piece)
        {
            return new RemovedPiece() { Coord = coord, Piece = piece };
        }
    }
}
