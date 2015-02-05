using Latrunculi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace LatrunculiConsole
{
    /// <summary>
    /// Uzivatelske rozhrani Console - vlastni program.
    /// </summary>
    internal class LatrunculiUI: IDisposable
    {
        /// <summary>
        /// konzolova reprezentace kamenu.
        /// </summary>
        static private readonly Dictionary<Pieces, char> PcToC = new Dictionary<Pieces, char>() 
            { 
                { Pieces.pcNone, ' '},
                { Pieces.pcWhite, 'o'},
                { Pieces.pcBlack, 'x'},
                { Pieces.pcWhiteKing, 'W'},
                { Pieces.pcBlackKing, 'B'}
            };

        /// <summary>
        /// Jmeno knihovny s herni logikou.
        /// </summary>
        private string FileName = "Latrunculi.Impl";

        private bool GameLoaded = false;
        
        /// <summary>
        /// Handle na knihovnu
        /// </summary>
        private ObjectHandle hlib;
        private IGame Game;
        
        /// <summary>
        /// Nacteni herni logiky
        /// </summary>
        private void LoadGameLibrary()
        {
            if (GameLoaded)
                throw new InvalidOperationException("Neplatná operace, knihovna hry byla již nahrána.");

            hlib = Activator.CreateInstance(FileName, "Latrunculi.Impl.Game");
            if (hlib == null)
                throw new Exception(string.Format("Nepodařilo se vytvořit instanci logiky hry ze souboru {0}.", FileName));

            Game = hlib.Unwrap() as IGame;
            if (Game == null)
                throw new Exception(string.Format("Nepodařilo se získat instanci logiky hry ze souboru {0}.", FileName));

            Console.WriteLine("Rozhraní hry {0} {1} bylo načteno.", Game.Title, Game.Version);

            Game.RenderBoard += Game_RenderBoard;
            Game.RenderActivePlayer += Game_RenderActivePlayer;
        }

        private void SetupPlayers(Players players)
        {
            Console.WriteLine("Aktuální nastavení hráčů: {0}.", players);
            Console.WriteLine("Zadejte nové nastavení hráčů v pořadí bílý, černý");
            Console.WriteLine("  C0 - C2 = počítač (0-lehký), H0 = lidský hráč, Enter = žádná změna...");

            string newSettings = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newSettings))
                Console.WriteLine("Nastavení hráčů nebylo změněno.");
            else
            {
                try
                {
                    players.ParseFromString(newSettings);
                    Console.WriteLine("Nové nastavení hráčů: {0}.", players);
                }
                catch (Exception exc)
                {
                    Console.WriteLine("CHYBA ! Změna nebyla provedena: {0}", exc.Message);
                    SetupPlayers(players); // zkus znovu
                }
            }
        }

        /// <summary>
        /// Spustit (UI loop)
        /// </summary>
        public void Run()
        {
            LoadGameLibrary();

            Players players = new Players();
            SetupPlayers(players);
            Game.Run(players, null);
        }

        /// <summary>
        /// Vykresli hraci desku
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="Board"></param>
        void Game_RenderBoard(IGame Sender, Board Board)
        {
            Coord c = new Coord();

            Console.WriteLine();
            // prochazet radky
            for (byte j = 7; j >= 1; j-- )
            {
                // horizont. oddelovac
                Console.Write("    ");
                for (char k = 'A'; k <= Board.MaxX; k++ )
                    Console.Write("{0}", "--");
                Console.WriteLine();

                // vypsani kamenu v radku
                Console.Write("  {0}|", j);
                for (char i = 'A'; i <= Board.MaxX; i++ )
                {
                    c.Set(i, j);
                    Console.Write("{0}|", PcToC[Board[c]]);
                }
                Console.WriteLine();

            }
            // horizont. oddelovac
            Console.Write("    ");
            for (char k = 'A'; k <= Board.MaxX; k++)
                Console.Write("{0}", "--");
            Console.WriteLine();

            // popisky x
            Console.Write("    ");
            for (char i = 'A'; i <= Board.MaxX; i++)
            {
                Console.Write("{0} ", i);
            }
            Console.WriteLine();
        }

        void Game_RenderActivePlayer(IGame Sender, Player Player)
        {
            if (Player != null)
            {
                string typ = (Player is HumanPlayer) ? "lidský hráč" : "počítač";
                Console.WriteLine("Aktuální hráč na tahu: {0} ({1}).", Player.Name, typ);
            }
        }

        public void Dispose()
        {
            if (Game != null)
            {
                Game.RenderBoard -= Game_RenderBoard;
                Game.RenderActivePlayer -= Game_RenderActivePlayer;
            }

            Game = null;
            hlib = null;
        }
    }
}
