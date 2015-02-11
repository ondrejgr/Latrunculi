using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Latrunculi.Common
{
    public abstract class Brain
    {
        public Brain(Board board, Rules rules)
        {
            if (board == null)
                throw new ArgumentNullException("board");
            if (rules == null)
                throw new ArgumentNullException("rules");
            _board = board;
            _rules = rules;
        }

        public virtual void ComputeBestMove(int level, GameColorsEnum color)
        {
            if (level < 0 || level > 2)
                throw new ArgumentOutOfRangeException("level");
                        
            BestMove = null;
            OnComputeMove(level, color);
        }

        protected virtual void OnComputeMove(int level, GameColorsEnum color)
        {
            throw new NotImplementedException();
        }

        private Board _board;
        protected Board Board
        {
            get
            {
                return _board;
            }
        }

        private Rules _rules;
        protected Rules Rules
        {
            get
            {
                return _rules;
            }
        }

        private Move _bestMove;
        public Move BestMove
        {
            get
            {
                return _bestMove;
            }
            protected set
            {
                _bestMove = value;
            }
        }
    }
}
