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
        public event PiecesRemovedEvent PiecesRemoved;

        public event BrainComputationStartedEvent BrainComputationStarted;
        public event BrainComputationFinishedEvent BrainComputationFinished;

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

        private bool _isComputing = false;
        public bool IsComputing
        {
            get
            {
                return _isComputing;
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

        protected virtual void OnPiecesRemoved(Move move)
        {
            if (PiecesRemoved != null)
            {
                Player piecesOwner;
                if (ActivePlayer == Players.Player1)
                    piecesOwner = Players.Player2;
                else
                    piecesOwner = Players.Player1;
                PiecesRemoved(ActivePlayer, piecesOwner, move, Board.GetNumberOfPieces(GameColorsEnum.plrWhite), Board.GetNumberOfPieces(GameColorsEnum.plrBlack));
            }
        }

        protected virtual void OnBrainComputationStarted()
        {
            if (BrainComputationStarted != null)
                BrainComputationStarted();
        }

        protected virtual void OnBrainComputationFinished(Move bestMove, string errorMessage, bool isCancelled)
        {
            if (BrainComputationFinished != null)
                BrainComputationFinished(bestMove, errorMessage, isCancelled);
        }
        
        /// <summary>
        /// Zmenit nastaveni hracu podle retezce (pr. H0C1)
        /// <param name="newSettings"></param>
        public void SetPlayersFromString(string newSettings)
        {
            Players.SetFromString(newSettings);
            Rules.CheckPlayers(Players);
        }

        /// <summary>
        /// Ukoncit hry a zobrazit viteze.
        /// </summary>
        public void EndGame()
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
            Rules.ClearNumOfMovesWithoutRemoval();

            // hrač na tahu - pokud není zadán, určí jej pravidla (začátek hry)
            if (activePlayerColor.HasValue)
                ActivePlayerColor = activePlayerColor.Value;
            else
                ActivePlayerColor = Rules.GetFirstActivePlayerColor(); 

            // pripravit novou hraci desku, postavit figurky
            Board.Init();

            OnRenderBoard();
            OnRenderActivePlayer();
        }

        CancellationTokenSource cts = null;
        public void CancelBrainComputation()
        {
            cts.Cancel();
        }

        /// <summary>
        /// Zjistit napovedu tahu.
        /// </summary>
        public Brain GetBrainInstance()
        {
            return new LatrunculiBrain(Board, Rules);
        }

        /// <summary>
        /// Smycka manazera hry
        /// </summary>
        /// <returns>False = konec hry</returns>
        public bool Proceed()
        {
            bool isMoveValid;
            Move move;
            cts = new CancellationTokenSource();
            try
            {
                // zjistit tah hrace
                Task<Move> getMoveTask = null;
                getMoveTask = Task.Run<Move>(new Func<Move>(() => { return ActivePlayer.GetMove(cts.Token); }), cts.Token);
                try
                {
                    _isComputing = true;
                    if (ActivePlayer is ComputerPlayer)
                        OnBrainComputationStarted();
                                        
                    getMoveTask.Wait();

                    _isComputing = false;
                    if (ActivePlayer is ComputerPlayer)
                        OnBrainComputationFinished(getMoveTask.Result, string.Empty, false);
                }
                catch (AggregateException exc)
                {
                    _isComputing = false;
                    if (ActivePlayer is ComputerPlayer)
                    {
                        if ((exc.InnerExceptions[0] is OperationCanceledException) || getMoveTask.IsCanceled)
                            OnBrainComputationFinished(null, "Operace byla přerušena uživatelem.", true);
                        else
                            OnBrainComputationFinished(null, string.Join(Environment.NewLine, exc.InnerExceptions.Select(e => e.Message)), false);
                        return true;
                    }
                    else
                        throw exc;
                }
                catch (Exception exc)
                {
                    _isComputing = false;
                    if (ActivePlayer is ComputerPlayer)
                    {
                        OnBrainComputationFinished(null, exc.Message, false);
                        return true;
                    }
                    else
                        throw exc;
                }

                move = getMoveTask.Result;

                isMoveValid = (ActivePlayer is ComputerPlayer && (move != null)) || Rules.IsMoveValid(move, ActivePlayer.Color);
                if (isMoveValid)
                {
                    // provedeni tahu deskou
                    Rules.SetPiecesToBeRemoved(move);
                    Board.ApplyMove(move);

                    OnRenderBoard();

                    if (move.RemovedPieces.Count > 0)
                    {
                        Rules.ClearNumOfMovesWithoutRemoval();
                        OnPiecesRemoved(move);
                    }
                    else
                        Rules.IncNumOfMovesWithoutRemoval();

                    // zkontrolovat prohru/vitezstvi
                    if (Rules.IsGameOver())
                    {
                        EndGame();
                        return false;
                    }

                    // zmenit hrace na tahu
                    ActivePlayerColor = Rules.GetNextPlayerColor(ActivePlayerColor);

                    OnRenderActivePlayer();
                }
                else
                    OnMoveInvalid(move);
            }
            finally
            {
                cts.Dispose();
                cts = null;
            }

            return true;
        }
    }
}
