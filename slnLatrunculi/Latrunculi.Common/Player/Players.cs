using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latrunculi
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

            Player NewPlayer1 = null;
            Player NewPlayer2 = null;

            int level;
            char type;

            type = Char.ToUpper(newSettings[0]);
            switch (type)
            {
                case 'H':
                    NewPlayer1 = new HumanPlayer(GameColorsEnum.plrWhite);
                    break;
                case 'C':
                    try
                    {
                        level = newSettings[1] - '0';
                        if (level < 0 || level > 2)
                            throw new ArgumentException();
                    }
                    catch
                    {
                        throw new ArgumentException("Neplatné nastavení obtížnosti bílého hráče: očekávám 0 - 2.");
                    }
                    NewPlayer1 = new ComputerPlayer(GameColorsEnum.plrWhite, _board, _rules, level);
                    break;
                default:
                    throw new ArgumentException("Neplatný znak v nastavení bílého hráče: očekávám H nebo C.");
            }

            type = Char.ToUpper(newSettings[2]);
            switch (type)
            {
                case 'H':
                    NewPlayer2 = new HumanPlayer(GameColorsEnum.plrBlack);
                    break;
                case 'C':
                    try
                    {
                        level = newSettings[3] - '0';
                        if (level < 0 || level > 2)
                            throw new ArgumentException();
                    }
                    catch
                    {
                        throw new ArgumentException("Neplatné nastavení obtížnosti bílého hráče: očekávám 0 - 2.");
                    }
                    NewPlayer2 = new ComputerPlayer(GameColorsEnum.plrBlack, _board, _rules, level);
                    break;
                default:
                    throw new ArgumentException("Neplatný znak v nastavení černého hráče: očekávám H nebo C.", "newSettings");
            }

            // nedošlo k výjimce, můžeme změnit
            Player1 = NewPlayer1;
            Player2 = NewPlayer2;
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
        public Players(Board board, Rules rules)
        {
            if (board == null)
                throw new ArgumentNullException("board");
            if (rules == null)
                throw new ArgumentNullException("rules");

            _board = board;
            _rules = rules;
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
