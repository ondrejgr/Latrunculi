using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    public class ComputerPlayer : Player
    {
        public ComputerPlayer(GameColorsEnum color, Board board, Rules rules, int level = 1)
            : base(color)
        {
            if (board == null)
                throw new ArgumentNullException("board");
            if (rules == null)
                throw new ArgumentNullException("rules");
            Board = board;
            Rules = rules;
            Level = level;
        }

        private Board Board;
        private Rules Rules;

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

        Random r = new Random();
        public override Move GetMove()
        {
            Moves moves = Rules.GetValidMoves(Color);
            
            if (moves.Count == 0)
                throw new InvalidOperationException("Žádné volné tahy !");

            return moves[r.Next(0, moves.Count)];
        }

        public override string ToString()
        {
            return string.Format("C{0}", Level);
        }
    }
}