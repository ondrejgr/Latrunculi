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
    }
}
