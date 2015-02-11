using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi.Common
{
    public class Players
    {
        public bool ArePlayersAssigned
        {
            get
            {
                return Player1 != null && Player2 != null;
            }
        }

        private Board _board;
        private Rules _rules;
        private Type _brainType;

        private Player _player1;
        public Player Player1
        {
            get
            {
                return _player1;
            }
            private set
            {
                _player1 = value;
            }
        }

        private Player _player2;
        public Player Player2
        {
            get
            {
                return _player2;
            }
            private set
            {
                _player2 = value;
            }
        }

        /// <summary>
        /// Nastavit nastaveni hracu z retezce
        /// </summary>
        /// <param name="newSettings"></param>
        public void SetFromString(string newSettings)
        {
            if (string.IsNullOrWhiteSpace(newSettings))
                throw new ArgumentException("Neplatné nastavení hráčů.");

            if (newSettings.Length != 4)
                throw new ArgumentException("Neplatné nastavení hráčů.");

            Player[] NewPlayers = new Player[2];

            int level;
            char type;

            Action<int, int> ParseString = new Action<int, int>((offset, player_index) =>
                {
                    type = Char.ToUpper(newSettings[0 + offset]);
                    try
                    {
                        level = newSettings[1 + offset] - '0';
                        if (level < 0 || level > 2)
                            throw new ArgumentException();
                    }
                    catch
                    {
                        throw new ArgumentException(string.Format("Neplatné nastavení obtížnosti {0}. hráče: očekávám 0 - 2.", player_index + 1));
                    }

                    switch (type)
                    {
                        case 'H':
                            NewPlayers[player_index] = new HumanPlayer((GameColorsEnum)player_index, level);
                            break;
                        case 'C':
                            NewPlayers[player_index] = new ComputerPlayer((GameColorsEnum)player_index, Activator.CreateInstance(_brainType, _board, _rules) as Brain, level);
                            break;
                        default:
                            throw new ArgumentException(string.Format("Neplatný znak v nastavení {0}. hráče: očekávám H nebo C.", player_index + 1));
                    }
                });

            ParseString(0, 0);
            ParseString(2, 1); 

            // nedošlo k výjimce, můžeme změnit
            Player1 = NewPlayers[0];
            Player2 = NewPlayers[1];
        }

        public Player GetPlayerByColor(GameColorsEnum color)
        {
            if (Player1 != null && Player1.Color == color)
                return Player1;
            else if (Player2 != null && Player2.Color == color)
                return Player2;
            else
                throw new UnableToDeterminePlayerByColor();
        }

        /// <summary>
        /// Vytvarit instanci nastaveni hracu.
        /// </summary>
        /// <param name="board">Kvuli PC hraci a jeho mozku.</param>
        public Players(Board board, Rules rules, Type brainType)
        {
            if (board == null)
                throw new ArgumentNullException("board");
            if (rules == null)
                throw new ArgumentNullException("rules");
            if (brainType == null)
                throw new ArgumentNullException("brainType");
            if (!brainType.IsSubclassOf(typeof(Brain)))
                throw new ArgumentException("Byl předán nesprávný typ.", "brainType");
            
            _board = board;
            _rules = rules;
            _brainType = brainType;
        }

        public override string ToString()
        {
            string p1 = "X0";
            string p2 = "X0";

            if (Player1 != null)
                p1 = Player1.ToString();
            if (Player2 != null)
                p2 = Player2.ToString();

            return string.Concat(p1, p2);
        }
    }
}
