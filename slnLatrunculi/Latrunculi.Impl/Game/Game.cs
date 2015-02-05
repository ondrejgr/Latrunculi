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
        public event RenderBoardRequestEvent RenderBoardRequest;

        private Board Board = new LatrunculiBoard();

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

        private void OnRenderBoardRequest()
        {
            if (RenderBoardRequest != null)
                RenderBoardRequest(this, Board);
        }

        public void Run()
        {
            Board.Init();
            OnRenderBoardRequest();
        }
    }
}
