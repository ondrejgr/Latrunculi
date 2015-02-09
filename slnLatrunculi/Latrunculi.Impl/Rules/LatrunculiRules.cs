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
            Pieces ownPiece;
            Pieces enemyPiece;
            if (move.TargetPiece == Pieces.pcBlack)
            {
                enemyPiece = Pieces.pcWhite;
                ownPiece = Pieces.pcBlack;
            }
            else if (move.TargetPiece == Pieces.pcWhite)
            {
                enemyPiece = Pieces.pcBlack;
                ownPiece = Pieces.pcWhite;
            }
            else 
                return;

            Coord c1 = move.Target;
            Coord? c2;
            Coord? c3;

            // sebrani obkliceneho soupere
            Action<CoordDirectionEnum> check = new Action<CoordDirectionEnum>((dir) =>
                {
                    c2 = Board.GetRelativeCoord(c1, dir);
                    if (c2.HasValue)
                    {
                        c3 = Board.GetRelativeCoord(c2.Value, dir);
                        if (c3.HasValue && Board[c2.Value] == enemyPiece && Board[c3.Value] == ownPiece)
                            move.RemovedPiecesCoords.Add(c2.Value);
                    }
                });

            check(CoordDirectionEnum.deForward);
            check(CoordDirectionEnum.deAft);
            check(CoordDirectionEnum.deLeft);
            check(CoordDirectionEnum.deRight);

            // sebrani soupere obliceneho v rohu
            Action<Coord,Coord,Coord> checkCorner = new Action<Coord,Coord,Coord>((corner, cc1, cc2) =>
                {
                    if (Board[corner] == enemyPiece)
                    {
                        if (move.Target.Equals(cc1) && (Board[cc2] == ownPiece))
                            move.RemovedPiecesCoords.Add(corner);
                        else if (move.Target.Equals(cc2) && (Board[cc1] == ownPiece))
                            move.RemovedPiecesCoords.Add(corner);
                    }
                });

            checkCorner(Coord.Parse("a1"), Coord.Parse("a2"), Coord.Parse("b1"));
            checkCorner(Coord.Parse("h1"), Coord.Parse("h2"), Coord.Parse("g1"));
            checkCorner(Coord.Parse("a7"), Coord.Parse("g7"), Coord.Parse("h8"));
            checkCorner(Coord.Parse("h7"), Coord.Parse("b7"), Coord.Parse("a6"));
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
