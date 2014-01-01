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
using Tetris.AdvancedGUI.Styles;
using Tetris.GameControl;
using System.Windows.Media.Animation;

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// GameModeSelectionPage.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class GameModeSelectionPage : Page
    {
        NavigationService nav; // Navigate the page
        public MainWindow holderWin;     // to change the size of main window
        
        public GameModeSelectionPage()
        {
            InitializeComponent();

            Grid aGrid = ButtonGrid1(holder);
            addChildrenGrid(holder, aGrid, 1);

        }

        private void addChildrenGrid(Grid holder, Grid Child, double opaque)
        {
            holder.Children.Add(Child);
            Child.SetValue(Grid.ColumnProperty, 2);
            Child.SetValue(Grid.RowProperty, 2);
            Child.Opacity = opaque;
        }

        private void setGrid(int colNum, int rowNum, Grid aGrid)
        {
            int i = 0;
            ColumnDefinition aCol = new ColumnDefinition();
            RowDefinition aRow = new RowDefinition();
            for (i = 0; i < colNum; i++)
            {
                aCol = new ColumnDefinition();
                if (i == 0) aCol.Width = new GridLength(200, GridUnitType.Pixel);
                aGrid.ColumnDefinitions.Add(aCol);
            }
            for (i = 0; i < rowNum + 1; i++)
            {
                aRow = new RowDefinition();
                if (i == rowNum - 1) aRow.Height = new GridLength(20, GridUnitType.Pixel);
                aGrid.RowDefinitions.Add(aRow);
            }        
        }

        private void switchGrid(Grid fromGrid, Grid toGrid, Grid holder)
        {
            addChildrenGrid(holder, toGrid, 0);
            DoubleAnimation fa = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.2));
            DoubleAnimation ta = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.2));

            ta.Completed += new EventHandler(
                delegate
                {
                    holder.Children.Remove(fromGrid);
                });
  
            fa.Completed += new EventHandler(
                delegate
                {
                    toGrid.BeginAnimation(Grid.OpacityProperty, ta); 
                });

            fromGrid.BeginAnimation(Grid.OpacityProperty, fa);        
        }

        private Grid ButtonGrid1(Grid holder)
        {
            Grid aGrid = new Grid();
            aGrid.ShowGridLines = true;
            setGrid(1, 3, aGrid);

            int colorNum = 6;

            Color[] colors = SquareGenerator.randomColor(colorNum);

            CustomizedButton2 singleModeButton =
                new CustomizedButton2("单人游戏", colors[0]);
            singleModeButton.button.Click += new RoutedEventHandler(
                delegate { 
                    Grid nextGrid = this.ButtonGridSingle(holder);
                    switchGrid(aGrid, nextGrid, holder);
                });
            singleModeButton.SetValue(Grid.RowProperty, 0);

            CustomizedButton2 dualModeButton =
                new CustomizedButton2("双人游戏", colors[1]);
            //dualModeButton.button.Click += selDualMode_Click;
            dualModeButton.SetValue(Grid.RowProperty, 1);

            CustomizedButton2 backButton =
                new CustomizedButton2("后  退", colors[2]);
            backButton.button.Click += back_Click;
            backButton.SetValue(Grid.RowProperty, 3);
            
            aGrid.Children.Add(singleModeButton);
            aGrid.Children.Add(dualModeButton);
            aGrid.Children.Add(backButton);
            
            return (aGrid);
        }

        private Grid ButtonGridSingle(Grid holder)
        {
            Grid aGrid = new Grid();
            aGrid.ShowGridLines = true;
            setGrid(2, 3, aGrid);

            int colorNum = 6;

            Color[] colors = SquareGenerator.randomColor(colorNum);

            CustomizedLabel whoAreYouPlayer1 =
                new CustomizedLabel( colors[0], "您哪位:");
            //whoAreYouPlayer1.Width = 100;
            whoAreYouPlayer1.SetValue(Grid.RowProperty, 0);

            TextBox aBox = new TextBox();
            aBox.Text = PlayersName.getName(0);
            aBox.BorderThickness = new Thickness(0, 0, 0, 0);
            aBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            aBox.FontSize = Styles.WindowSizeGenerator.fontSizeMedium;
            aBox.SetValue(Grid.RowProperty, 0);
            aBox.SetValue(Grid.ColumnProperty, 1);
            aBox.Focus();

            CustomizedButton2 goButton =
                new CustomizedButton2("开始游戏！", colors[1]);
            //goButton.Width = 100;
            goButton.button.Click += new RoutedEventHandler(
                delegate
                {
                    PlayersName.setName(0, aBox.Text);
                    nav = NavigationService.GetNavigationService(this);
                    SingleModePage nextPage = new SingleModePage();
                    nextPage.holderWin = holderWin;
                    nav.Navigate(nextPage);
                });
            goButton.SetValue(Grid.RowProperty, 1);

            CustomizedButton2 backButton =
                new CustomizedButton2("后  退", colors[2]);
            //backButton.Width = 100;
            backButton.button.Click += new RoutedEventHandler(
                    delegate
                    {
                        Grid nextGrid = ButtonGrid1(holder);
                        switchGrid(aGrid, nextGrid, holder);
                    }); 
            backButton.SetValue(Grid.RowProperty, 3);

            aGrid.Children.Add(whoAreYouPlayer1);
            aGrid.Children.Add(aBox);
            aGrid.Children.Add(goButton);
            aGrid.Children.Add(backButton);

            return (aGrid);
        }
            

        // Click to start single mode game
        private void selSingleMode_Click(object sender, RoutedEventArgs e)
        {
            
        }

        // Click to start dual mode game
        private void selDualMode_Click(object sender, RoutedEventArgs e)
        { 
            nav = NavigationService.GetNavigationService(this);
            difficultySelectionPage nextPage = new difficultySelectionPage();
            nextPage.holderWin = holderWin;
            nav.Navigate(nextPage);
        }
        // Click to go to the last page
        private void back_Click(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            NavigationPage backPage = new NavigationPage();
            backPage.holderWin = holderWin;
            nav.Navigate(backPage);            
        }
    }
}
