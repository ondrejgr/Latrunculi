using Latrunculi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    public class ComputerPlayer : Player
    {
        public ComputerPlayer(GameColorsEnum color, Brain brain, int level = 1):base(color, level)
        {
            if (brain == null)
                throw new ArgumentNullException("brain");
            _brain = brain;
        }

        private Brain _brain;
        private Brain Brain
        {
            get
            {
                return _brain;
            }
        }

        public override Move GetMove(System.Threading.CancellationToken ct)
        {
            Move result = null;

            
            return base.GetMove(ct);
        }

        public override string ToString()
        {
            return string.Format("C{0}", Level);
        }
    }
}