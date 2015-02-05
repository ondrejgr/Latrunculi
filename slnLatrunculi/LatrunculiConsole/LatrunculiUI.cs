using Latrunculi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace LatrunculiConsole
{
    internal class LatrunculiUI: IDisposable
    {
        private string FileName = "Latrunculi.Impl";
        private ObjectHandle hlib;
        private IGame Game;

        public void LoadLibrary()
        {
            hlib = Activator.CreateInstance(FileName, "Latrunculi.Impl.Game");
            if (hlib == null)
                throw new Exception(string.Format("Nepodařilo se vytvořit instanci logiky hry ze souboru {0}.", FileName));

            Game = hlib.Unwrap() as IGame;
            if (Game == null)
                throw new Exception(string.Format("Nepodařilo se získat instanci logiky hry ze souboru {0}.", FileName));

            Console.WriteLine("Rozhraní hry {0} {1} bylo načteno.", Game.Title, Game.Version);
        }

        public void Dispose()
        {
            
        }
    }
}
