using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    public class HumanPlayer: Player
    {
        public Move HumanMove
        {
            get;
            set;
        }

        public HumanPlayer(GameColorsEnum color)
            : base(color)
        {
        }

        public override Move GetMove()
        {
            return HumanMove;
        }

        public override string ToString()
        {
            return string.Format("H{0}", 0);
        }
    }
}
