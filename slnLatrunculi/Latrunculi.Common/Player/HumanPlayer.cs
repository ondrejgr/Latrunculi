using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    public class HumanPlayer: Player
    {
        public HumanPlayer(PlayersEnum color)
            : base(color)
        {
        }

        public override string ToString()
        {
            return string.Format("H{0}", 0);
        }
    }
}
