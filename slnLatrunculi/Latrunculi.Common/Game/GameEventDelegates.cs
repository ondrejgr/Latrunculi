﻿using Latrunculi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Latrunculi
{
    public delegate void RenderBoardEvent(IGame Sender);
    public delegate void RenderActivePlayerEvent(IGame Sender, Player Player);
    public delegate void MoveInvalidEvent(IGame Sender, Player Player, Move Move);
    public delegate void PiecesRemovedEvent(Player piecesTaker, Player piecesOwner, Move move, int numPiecesWhite, int numPiecesBlack);
    public delegate void GameOverEvent(IGame Sender, Player Winner);
}
