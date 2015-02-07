using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    public abstract class Rules
    {
        protected Board Board;

        public Rules(Board board)
        {
            if (board == null)
                throw new ArgumentNullException("board");

            Board = board;
        }

        /// <summary>
        /// Zjistit, kdo je první na tahu (při nové hře).
        /// </summary>
        /// <returns></returns>
        public virtual GameColorsEnum GetFirstActivePlayerColor()
        {
            return GameColorsEnum.plrWhite;
        }

        /// <summary>
        /// Ověřit správnost nastavení hráčů.
        /// </summary>
        /// <param name="players"></param>
        public void CheckPlayers(Players players)
        {
            if (players == null)
                throw new ArgumentNullException("players");

            if (!players.ArePlayersAssigned)
                throw new ArgumentException("Některý z hráčů není připraven.", "players");

            if (players.Player1.Color == players.Player2.Color)
                throw new ArgumentException("Hráči nesmějí mít stejné barvy.", "players");
        }

        /// <summary>
        /// Kontrola tahu podle pravidel.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool IsMoveValid(Move move, GameColorsEnum color)
        {
            if (move == null)
                throw new ArgumentNullException("move");

            return OnIsMoveValid(move, color); 
        }

        /// <summary>
        /// Metoda, kterou je nutno overridovat vlastní implementací kontroly platnosti tahu.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="activeColor"></param>
        protected virtual bool OnIsMoveValid(Move move, GameColorsEnum color)
        {
            throw new NotImplementedException();
        }
    }
}
