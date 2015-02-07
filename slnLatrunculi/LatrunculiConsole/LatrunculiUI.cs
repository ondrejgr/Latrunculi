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
            Game.MoveInvalid += Game_MoveInvalid;
            Game.GameOver += Game_GameOver;
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
            Console.WriteLine("Aktuální hráč na tahu: {0} ({1}).", Player.Name, typ);

            // zjistit tah hrace nebo umoznit ovladani programu pri tahu PC
            string prompt;
            if (isHuman)
            {
                ((HumanPlayer)Player).HumanMove = null;
                prompt = "Zadejte svůj tah (př. A2A3) nebo řídicí příkaz (? pro nápovědu)...";
            }
            else
                prompt = "Zadejte řídicí příkaz (? pro nápovědu) nebo Enter pro provedení tahu počítačem...";

            string str;
            Console.Write(prompt);
            str = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(str))
            {
                if (isHuman)
                {
                    Console.WriteLine("CHYBA: Byl zadán prázdný řetězec.");
                    Game_RenderActivePlayer(Sender, Player);
                }
            }
            else
            {
                char command = Char.ToUpper(str[0]);
                switch (command)
                {
                    case '?':
                        Console.WriteLine("Příkazy, které je možné zadat:");
                        Console.WriteLine("    T = napoveda nejlepsiho tahu");
                        Console.WriteLine("    R = znovu vykreslit hraci desku");
                        Console.WriteLine("    S = změnit nastavení hráčů");
                        Console.WriteLine("    X = ukoncit hru");
                        Game_RenderActivePlayer(Sender, Player);
                        break;
                    case 'T':
                        try
                        {
                            throw new NotImplementedException();
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine("CHYBA: Nejlepší tah nelze napovědět ! {0}", exc.Message);
                        }
                        Game_RenderActivePlayer(Sender, Player);
                        break;
                    case 'S':
                        string oldSettings = Game.CurrentPlayersSetting;
                        try
                        {
                            string newSettings = GetPlayersSetting(oldSettings);
                            if (oldSettings != newSettings)
                                Game.SetPlayersFromString(newSettings);
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine("CHYBA: Nastavení hráčů nebylo změněno: {0}", exc.Message);
                            Game.SetPlayersFromString(oldSettings);
                        }
                        Game_RenderActivePlayer(Sender, Player);
                        break;
                    case 'R':
                        try
                        {
                            Game_RenderBoard(Sender);
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine("CHYBA: Desku se nepodařilo překreslit ! {0}", exc.Message);
                        }
                        Game_RenderActivePlayer(Sender, Player);
                        break;
                    case 'X':
                        Game.RequestQuit();
                        break;
                    default:
                        // jinak předpokládáme, že nejde o příkaz, ale byl zadán tah
                        if (isHuman)
                        {
                            try
                            {
                                ((HumanPlayer)Player).HumanMove = Move.Parse(str, (Player.Color == GameColorsEnum.plrBlack) ? Pieces.pcBlack : Pieces.pcWhite);
                            }
                            catch (Exception exc)
                            {
                                Console.WriteLine("Chyba zadání: {0}", exc.Message);
                            }
                        }
                        break;
                }
            }
        }

        void Game_MoveInvalid(IGame Sender, Player Player, Move Move)
        {
            Console.WriteLine("Zadaných tah {0} není podle pravidel platný !", Move);
            // opakuj zadani tahu
            Game_RenderActivePlayer(Sender, Player);
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

        /// <summary>
        /// Spustit (UI loop)
        /// </summary>
        public void RunUI()
        {
            LoadGameLibrary();

            string playerSettings = GetPlayersSetting("H0C1");
            Console.WriteLine("Spouštím hru.");
            Game.Run(playerSettings);
        }

        public void Dispose()
        {
            if (Game != null)
            {
                Game.RenderBoard -= Game_RenderBoard;
                Game.RenderActivePlayer -= Game_RenderActivePlayer;
                Game.MoveInvalid -= Game_MoveInvalid;
                Game.GameOver -= Game_GameOver;
            }

            Game = null;
            hlib = null;
        }
    }
}
