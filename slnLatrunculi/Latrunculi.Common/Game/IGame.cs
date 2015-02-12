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
        event PiecesRemovedEvent PiecesRemoved;

        event BrainComputationStartedEvent BrainComputationStarted;
        event BrainComputationFinishedEvent BrainComputationFinished;

        Board Board
        {
            get;
        }

        Player ActivePlayer
        {
            get;
        }

        string CurrentPlayersSetting
        {
            get;
        }

        bool IsComputing
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

        void Run(string playersSetting, GameColorsEnum? activePlayerColor = null);
        void EndGame();
        bool Proceed();

        void SetPlayersFromString(string newSettings);
        void CancelBrainComputation();
    }
}
