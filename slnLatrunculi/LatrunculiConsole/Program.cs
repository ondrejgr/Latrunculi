using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatrunculiConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Latrunculi - desková hra");
            try
            {
                // vlastni běh zabalen do objektu LatrunculiUI,
                // abych se vyhnul statickym polim ve tride Program
                using (LatrunculiUI ui = new LatrunculiUI())
                {
                    ui.Run();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine();
                Console.WriteLine(string.Format("CHYBA ! Při běhu aplikace došlo k výjimce: {0}", exc.Message));
            }
            Console.WriteLine("Konec. Stiskněte Enter pro opuštění aplikace.");
            Console.ReadLine();
        }
    }
}
