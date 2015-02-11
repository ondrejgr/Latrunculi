using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    public class HumanPlayer: Player
    {
        public HumanPlayer(GameColorsEnum color, int level = 1)
            : base(color, level)
        {
        }

        public override Move GetMove(System.Threading.CancellationToken ct)
        {
            return HumanMove.Move;
        }

        public override string ToString()
        {
            return string.Format("H{0}", Level);
        }
    }
}
