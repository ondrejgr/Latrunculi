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

        public string CurrentPlayersSetting
        {
            get
            {
                if (Players != null)
                    return Players.ToString();
                else
                    return null;
            }
        }

        private Player _activePlayer;
        private Player ActivePlayer
        {
            get
            {
                return _activePlayer;
            }
            set 
            {
                _activePlayer = value;
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

        private bool quit_request = false;
        public void RequestQuit()
        {
            quit_request = true;
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
            ActivePlayer = Players.GetPlayerByColor(Rules.GetFirstActivePlayerColor()); 
            if (ActivePlayer == null)
                throw new Exception("Nepodařilo se zjistit, který hráč je na tahu.");

            Board.Init();

            // smycka Manazera
            while (true)
            {
                // vykresleni
                OnRenderBoard();

                // zjisti tah
                Move move;
                bool isMoveValid;
                do
                {
                    OnRenderActivePlayer();
                    if (quit_request)
                    {
                        quit_request = false;
                        OnGameOver(this, null);
                        return; 
                    }
                    move = ActivePlayer.GetMove();

                    // kontrola tahu rozhodcim
                    isMoveValid = (ActivePlayer is ComputerPlayer) || Rules.IsMoveValid(move, ActivePlayer.Color);
                    if (!isMoveValid)
                        OnMoveInvalid(move);
                } while (!isMoveValid);

                // provedeni tahu deskou
                Board.ApplyMove(move);

                // zmenit hrace na tahu
                if (ActivePlayer.Color == GameColorsEnum.plrBlack)
                    ActivePlayer = Players.GetPlayerByColor(GameColorsEnum.plrWhite);
                else
                    ActivePlayer = Players.GetPlayerByColor(GameColorsEnum.plrBlack);
            }   
        }

        /// <summary>
        /// Zmenit nastaveni hracu podle retezce (pr. H0C1)
        /// <param name="newSettings"></param>
        public void SetPlayersFromString(string newSettings)
        {
            Players.SetFromString(newSettings);
            Rules.CheckPlayers(Players);
        }
    }
}
