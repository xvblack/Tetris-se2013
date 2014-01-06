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
    /// Game Mode Selection Page
    /// </summary>
    /// 
    public partial class GameModeSelectionPage : Page
    {
        NavigationService nav; // Navigate the page
        public MainWindow holderWin;     // to change the size of main window
        
        public GameModeSelectionPage()
        {

            // layout
            InitializeComponent();

            Grid aGrid = ButtonGrid1(holder);
            addChildrenGrid(holder, aGrid, 1);

            StringGrid s = new StringGrid("G", SquareGenerator.squareSize * 1.5);
            s.noAnimation();

            aCanvas.Children.Add(s);
            s.SetValue(Canvas.RightProperty, 0.0);
            s.SetValue(Canvas.TopProperty, 
                (WindowSizeGenerator.screenHeight - s.getHeight())/2);
            s.SetValue(Canvas.ZIndexProperty, 2);

            Pic.PicGen pic = new Pic.CatGen();
            Pic.PicGenGrid pg = new Pic.PicGenGrid(pic, SquareGenerator.picSquareSize / 1.2);
            aCanvas.Children.Add(pg);
            pg.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetRight(pg, 2);
            Canvas.SetBottom(pg, 2);

        }

        // used to show different groups of settings
        private void addChildrenGrid(Grid holder, Grid Child, double opaque)
        {
            holder.Children.Add(Child);
            Child.SetValue(Grid.ColumnProperty, 2);
            Child.SetValue(Grid.RowProperty, 2);
            Child.Opacity = opaque;
        }

        // used to generate the layout
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

        // used to show the animation of switching settings
        private void switchGrid(Grid fromGrid, Grid toGrid, Grid holder, Button trigger)
        {
            trigger.IsEnabled = false;

            addChildrenGrid(holder, toGrid, 0);
            DoubleAnimation fa = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.2)); // the last gone animation
            DoubleAnimation ta = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.2)); // the next comes

            ta.Completed += new EventHandler(
                delegate
                {
                    holder.Children.Remove(fromGrid);
                    trigger.IsEnabled = true;
                });
  
            fa.Completed += new EventHandler(
                delegate
                {
                    toGrid.BeginAnimation(Grid.OpacityProperty, ta); 
                });

            fromGrid.BeginAnimation(Grid.OpacityProperty, fa);        
        }

        // used to show the animation inside a group of settings
        private void switchContent(FrameworkElement from, FrameworkElement to, Grid holder, Button trigger)
        {
            trigger.IsEnabled = false;
            holder.Children.Add(to);

            DoubleAnimation fa = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.2));// the last gone
            DoubleAnimation ta = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.2));// the next cames

            ta.Completed += new EventHandler(
                delegate
                {
                    holder.Children.Remove(from);
                    trigger.IsEnabled = true;
                });

            fa.Completed += new EventHandler(
                delegate
                {
                    to.BeginAnimation(Grid.OpacityProperty, ta);
                });

            from.BeginAnimation(Grid.OpacityProperty, fa); 
        }

        // the first group of setting
        private Grid ButtonGrid1(Grid holder)
        {
            Grid aGrid = new Grid();
            setGrid(1, 3, aGrid);

            int colorNum = 6;

            Color[] colors = SquareGenerator.randomColor(colorNum);

            CustomizedButton2 singleModeButton =
                new CustomizedButton2("单人游戏", colors[0]);
            singleModeButton.button.Click += new RoutedEventHandler(
                delegate { 
                    Grid nextGrid = this.ButtonGridSingle(holder);
                    switchGrid(aGrid, nextGrid, holder, singleModeButton.button);
                });
            singleModeButton.SetValue(Grid.RowProperty, 0);

            CustomizedButton2 dualModeButton =
                new CustomizedButton2("双人游戏", colors[1]);
            dualModeButton.button.Click += new RoutedEventHandler(
                delegate
                {
                    Grid nextGrid = this.ButtonGridDual(holder);
                    switchGrid(aGrid, nextGrid, holder, dualModeButton.button);
                });
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

        // set the text box to change the names of players
        private TextBox setTextBox(int row, int col, int nameIndex)
        {
            TextBox aBox = new TextBox();
            String tmp = PlayersName.getName(nameIndex);
            aBox.Text = tmp == "_AI" ? "" : tmp;
            aBox.BorderThickness = new Thickness(0, 0, 0, 0);
            aBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            aBox.FontSize = Styles.WindowSizeGenerator.fontSizeMedium;
            aBox.SetValue(Grid.RowProperty, row);
            aBox.SetValue(Grid.ColumnProperty, col);
            aBox.Background = new SolidColorBrush(Colors.Transparent);
            aBox.Focus();

            return (aBox);
        }

        // a label
        private CustomizedLabel whoAreYouLabel(int row, Color c, String content)
        {
            CustomizedLabel l = new CustomizedLabel(c, content);
            l.SetValue(Grid.RowProperty, row);

            return (l);
        }

        // the single mode setting
        private Grid ButtonGridSingle(Grid holder)
        {
            Grid aGrid = new Grid();
            setGrid(2, 3, aGrid);

            int colorNum = 6;

            Color[] colors = SquareGenerator.randomColor(colorNum);

            CustomizedLabel whoAreYouPlayer1 = whoAreYouLabel(0, colors[0], "昵称:");

            TextBox aBox = setTextBox(0, 1, 0);

            CustomizedButton2 goButton =
                new CustomizedButton2("开始游戏", colors[1]);
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
                        switchGrid(aGrid, nextGrid, holder, backButton.button);
                    }); 
            backButton.SetValue(Grid.RowProperty, 3);

            aGrid.Children.Add(whoAreYouPlayer1);
            aGrid.Children.Add(aBox);
            aGrid.Children.Add(goButton);
            aGrid.Children.Add(backButton);

            return (aGrid);
        }

        // a label
        private SwitchLabel setSwitchLabel(int row, String[] contents, double opaque)
        {
            SwitchLabel l = new SwitchLabel(contents);
            l.SetValue(Grid.RowProperty, row);
            l.SetValue(Grid.ColumnProperty, 1);
            l.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            l.Opacity = opaque;

            return (l);
        }

        // the dual mode setting
        private Grid ButtonGridDual(Grid holder)
        {
            String[] contents1 = new string[2]{"  人  ", "电  脑"};
            String[] contents2 = new string[3] { "低难度", "中难度", "高难度" };

            Grid aGrid = new Grid();
            setGrid(2, 6, aGrid);

            int colorNum = 6;
            Color[] colors = SquareGenerator.randomColor(colorNum);

            CustomizedLabel player1 = whoAreYouLabel(0, colors[5], "玩家1:");

            SwitchLabel player1Sel = setSwitchLabel(0, contents1, 1);

            CustomizedLabel whoAreYouPlayer1 = whoAreYouLabel(1, colors[0], "昵称:");
            CustomizedLabel difficulty1 = whoAreYouLabel(1, colors[0], "难  度:");
            difficulty1.Opacity = 0;

            TextBox aBox1 = setTextBox(1, 1, 0);

            SwitchLabel dif1Sel = setSwitchLabel(1, contents2, 0);

            CustomizedLabel player2 = whoAreYouLabel(2, colors[3], "玩家2:");

            SwitchLabel player2Sel = setSwitchLabel(2, contents1, 1);

            CustomizedLabel whoAreYouPlayer2 = whoAreYouLabel(3, colors[4], "昵称:");
            CustomizedLabel difficulty2 = whoAreYouLabel(3, colors[4], "难  度:");
            difficulty2.Opacity = 0;

            TextBox aBox2 = setTextBox(3, 1, 1);
            SwitchLabel dif2Sel = setSwitchLabel(3, contents2, 0);

            DualModePage.gameMode[] pModes = new DualModePage.gameMode[2];

            CustomizedButton2 goButton =
                new CustomizedButton2("开始游戏", colors[2]);

            // used to store the settings and go to dual game 
            goButton.button.Click += new RoutedEventHandler(
                delegate
                {
                    if (player1Sel.getLabelIndex() == 0) // player1 is 人 
                    {
                        PlayersName.setName(0, aBox1.Text);
                        pModes[0] = new DualModePage.gameMode(0, 0);
                    }
                    else
                    {
                        PlayersName.setName(0, "_AI");
                        pModes[0] = new DualModePage.gameMode(1, dif1Sel.getLabelIndex());
                    }
                    if (player2Sel.getLabelIndex() == 0) // player2 is 人 
                    {
                        PlayersName.setName(1, aBox2.Text);
                        pModes[1] = new DualModePage.gameMode(0, 0);
                    }
                    else
                    {
                        PlayersName.setName(1, "_AI");
                        pModes[1] = new DualModePage.gameMode(1, dif2Sel.getLabelIndex());
                    }

                    nav = NavigationService.GetNavigationService(this);
                    DualModePage nextPage = new DualModePage(pModes);
                    nextPage.holderWin = holderWin;
                    nav.Navigate(nextPage);
                });

            goButton.SetValue(Grid.RowProperty, 4);

            CustomizedButton2 backButton =
                new CustomizedButton2("后  退", colors[1]);
            backButton.button.Click += new RoutedEventHandler(
                    delegate
                    {
                        Grid nextGrid = ButtonGrid1(holder);
                        switchGrid(aGrid, nextGrid, holder, backButton.button);
                    }); 
            backButton.SetValue(Grid.RowProperty, 6);

            aGrid.Children.Add(player1);
            aGrid.Children.Add(player2);
            aGrid.Children.Add(player1Sel);
            aGrid.Children.Add(whoAreYouPlayer1);
            aGrid.Children.Add(aBox1);
            aGrid.Children.Add(player2Sel);
            aGrid.Children.Add(whoAreYouPlayer2);
            aGrid.Children.Add(aBox2);
            aGrid.Children.Add(goButton);
            aGrid.Children.Add(backButton);

            setClickSwitch(whoAreYouPlayer1, difficulty1, aBox1, dif1Sel, player1Sel, aGrid);
            setClickSwitch(whoAreYouPlayer2, difficulty2, aBox2, dif2Sel, player2Sel, aGrid);

            return (aGrid);
        }

        // a switching animation
        private void setClickSwitch(FrameworkElement from1, FrameworkElement to1,
            FrameworkElement from2, FrameworkElement to2, SwitchLabel l, Grid holder)
        {
            l.p.Click += new RoutedEventHandler(
                delegate
                {
                    if (l.getLabelIndex() == 0)
                    {
                        switchContent(from1, to1, holder, l.p);
                        switchContent(from2, to2, holder, l.p);
                    }
                });
            l.n.Click += new RoutedEventHandler(
                delegate
                {
                    if (l.getLabelIndex() == 1)
                    {
                        switchContent(to1, from1, holder, l.n);
                        switchContent(to2, from2, holder, l.p);
                    }
                });
        }

        // Click to start dual mode game
        private void selDualMode_Click(object sender, RoutedEventArgs e)
        { 
            
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
