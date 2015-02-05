using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    public delegate void RenderBoardEvent(IGame Sender, Board Board);
    public delegate void RenderActivePlayerEvent(IGame Sender, Player Player);
}
