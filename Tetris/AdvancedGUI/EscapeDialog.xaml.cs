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
    /// Escape Dialog
    /// </summary>
    public partial class EscapeDialog : Window
    {
        public MainWindow holderWindow; // use to navigate the main window
        private GameContainerPage holderPage;
        private readonly Tetris.GameBase.TetrisGame _game1;
        private readonly Tetris.GameBase.TetrisGame _game2;

        public EscapeDialog(GameContainerPage aPage, Tetris.GameBase.TetrisGame game1, Tetris.GameBase.TetrisGame game2 = null)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Topmost = true;
            this._game1 = game1;
            this._game2 = game2;
            this.holderPage = aPage;
        }

        // when okay button is pressed
        private void okayClicked(object sender, RoutedEventArgs e) {
            // bake to the navigation page
            NavigationPage backPage = new NavigationPage();
            backPage.holderWin = this.holderWindow;
            this.holderWindow.Navigate(backPage);
            this.Close();
        }

        //cancel button is pressed
        private void cancelClicked(object sender, RoutedEventArgs e) 
        {
            //resume the game
            if (holderPage.gameHasStarted == true)
            {
                if (_game1 != null) _game1.Continue();
                if (_game2 != null) _game2.Continue();
            }

            // resume the starting animation (the "ready go")
            this.holderPage.welcomeString1.story.Resume(
            this.holderPage.welcomeString1);
            this.holderPage.welcomeString2.story.Resume(
                this.holderPage.welcomeString2);
            this.holderPage.welcomeString1.pauseState = false;
            this.holderPage.welcomeString2.pauseState = false;
             
            // close the dialog
            this.Close(); 
        }
    }
}
