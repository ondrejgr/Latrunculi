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
        event RenderBoardEvent RenderBoard;
        event RenderActivePlayerEvent RenderActivePlayer;
        event MoveInvalidEvent MoveInvalid;
        event GameOverEvent GameOver;

        Board Board
        {
            get;
        }

        string Title
        {
            get;
        }

        string Version
        {
            get;
        }

        void Run(string playersSetting);
    }
}
