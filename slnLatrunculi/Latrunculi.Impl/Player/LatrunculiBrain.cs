using Latrunculi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi.Impl
{
    public class LatrunculiBrain: Brain
    {
        public LatrunculiBrain(Board board, Rules rules)
            : base(board, rules)
        {
        }

        Random r = new Random();
        protected override void OnComputeMove(int level, GameColorsEnum color, System.Threading.CancellationToken ct)
        {
            Moves moves = Rules.GetValidMoves(color);
            ct.ThrowIfCancellationRequested();

            System.Threading.Thread.Sleep(3000);

            ct.ThrowIfCancellationRequested();
            BestMove = moves[r.Next(0, moves.Count)];
        }
    }
}
