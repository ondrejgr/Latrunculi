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
            _players = new Players(_board, _rules);
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

        private GameColorsEnum _activePlayerColor;
        private GameColorsEnum ActivePlayerColor
        {
            get
            {
                return _activePlayerColor;
            }
            set 
            {
                _activePlayerColor = value;
            }
        }

        public Player ActivePlayer
        {
            get
            {
                return Players.GetPlayerByColor(ActivePlayerColor);
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

        private bool quit_requested = false;
        public void RequestQuit()
        {
            quit_requested = true;
        }

        private bool control_loop_reset_requested = false;
        public void RequestControlLoopReset()
        {
            control_loop_reset_requested = true;
        }

        /// <summary>
        /// Vyvolat vyjimku pokud je pozadovano
        /// </summary>
        private void AssertControlLoopCommands()
        {
            if (control_loop_reset_requested)
            {
                control_loop_reset_requested = false;
                throw new ControlLoopResetRequestedException();
            }
            if (quit_requested)
            {
                quit_requested = false;
                throw new ControlLoopQuitException();
            }
        }

        /// <summary>
        /// Ukoncit hry a zobrazit viteze.
        /// </summary>
        private void EndGame()
        {
            Player winner = null;
            GameColorsEnum? winnerColor = Rules.GetWinner();

            if (winnerColor.HasValue)
                winner = Players.GetPlayerByColor(winnerColor.Value);

            OnGameOver(this, winner);
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
            ActivePlayerColor = Rules.GetFirstActivePlayerColor(); 

            Board.Init();

            // smycka Manazera
            try
            {
                while (true)
                {
                    // vykresleni
                    OnRenderBoard();

                    // zjisti tah
                    Move move = null;
                    bool isMoveValid;
                    do
                    {
                        do
                        {
                            try
                            {
                                OnRenderActivePlayer();
                                AssertControlLoopCommands();

                                move = ActivePlayer.GetMove();
                                AssertControlLoopCommands();
                                break;
                            }
                            catch (ControlLoopResetRequestedException)
                            {
                            }
                        } while (true);

                        // kontrola tahu rozhodcim
                        isMoveValid = (ActivePlayer is ComputerPlayer) || Rules.IsMoveValid(move, ActivePlayer.Color);
                        if (!isMoveValid)
                            OnMoveInvalid(move);
                    } while (!isMoveValid);

                    // provedeni tahu deskou
                    Board.ApplyMove(move);

                    // zmenit hrace na tahu
                    if (ActivePlayerColor == GameColorsEnum.plrBlack)
                        ActivePlayerColor = GameColorsEnum.plrWhite;
                    else
                        ActivePlayerColor = GameColorsEnum.plrBlack;
                }
            }
            catch (ControlLoopQuitException)
            {
                // ukoncit hru
                EndGame();
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
