using Latrunculi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public event HumanMoveRequestedEvent HumanMoveRequested;

        public Game()
        {
            _board = new LatrunculiBoard();
            _rules = new LatrunculiRules(_board);
            _players = new Players(_board, _rules, typeof(LatrunculiBrain));
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
        public void Run(string playersSetting, GameColorsEnum? activePlayerColor = null)
        {
            // nastavit hrace
            Players.SetFromString(playersSetting);
            Rules.CheckPlayers(Players);

            // hrač na tahu - pokud není zadán, určí jej pravidla (začátek hry)
            if (activePlayerColor.HasValue)
                ActivePlayerColor = activePlayerColor.Value;
            else
                ActivePlayerColor = Rules.GetFirstActivePlayerColor(); 

            // pripravit novou hraci desku, postavit figurky
            Board.Init();
        }

        /// <summary>
        /// Smycka manazera hry
        /// </summary>
        private void GameLoop()
        {
            OnRenderBoard();
            OnRenderActivePlayer();

            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                Task<Move> getMoveTask = Task.Run<Move>(new Func<Move>(() => { return ActivePlayer.GetMove(cts.Token); }), cts.Token);
            }
        }
    }
}
