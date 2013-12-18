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
    public partial class SingleModePage : GameContainerPage
    {
        private Tetrisor t = new Tetrisor();
        private GameGrid gameGrid;
        private Tetris.GameBase.TetrisGame game;

        public SingleModePage():base()
        {
            InitializeComponent();

            ColumnDefinition aCol = new ColumnDefinition();
            aCol.Width = new GridLength(50, GridUnitType.Star);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(50, GridUnitType.Auto);
            outerGrid.ColumnDefinitions.Add(aCol);

            aCol = new ColumnDefinition();
            aCol.Width = new GridLength(50, GridUnitType.Star);
            outerGrid.ColumnDefinitions.Add(aCol);
    
            Border border = new Border();

            border.BorderBrush = new SolidColorBrush(Colors.Gray);
            border.BorderThickness = new Thickness(1, 1, 1, 1);

            game = t.NewGame();

            int[] gridSize = new int[2] { game.Height, game.Width };

            gameGrid = new GameGrid(gridSize);

            game.AddDisplay(gameGrid);
            AIController _aiController = new AIController(game, 100);
            game.SetController(_aiController);

            border.Child = gameGrid;
            outerGrid.Children.Add(border);
            border.SetValue(Grid.RowProperty, 1);
            border.SetValue(Grid.ColumnProperty, 1);
        }

        protected override void Loaded_Event(object sender, RoutedEventArgs e)
        {
            holderWin.Width = Styles.WindowSizeGenerator.singleModePageWidth;
            holderWin.Left = Styles.WindowSizeGenerator.singleModePageLocationLeft;

            outerGrid.Width = holderWin.Width;
            outerGrid.Height = holderWin.Height;

            base.Loaded_Event(sender, e);

        }

        protected override void whatHappenWhenAnimationStop(object sender, System.Timers.ElapsedEventArgs e)
        {
            game.Start();
            base.whatHappenWhenAnimationStop(sender, e);
        }
    }
}
