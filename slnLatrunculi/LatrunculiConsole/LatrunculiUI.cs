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

            Game.RenderBoardRequest += Game_RenderBoardRequest;
        }

        /// <summary>
        /// Spustit (UI loop)
        /// </summary>
        public void Run()
        {
            LoadGameLibrary();
                       
            Game.Run();
        }

        /// <summary>
        /// Vykresli hraci desku
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="Board"></param>
        void Game_RenderBoardRequest(IGame Sender, Board Board)
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

        public void Dispose()
        {
            if (Game != null)
                Game.RenderBoardRequest -= Game_RenderBoardRequest;

            Game = null;
            hlib = null;
        }
    }
}
