using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
{
    /// <summary>
    /// Abstraktni hrac
    /// </summary>
    public abstract class Player
    {
        public Player(PlayersEnum color)
        {
            _color = color;
        }

        public string Name
        {
            get
            {
                switch (Color)
                {
                    case PlayersEnum.plrBlack:
                        return "černý";
                    default:
                        return "bílý";
                }
            }
        }

        private readonly PlayersEnum _color;
        /// <summary>
        /// Player color
        /// </summary>
        public PlayersEnum Color
        {
            get
            {
                return _color;
            }
        }

        /// <summary>
        /// Zjistit tah hrace.
        /// </summary>
        /// <returns></returns>
        public virtual Move GetMove()
        {
            throw new NotImplementedException();
        }
    }
}
