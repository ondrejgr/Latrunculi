using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Latrunculi
{
    static public class MainWindowCommands
    {
        static private readonly RoutedUICommand _exitCommand = new RoutedUICommand("_Konec", "ExitCommand", typeof(MainWindow), GetKeyGesture(Key.F4, ModifierKeys.Alt));
        static public RoutedUICommand ExitCommand
        {
            get
            {
                return _exitCommand;
            }
        }

        static private InputGestureCollection GetKeyGesture(Key key, ModifierKeys modifiers)
        {
            InputGestureCollection col = new InputGestureCollection();
            col.Add(new KeyGesture(key, modifiers));

            return col;
        }
    }
}
