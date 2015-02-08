using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi.Impl
{
    public class LatrunculiRules: Rules
    {
        public LatrunculiRules(Board board): base(board)
        {
            
        }

        protected override void OnGetValidMoves(Moves moves, GameColorsEnum color)
        {
            Pieces tarPiece = (color == GameColorsEnum.plrBlack) ? Pieces.pcBlack : Pieces.pcWhite;

            Coord src = new Coord();

            for (char x = 'A'; x <= Board.MaxX; x++)
            {
                for (byte y = 1; y <= Board.MaxY; y++)
                {
                    src.Set(x, y);
                    if (((color == GameColorsEnum.plrBlack) &&
                        (Board[src] == Pieces.pcBlack || Board[src] == Pieces.pcBlackKing)) ||
                        ((color == GameColorsEnum.plrWhite) &&
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
        }

        protected override void OnSetPiecesToBeRemoved(Move move)
        {
            // zajeti v rozich
            GameColorsEnum ownColor;
            GameColorsEnum enemyColor;
            Pieces enemyPiece;
            if (move.TargetPiece == Pieces.pcBlack)
            {
                ownColor = GameColorsEnum.plrBlack;
                enemyPiece = Pieces.pcWhite;
            }
            else if (move.TargetPiece == Pieces.pcWhite)
            {
                ownColor = GameColorsEnum.plrWhite;
                enemyPiece = Pieces.pcBlack;
            }
            else 
                return;

            enemyColor = (ownColor == GameColorsEnum.plrWhite) ? GameColorsEnum.plrBlack : GameColorsEnum.plrWhite;

            // TODO: impl determine pieces to be removed
            throw new NotImplementedException();
        }

        /// <summary>
        /// Zjisti viteze
        /// </summary>
        /// <returns>null = remiza</returns>
        public override GameColorsEnum? GetWinner()
        {
            int white = Board.GetNumberOfPieces(GameColorsEnum.plrWhite);
            int black = Board.GetNumberOfPieces(GameColorsEnum.plrBlack);

            if (white > black)
                return GameColorsEnum.plrWhite;
            else if (black > white)
                return GameColorsEnum.plrBlack;
            else
                return null;
        }
    }
}
