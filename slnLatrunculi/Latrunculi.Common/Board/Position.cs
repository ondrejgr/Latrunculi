using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    /// <summary>
    /// Pozice hry
    /// </summary>
    public class Position
    {
        public Position(GameColorsEnum activePlayerColor, Board board)
        {
            if (board == null)
                throw new ArgumentNullException("board");

            ActivePlayerColor = activePlayerColor;
            Board = board.GetBoardCopy();
        }
        
        public GameColorsEnum EnemyPlayerColor
        {
            get
            {
                if (ActivePlayerColor == GameColorsEnum.plrBlack)
                    return GameColorsEnum.plrWhite;
                else
                    return GameColorsEnum.plrBlack;
            }
        }

        public int EnemyNumOfPieces
        {
            get
            {
                return Board.GetNumberOfPieces(EnemyPlayerColor);
            }
        }

        public int MyNumOfPieces
        {
            get
            {
                return Board.GetNumberOfPieces(ActivePlayerColor);
            }
        }

        public Board Board
        {
            get;
            private set;
        }

        public GameColorsEnum ActivePlayerColor
        {
            get;
            private set;
        }

        public bool IsEqual
        {
            get
            {
                return MyNumOfPieces == EnemyNumOfPieces;
            }
        }

        public bool IsWinning
        {
            get
            {
                return EnemyNumOfPieces == 0;
            }
        }

        public bool IsLosing
        {
            get
            {
                return MyNumOfPieces == 0;
            }
        }
    }
}
 