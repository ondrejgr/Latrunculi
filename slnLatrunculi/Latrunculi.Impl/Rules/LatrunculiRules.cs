using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi.Impl
{
    public class LatrunculiRules: Rules
    {
        public LatrunculiRules(Board board): base(board)
        {
            
        }

        protected override bool OnIsMoveValid(Move move, GameColorsEnum color)
        {
            // TODO: kontrola tahu
            return false;
        }
    }
}
