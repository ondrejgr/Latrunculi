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
        /// Nacteni herni logiky
        /// </summary>
        private IGame GetNewGameInstance()
        {
            if (GameLoaded)
                throw new InvalidOperationException("Neplatná operace, knihovna hry byla již nahrána.");

            ObjectHandle hlib;
            hlib = Activator.CreateInstance(FileName, "Latrunculi.Impl.Game");
            if (hlib == null)
                throw new Exception(string.Format("Nepodařilo se vytvořit instanci logiky hry ze souboru {0}.", FileName));

            IGame game = hlib.Unwrap() as IGame;
            if (game == null)
                throw new Exception(string.Format("Nepodařilo se získat instanci logiky hry ze souboru {0}.", FileName));

            return game;
        }

        private void HookGameEvents(IGame game)
        {
            if (game == null)
                throw new ArgumentNullException("game");

            game.RenderBoard += Game_RenderBoard;
            game.RenderActivePlayer += Game_RenderActivePlayer;
            game.MoveInvalid += Game_MoveInvalid;
            game.GameOver += Game_GameOver;
        }

        private void UnhookGameEvents(IGame game)
        {
            if (game != null)
            {
                game.RenderBoard -= Game_RenderBoard;
                game.RenderActivePlayer -= Game_RenderActivePlayer;
                game.MoveInvalid -= Game_MoveInvalid;
                game.GameOver -= Game_GameOver;
            }
        }

        /// <summary>
        /// Vykresli hraci desku
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="Board"></param>
        void Game_RenderBoard(IGame Sender)
        {
            if (Sender == null)
                throw new ArgumentNullException("Sender");
            if (Sender.Board == null)
                throw new ArgumentException("Objekt hrací deska není známý.");

            Board Board = Sender.Board;
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
            if (Sender == null)
                throw new ArgumentNullException("Sender");
            if (Player == null)
                throw new ArgumentNullException("Player");

            // vypis aktualniho hrace
            bool isHuman = (Player is HumanPlayer);
            string typ = isHuman ? "lidský hráč" : "počítač";
            Console.WriteLine();
            Console.WriteLine("Aktuální hráč na tahu: {0} ({1}).", Player.Name, typ);
        }

        void Game_MoveInvalid(IGame Sender, Player Player, Move Move)
        {
            Console.WriteLine("Zadaných tah {0} není podle pravidel platný !", Move);
        }

        void Game_GameOver(IGame Sender, Player Winner)
        {
            if (Winner == null)
                Console.WriteLine("Konec hry. Remíza.");
            else
            {
                string typ = (Winner is HumanPlayer) ? "lidský hráč" : "počítač";
                Console.WriteLine("Konec hry. Vítězem je: {0} ({1}).", Winner.Name, typ);
            }
        }

        private string GetPlayersSetting(string current)
        {
            Console.WriteLine("Aktuální nastavení hráčů: {0}.", current);
            Console.WriteLine("Zadejte nové nastavení hráčů v pořadí bílý, černý");
            Console.WriteLine("  C0 - C2 = počítač (0-lehký), H0 = lidský hráč, Enter = žádná změna...");

            string newSettings = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newSettings))
            {
                Console.WriteLine("Nastavení hráčů nebylo změněno.");
                return current;
            }
            else
                return newSettings;
        }

        /// <summary>
        /// Spustit (UI loop)
        /// </summary>
        public void RunUI()
        {
            IGame game = GetNewGameInstance();
            Console.WriteLine("Instance hry {0} {1} byla vytvořena.", game.Title, game.Version);
            try
            {
                HookGameEvents(game);

                string playersSetting = GetPlayersSetting("H1C1");
                Console.WriteLine("Spouštím hru.");

                Task gameTask = Task.Run(new Action(() => game.Run(playersSetting)));
                gameTask.Wait();
            }
            catch (AggregateException exc)
            {
                Console.WriteLine("CHYBA ! Při běhu hry došlo k chybám !");
                foreach (Exception ex in exc.InnerExceptions)
                {
                    Console.WriteLine("  {0}", ex.Message);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("CHYBA ! Při běhu hry došlo k výjimce: {0}", exc.Message);
            }
            finally
            {
                UnhookGameEvents(game);
            }
        }

        public void Dispose()
        {

        }
    }
}
