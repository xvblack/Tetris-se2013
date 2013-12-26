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
using System.Windows.Shapes;

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// EscapeDialog.xaml 的交互逻辑
    /// </summary>
    public partial class EscapeDialog : Window
    {
        public MainWindow holderWindow;
        private readonly Tetris.GameBase.TetrisGame _game1;
        private readonly Tetris.GameBase.TetrisGame _game2;

        public EscapeDialog(Tetris.GameBase.TetrisGame game1, Tetris.GameBase.TetrisGame game2 = null)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Topmost = true;
            this._game1 = game1;
            this._game2 = game2;
        }
        private void okayClicked(object sender, RoutedEventArgs e) {
            NavigationPage backPage = new NavigationPage();
            backPage.holderWin = this.holderWindow;
            this.holderWindow.Navigate(backPage);
            this.Close();
        }
        private void cancelClicked(object sender, RoutedEventArgs e) 
        {
            _game1.Continue();
            if (_game2 != null) _game2.Continue();
            this.Close(); 
        }
    }
}
