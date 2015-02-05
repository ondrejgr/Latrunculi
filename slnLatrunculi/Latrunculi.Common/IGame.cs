using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    /// <summary>
    /// Interface pro vytváření instancí logiky hry
    /// </summary>
    public interface IGame
    {
        string Title
        {
            get;
        }

        string Version
        {
            get;
        }
    }
}
