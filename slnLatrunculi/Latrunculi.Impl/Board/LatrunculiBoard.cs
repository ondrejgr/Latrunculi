using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi.Impl
{
    /// <summary>
    /// Deska pro Latrunculi
    /// </summary>
    public class LatrunculiBoard: Board
    {
        public override char MaxX
        {
            get
            {
                return 'H';
            }
        }

        public override byte MaxY
        {
            get
            {
                return 7;
            }
        }

        /// <summary>
        /// inicializace desky = výchozí rozestavění figurek podle pravidel
        /// </summary>
        protected override void OnInit()
        {
            Coord c = new Coord();
            Pieces p;

            // bily
            p = Pieces.pcWhite;
            for (byte y = 1; y <= 2; y++)
                for (char x = 'A'; x <= MaxX; x++)
                {
                    c.Set(x, y);
                    this[c] = p;
                }

            // cerny
            p = Pieces.pcBlack;
            for (byte y = 6; y <= 7; y++)
                for (char x = 'A'; x <= MaxX; x++)
                {
                    c.Set(x, y);
                    this[c] = p;
                }
        }
    }
}
