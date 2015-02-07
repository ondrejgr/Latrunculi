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

        public override string ToString()
        {
            return string.Format("C{0}", Level);
        }
    }
}