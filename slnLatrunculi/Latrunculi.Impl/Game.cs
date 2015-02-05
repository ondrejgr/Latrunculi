using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi.Impl
{
    /// <summary>
    /// Objekt logiky hry
    /// </summary>
    public class Game: IGame
    {
        public string Title
        {
            get
            {
                return "Latrunculi";
            }
        }

        public string Version
        {
            get
            {
                return "1.0";
            }
        }
    }
}
