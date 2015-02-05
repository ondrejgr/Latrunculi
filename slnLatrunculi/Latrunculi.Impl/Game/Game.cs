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
        public event RenderBoardEvent RenderBoard;
        public event RenderActivePlayerEvent RenderActivePlayer;

        private readonly Board _board = new LatrunculiBoard();
        private Board Board
        {
            get
            {
                return _board;
            }
        }

        private readonly Rules _rules = new LatrunculiRules();
        private Rules Rules
        {
            get
            {
                return _rules;
            }
        }

        private Players _players;
        private Players Players
        {
            get
            {
                return _players;
            }
        }

        private Player _activePlayer;
        private Player ActivePlayer
        {
            get
            {
                return _activePlayer;
            }
        }

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

        protected virtual void OnRenderBoard()
        {
            if (RenderBoard != null)
                RenderBoard(this, Board);
        }

        protected virtual void OnRenderActivePlayer()
        {
            if (RenderActivePlayer != null)
                RenderActivePlayer(this, ActivePlayer);
        }

        /// <summary>
        /// Spustit hru
        /// </summary>
        /// <param name="players">Nastaveni hracu</param>
        public void Run(Players players, Player activePlayer)
        {
            Rules.CheckPlayers(players);
            _players = players;

            if (activePlayer == null)
                activePlayer = players.GetPlayerByColor(Rules.GetFirstActivePlayerColor());
            _activePlayer = activePlayer;
            if (ActivePlayer == null)
                throw new Exception("Nepodařilo se zjistit, který hráč je na tahu.");

            Board.Init();
            OnRenderBoard();
            OnRenderActivePlayer();
        }
    }
}
