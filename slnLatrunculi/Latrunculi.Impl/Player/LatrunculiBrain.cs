using Latrunculi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Latrunculi.Impl
{
    public class LatrunculiBrain: Brain
    {
        public LatrunculiBrain(Board board, Rules rules)
            : base(board, rules)
        {
        }

        private const int MAX = 99;
        private const int MNOHO = 90;
        Random rnd = new Random();

        protected override void OnComputeMove(int level, GameColorsEnum color, CancellationToken ct)
        {
            Position cur = new Position(color, Board);

            BestMove = getBestMove(cur, 3, ct);
        }

        static private int get_max(int ohod1, int ohod2)
        {
            if (ohod1 >= ohod2)
                return ohod1;
            else
                return ohod2;
        }

        private int minimax(Position pos, int depth, CancellationToken ct)
        {
            if (pos == null)
                throw new ArgumentNullException("pos");

            if (pos.IsWinning)
                return MAX;
            if (pos.IsLosing)
                return MAX;

            if (depth == 0)
                return eval(pos);
            else
            {
                int result = -MAX;
                Rules r = new LatrunculiRules(pos.Board);
                Moves moves = r.GetValidMoves(pos.ActivePlayerColor);
                Position child;

                foreach(Move move in moves)
                {
                    ct.ThrowIfCancellationRequested();

                    r.SetPiecesToBeRemoved(move);
                    pos.Board.ApplyMove(move);
                    child = new Position(pos.EnemyPlayerColor, pos.Board);
                    pos.Board.ApplyInvMove(move);

                    result = get_max(result, -minimax(child, depth - 1, ct));
                }

                if (result > MNOHO)
                    result = result - 1;
                if (result < -MNOHO)
                    result = result + 1;

                return result;                   
            }
        }

        private Move getBestMove(Position pos, int depth, CancellationToken ct)
        {
            Rules r = new LatrunculiRules(pos.Board);
            int best_ohod = -MAX;
            int ohod;

            Moves moves = r.GetValidMoves(pos.ActivePlayerColor);
            Move best_move = null;
            Position child;
            
            foreach(Move move in moves)
            {
                ct.ThrowIfCancellationRequested();

                r.SetPiecesToBeRemoved(move);
                pos.Board.ApplyMove(move);
                child = new Position(pos.EnemyPlayerColor, pos.Board);
                pos.Board.ApplyInvMove(move);

                ohod = -minimax(child, depth - 1, ct);
                if (ohod > best_ohod)
                {
                    best_ohod = ohod;
                    best_move = move;
                }
            }

            return best_move;
        }

        private int eval(Position pos)
        {
            if (pos == null)
                throw new ArgumentNullException("pos");

            int result = 0;

            Pieces own = (pos.ActivePlayerColor == GameColorsEnum.plrWhite) ? Pieces.pcWhite : Pieces.pcBlack;
            Pieces enem = (pos.ActivePlayerColor == GameColorsEnum.plrBlack) ? Pieces.pcBlack : Pieces.pcWhite;

            Coord c = new Coord();
            Coord c2 = new Coord();
            for (char x = 'A'; x <= 'H'; x++ )
            {
                c.Set(x, 3);
                if (pos.Board[c] == own)
                    result = result + 1 + (x % 2) * 2;

                c.Set(x, 4);
                if (pos.Board[c] == own)
                    result = result + 2 + (x % 2) * 2;

                c.Set(x, 5);
                if (pos.Board[c] == own)
                    result = result + 3 + (x % 2) * 2;
            }
            for (char x = 'A'; x <= 'H'; x++)
            {
                c.Set(x, 3);
                if (pos.Board[c] == enem)
                    result = result - 1 - (x % 2) * 2;

                c.Set(x, 4);
                if (pos.Board[c] == enem)
                    result = result - 2 - (x % 2) * 2;

                c.Set(x, 5);
                if (pos.Board[c] == enem)
                    result = result - 3 - (x % 2) * 2;
            }

            if (pos.ActivePlayerColor == GameColorsEnum.plrWhite)
            {
                for (char x = 'A'; x <= 'H'; x++)
                {
                    c.Set(x, 1);
                    c2.Set(x, 2);
                    if (pos.Board[c] == own && pos.Board[c2] == Pieces.pcNone)
                        result = result + 5;
                }
            }
            else
            {
                for (char x = 'A'; x <= 'H'; x++)
                {
                    c.Set(x, 7);
                    c2.Set(x, 6);
                    if (pos.Board[c] == own && pos.Board[c2] == Pieces.pcNone)
                        result = result + 5;
                }
            }

            if (pos.MyNumOfPieces - pos.EnemyNumOfPieces > 1)
                result += 10;
            if (pos.MyNumOfPieces - pos.EnemyNumOfPieces > 2)
                result += 5;
            if (pos.MyNumOfPieces - pos.EnemyNumOfPieces > 4)
                result += 5;
            if (pos.MyNumOfPieces - pos.EnemyNumOfPieces > 8)
                result += 5;
            if (pos.MyNumOfPieces - pos.EnemyNumOfPieces > 10)
                result += 5;
            if (pos.MyNumOfPieces - pos.EnemyNumOfPieces > 12)
                result += 10;

            if (pos.ActivePlayerColor != GameColorsEnum.plrWhite)
            {
                result = -result;
                result -= rnd.Next(0, 5);
            }
            else
                result += rnd.Next(0, 5);

            return result;
        }
    }
}
