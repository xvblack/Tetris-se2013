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
using System.Windows.Media.Animation;

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// SingleModePage.xaml 的交互逻辑
    /// </summary>
    public partial class DualModePage : GameContainerPage
    {
        private Tetrisor t = new Tetrisor();
        private Tuple<Tetris.GameBase.TetrisGame,
            Tetris.GameBase.TetrisGame> games;

        public DualModePage() : base()
        {
            InitializeComponent();

            ColumnDefinition aCol = new ColumnDefinition();
            aCol.Width = new GridLength(50, GridUnitType.Star);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(50, GridUnitType.Auto);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(200, GridUnitType.Pixel);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(50, GridUnitType.Auto);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(50, GridUnitType.Star);
            outerGrid.ColumnDefinitions.Add(aCol);

            Border border1 = new Border();

            border1.BorderBrush = new SolidColorBrush(Colors.Gray);
            border1.BorderThickness = new Thickness(1, 1, 1, 1);

            Border border2 = new Border();

            border2.BorderBrush = new SolidColorBrush(Colors.Gray);
            border2.BorderThickness = new Thickness(1, 1, 1, 1);

            games = t.NewDuelGame();
            int[] gridSize = new int[2] 
                { games.Item1.Height, games.Item1.Width };

            GameGrid gameGrid1 = new GameGrid(gridSize);
            border1.Child = gameGrid1;
            outerGrid.Children.Add(border1);
            border1.SetValue(Grid.RowProperty, 1);
            border1.SetValue(Grid.ColumnProperty, 1);

            GameGrid gameGrid2 = new GameGrid(gridSize);
            //NextBlockGrid gameGrid2 = new NextBlockGrid(new int[2]{7, 7});
            border2.Child = gameGrid2;
            outerGrid.Children.Add(border2);
            border2.SetValue(Grid.RowProperty, 1);
            border2.SetValue(Grid.ColumnProperty, 3);

            games.Item1.AddDisplay(gameGrid1);
            games.Item2.AddDisplay(gameGrid2);
            //Add Display of NextBlockGrid

            AIController _aiController1 = new AIController(games.Item1, AIController.AIType.Middle);
            AIController _aiController2 = new AIController(games.Item2, AIController.AIType.Low);
            games.Item1.SetController(_aiController1);
            games.Item2.SetController(_aiController2);
            //games.Item1.SetController(_controller[0]);
            //games.Item2.SetController(_controller[1]);

        }

        protected override void Loaded_Event(object sender, RoutedEventArgs e)
        {
            holderWin.Width = Styles.WindowSizeGenerator.dualModePageWidth;
            holderWin.Left = Styles.WindowSizeGenerator.dualModePageLocationLeft;

            outerGrid.Width = holderWin.Width;
            outerGrid.Height = holderWin.Height;

            base.Loaded_Event(sender, e);
        }

        protected override void whatHappenWhenAnimationStop(object sender, System.Timers.ElapsedEventArgs e)
        {
            games.Item1.Start();
            games.Item2.Start();
            
            base.whatHappenWhenAnimationStop(sender, e);
        }
        protected override void keyPressed(object sender, KeyEventArgs e)
        {
            _controller[0].OnKeyDown(e);
            _controller[1].OnKeyDown(e);
            if (e.Key == Key.Escape)
            {
                games.Item1.Pause();
                games.Item2.Pause();
                EscapeDialog win = new EscapeDialog(games.Item1, games.Item2);
                win.holderWindow = this.holderWin;
                win.ShowDialog();
            }
        }
    }
}
