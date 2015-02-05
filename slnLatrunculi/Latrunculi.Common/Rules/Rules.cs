using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    public abstract class Rules
    {
        /// <summary>
        /// Zjistit, kdo je první na tahu (při nové hře).
        /// </summary>
        /// <returns></returns>
        public virtual PlayersEnum GetFirstActivePlayerColor()
        {
            return PlayersEnum.plrWhite;
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
    }
}
