using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    /// <summary>
    /// Abstraktni hrac
    /// </summary>
    public abstract class Player
    {
        public virtual Move GetMove()
        {
            throw new NotImplementedException();
        }
    }
}
