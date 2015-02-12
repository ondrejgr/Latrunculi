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
        private IGame Game;
        private bool ShowMenuForComputerPlayer = false;
        
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
            game.BrainComputationStarted += Game_BrainComputationStarted;
            game.BrainComputationFinished += Game_BrainComputationFinished;
            game.PiecesRemoved += Game_PiecesRemoved;
        }

        private void UnhookGameEvents(IGame game)
        {
            if (game != null)
            {
                game.RenderBoard -= Game_RenderBoard;
                game.RenderActivePlayer -= Game_RenderActivePlayer;
                game.MoveInvalid -= Game_MoveInvalid;
                game.GameOver -= Game_GameOver;
                game.BrainComputationStarted -= Game_BrainComputationStarted;
                game.BrainComputationFinished -= Game_BrainComputationFinished;
                game.PiecesRemoved -= Game_PiecesRemoved;
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

        private void Game_PiecesRemoved(Player piecesTaker, Player piecesOwner, Move move, int numPiecesWhite, int numPiecesBlack)
        {
            if (move.RemovedPieces.Count > 1)
                Console.WriteLine(" Kameny zajaty. Nový počet kamenů {0}:{1}.", numPiecesWhite, numPiecesBlack);
            else if (move.RemovedPieces.Count > 0)
                Console.WriteLine(" Kamen zajat. Nový počet kamenů {0}:{1}.", numPiecesWhite, numPiecesBlack);
        }

        private void QueryCancelComputation()
        {
            while (!Console.KeyAvailable && Game.IsComputing)
            {

            }
            if (Game.IsComputing)
            {
                char c = Console.ReadKey(true).KeyChar;
                if (Char.ToUpper(c) == 'X')
                {
                    Game.CancelBrainComputation();
                    ShowMenuForComputerPlayer = true;
                }
                else
                    QueryCancelComputation();
            }
        }

        private void Game_BrainComputationStarted()
        {
            Console.WriteLine(" Výpočet tahu byl zahájen...");
            Console.WriteLine("    Stiskněte klávesu X pro přerušení výpočtu.");
            Task.Run((Action)QueryCancelComputation);
        }

        private void Game_BrainComputationFinished(Move bestMove, string errorMessage, bool isCancelled)
        {
            if (isCancelled)
                Console.WriteLine(" Výpočet byl ukončen: {0}", errorMessage);
            else if (string.IsNullOrEmpty(errorMessage))
                Console.WriteLine(" Výpočet tahu byl dokončen. Vypočítaný tah: {0}", bestMove);
            else
                Console.WriteLine(" CHYBA při výpočtu tahu: {0}", errorMessage);
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
        /// Vykreslit menu
        /// </summary>
        /// <returns>False = ukoncit hru</returns>
        private bool RenderMenu()
        {
            // zjistit tah hrace nebo umoznit ovladani programu pri tahu
            Player Player = Game.ActivePlayer;
            bool isHuman = Player is HumanPlayer;

            if (!isHuman && !ShowMenuForComputerPlayer)
                return true;

            string prompt;
            if (isHuman)
            {
                HumanMove.Move = null;
                prompt = "Zadejte svůj tah (př. A2A3) nebo řídicí příkaz (? pro nápovědu)...";
            }
            else
                prompt = "Zadejte řídicí příkaz (? pro nápovědu) nebo Enter pro výpočet tahu počítače...";

            string str;
            Console.Write(prompt);
            str = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(str))
            {
                if (isHuman)
                    Console.WriteLine("CHYBA: Byl zadán prázdný řetězec.");
                else
                {
                    ShowMenuForComputerPlayer = false;
                    return true;
                }
            }
            else
            {
                char command = Char.ToUpper(str[0]);
                switch (command)
                {
                    case '?':
                        Console.WriteLine("Příkazy, které je možné zadat:");
                        Console.WriteLine("    Y = napoveda nejlepsiho tahu");
                        Console.WriteLine("    R = znovu vykreslit hraci desku"); // pouze pro konzolu
                        Console.WriteLine("    S = změnit nastavení hráčů");
                        Console.WriteLine("    X = ukoncit hru");
                        break;
                    case 'Y':
                        Task<Move> calcMove = null;
                        try
                        {
                            using (System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource())
                            {
                                Game_BrainComputationStarted();
                                calcMove = Task<Move>.Run(new Func<Move>(() =>
                                    {
                                        Brain brain = Game.GetBrainInstance();
                                        brain.ComputeBestMove(Player.Level, Player.Color, cts.Token);
                                        return brain.BestMove;
                                    }), cts.Token);
                                while (true)
                                {
                                    if (calcMove.IsCompleted)
                                    {
                                        Game_BrainComputationFinished(calcMove.Result, string.Empty, false);
                                        break;
                                    }
                                    else if (Console.KeyAvailable)
                                    {
                                        if (Char.ToUpper(Console.ReadKey(true).KeyChar) == 'X')
                                            cts.Cancel();
                                    }
                                }
                            }
                        }
                        catch (AggregateException exc)
                        {
                            if ((exc.InnerExceptions[0] is OperationCanceledException) || calcMove.IsCanceled)
                                Game_BrainComputationFinished(null, "Operace byla přerušena uživatelem.", true);
                            else
                                Game_BrainComputationFinished(null, string.Join(Environment.NewLine, exc.InnerExceptions.Select(s => s.Message)), false);
                        }
                        catch (Exception exc)
                        {
                            Game_BrainComputationFinished(null, exc.Message, false);
                        }
                        break;
                    case 'S':
                        string oldSettings = Game.CurrentPlayersSetting;
                        try
                        {
                            string newSettings = GetPlayersSetting(oldSettings);
                            if (oldSettings != newSettings)
                            {
                                Game.SetPlayersFromString(newSettings);
                                Console.WriteLine("Nastavení hráčů bylo změněno.");
                            }
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine("CHYBA: Nastavení hráčů nebylo změněno: {0}", exc.Message);
                            Game.SetPlayersFromString(oldSettings);
                        }
                        break;
                    case 'R':
                        try
                        {
                            Game_RenderBoard(Game);
                            Game_RenderActivePlayer(Game, Player);
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine("CHYBA: Desku se nepodařilo překreslit ! {0}", exc.Message);
                        }
                        break;
                    case 'X':
                        Game.EndGame();
                        return false;
                    default:
                        // jinak předpokládáme, že nejde o příkaz, ale byl zadán tah
                        if (isHuman)
                        {
                            try
                            {
                                HumanMove.Move = Move.Parse(str, (Player.Color == GameColorsEnum.plrBlack) ? Pieces.pcBlack : Pieces.pcWhite);
                                return true;
                            }
                            catch (Exception exc)
                            {
                                Console.WriteLine("Chyba zadání: {0}", exc.Message);
                            }
                        }
                        break;
                }
            } // prazdny retezec zadan

            return RenderMenu();
        }

        /// <summary>
        /// Spustit (UI loop)
        /// </summary>
        public void RunUI()
        {
            Game = GetNewGameInstance();
            Console.WriteLine("Instance hry {0} {1} byla vytvořena.", Game.Title, Game.Version);
            try
            {
                HookGameEvents(Game);

                string playersSetting = GetPlayersSetting("H1C1");
                Console.WriteLine("Spouštím hru.");

                char ch;
                do
                {
                    Game.Run(playersSetting);

                    while (RenderMenu() && Game.Proceed())
                    {
                    };

                    Console.WriteLine();
                    Console.Write("Chcete hrát znovu ? (A/N) ");
                    ch = Console.ReadKey().KeyChar;
                    Console.WriteLine();
                } 
                while (Char.ToUpper(ch) == 'A');
            }
            catch (AggregateException exc)
            {
                Console.WriteLine("CHYBA ! Při běhu hry došlo k chybám !");
                foreach (Exception ex in exc.InnerExceptions)
                {
                    Console.WriteLine("  {0}", ex.Message);
                }
                Console.ReadLine();
            }
            catch (Exception exc)
            {
                Console.WriteLine("CHYBA ! Při běhu hry došlo k výjimce: {0}", exc.Message);
                Console.ReadLine();
            }
            finally
            {
                UnhookGameEvents(Game);
            }
        }

        public void Dispose()
        {

        }
    }
}
