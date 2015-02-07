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
        public event MoveInvalidEvent MoveInvalid;
        public event GameOverEvent GameOver;

        public Game()
        {
            _board = new LatrunculiBoard();
            _rules = new LatrunculiRules(_board);
            _players = new Players(_board);
        }

        private readonly Board _board;
        public Board Board
        {
            get
            {
                return _board;
            }
        }

        private readonly Rules _rules;
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
                RenderBoard(this);
        }

        protected virtual void OnRenderActivePlayer()
        {
            if (RenderActivePlayer != null)
                RenderActivePlayer(this, ActivePlayer);
        }

        protected virtual void OnMoveInvalid(Move Move)
        {
            if (MoveInvalid != null)
                MoveInvalid(this, ActivePlayer, Move);
        }

        protected virtual void OnGameOver(IGame Sender, Player Winner)
        {
            if (GameOver != null)
                GameOver(this, Winner);
        }

        /// <summary>
        /// Spustit hru
        /// </summary>
        public void Run(string playersSetting)
        {
            // nastavit hrace
            Players.SetFromString(playersSetting);
            Rules.CheckPlayers(Players);

            // hrač na tahu - určí jej pravidla (začátek hry)
            _activePlayer = Players.GetPlayerByColor(Rules.GetFirstActivePlayerColor()); 
            if (ActivePlayer == null)
                throw new Exception("Nepodařilo se zjistit, který hráč je na tahu.");

            Board.Init();
            OnRenderBoard();

            Move move;
            do
            {
                OnRenderActivePlayer();

                move = ActivePlayer.GetMove();
                if (move == null)
                {
                    OnGameOver(this, null);
                    return; // prazdny tah je signal pro ukonceni hry
                }
            } while ((ActivePlayer is HumanPlayer) && !Rules.IsMoveValid(move, ActivePlayer.Color));
        }

        /// <summary>
        /// Zmenit nastaveni hracu podle retezce (pr. H0C1)
        /// </summary>
        /// <param name="newSettings"></param>
        public void SetPlayersFromString(string newSettings)
        {
            throw new NotImplementedException();
        }
    }
}
