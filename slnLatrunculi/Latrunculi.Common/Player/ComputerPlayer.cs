using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    public class ComputerPlayer : Player
    {
        public ComputerPlayer(GameColorsEnum color, Board board, int level = 1)
            : base(color)
        {
            if (board == null)
                throw new ArgumentNullException("board");
            Board = board;
            Level = level;
        }

        private Board Board;

        private int _level;
        /// <summary>
        /// Obtiznost PC hrace (0 = lehka,1,2 = tezka);
        /// </summary>
        public virtual int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }

        public override Move GetMove()
        {
            Coord src = new Coord();
            Coord tar = new Coord();

            src.Set("A1");
            tar.Set("A2");

            return new Move(src, tar, Pieces.pcNone, (Color == GameColorsEnum.plrBlack) ? Pieces.pcBlack : Pieces.pcWhite);
        }

        public override string ToString()
        {
            return string.Format("C{0}", Level);
        }
    }
}