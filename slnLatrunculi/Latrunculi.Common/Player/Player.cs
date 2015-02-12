using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Latrunculi
{
    /// <summary>
    /// Abstraktni hrac
    /// </summary>
    public abstract class Player
    {
        public Player(GameColorsEnum color, int level = 1)
        {
            _color = color;
            _level = level;
        }

        private int _level;
        /// <summary>
        /// Obtiznost PC hrace (0 = lehka,1,2 = tezka)
        /// Nastaveni pro napovedu tahu
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
                
        public string Name
        {
            get
            {
                switch (Color)
                {
                    case GameColorsEnum.plrBlack:
                        return "černý";
                    default:
                        return "bílý";
                }
            }
        }

        private readonly GameColorsEnum _color;
        /// <summary>
        /// Player color
        /// </summary>
        public GameColorsEnum Color
        {
            get
            {
                return _color;
            }
        }

        /// <summary>
        /// Vratit tah hrace.
        /// </summary>
        /// <returns></returns>
        public virtual Move GetMove(System.Threading.CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
