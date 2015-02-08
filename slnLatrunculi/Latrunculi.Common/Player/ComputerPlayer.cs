using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    public class ComputerPlayer : Player
    {
        public ComputerPlayer(GameColorsEnum color, Board board, int level = 1)
            : base(color)
        {
            if (board == null)
                throw new ArgumentNullException("board");
            Board = board;
            Level = level;
        }

        private Board Board;

        private int _level;
        /// <summary>
        /// Obtiznost PC hrace (0 = lehka,1,2 = tezka);
        /// </summary>
        public virtual int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }

        Random r = new Random();
        public override Move GetMove()
        {
            Moves moves = new Moves();
            
            Pieces tarPiece = (Color == GameColorsEnum.plrBlack) ? Pieces.pcBlack : Pieces.pcWhite;

            Coord src = new Coord();

            for (char x = 'A'; x <= Board.MaxX; x++)
            {
                for (byte y = 1; y <= Board.MaxY; y++)
                {
                    src.Set(x, y);
                    if (((Color == GameColorsEnum.plrBlack) &&
                        (Board[src] == Pieces.pcBlack || Board[src] == Pieces.pcBlackKing)) ||
                        ((Color == GameColorsEnum.plrWhite) &&
                        (Board[src] == Pieces.pcWhite || Board[src] == Pieces.pcWhiteKing)))
                    {
                        // barva figurky na danem miste patri hraci provadejicimu tah,
                        // muzeme overit mozne tahy v ortogonalich smerech.
                        // Pokud tam neni figurka a pokud to neni mimo desku, je tah dovoleny.
                        Action<CoordDirectionEnum> addMoveIfValid = new Action<CoordDirectionEnum>((dir) =>
                        {
                            Coord? tar;
                            tar = Board.GetRelativeCoord(src, dir);
                            if (tar.HasValue && (Board[tar.Value] == Pieces.pcNone))
                                moves.Add(new Move(src, tar.Value, Pieces.pcNone, tarPiece));
                        }
                        );

                        addMoveIfValid(CoordDirectionEnum.deForward);
                        addMoveIfValid(CoordDirectionEnum.deAft);
                        addMoveIfValid(CoordDirectionEnum.deLeft);
                        addMoveIfValid(CoordDirectionEnum.deRight);
                    }
                }
            }

            if (moves.Count == 0)
                throw new InvalidOperationException("Žádné volné tahy !");

            return moves[r.Next(0, moves.Count)];
        }

        public override string ToString()
        {
            return string.Format("C{0}", Level);
        }
    }
}