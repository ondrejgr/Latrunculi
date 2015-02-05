using Latrunculi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Latrunculi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _current = this;
        }

        static private MainWindow _current;
        static public MainWindow Current
        {
            get
            {
                return _current;
            }
        }

        public MainWindowViewModel ViewModel
        {
            get
            {
                return DataContext as MainWindowViewModel;
            }
        }

        private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
            e.CanExecute = true;
        }

        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            Close();
        }

        private bool GetCanCloseApplication()
        {
            return MessageBox.Show(this, "Chcete opravdu hru ukončit ?", ViewModel.Title, MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !GetCanCloseApplication();
        }
    }
}
