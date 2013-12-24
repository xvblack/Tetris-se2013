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
using Tetris.AdvancedGUI.Pic;

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

        private ScoreGrid score;

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

            PicGen pic = new CrocodileGen();

            PicGenGrid pg1 = new PicGenGrid(pic, Styles.SquareGenerator.picSquareSize);
            aCanvas.Children.Add(pg1);
            pg1.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetLeft(pg1, 20);
            Canvas.SetBottom(pg1, 50);

            pic = new CrocodileGen();
            PicGenGrid pg2 = new PicGenGrid(pic, Styles.SquareGenerator.picSquareSize);
            aCanvas.Children.Add(pg2);
            pg2.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetLeft(pg2, 350);
            Canvas.SetBottom(pg2, 50);

            pic = new SunGen();
            PicGenGrid pg3 = new PicGenGrid(pic, Styles.SquareGenerator.picSquareSize/1.5);
            aCanvas.Children.Add(pg3);
            pg3.SetValue(Canvas.ZIndexProperty, 0);

            Canvas.SetLeft(pg3, -50);
            Canvas.SetTop(pg3, -50);

            double scoreHeight = 150;
            double scoreWidth = 200;
            double scoreRightLoc = 50;
            double scoreTopLoc = 50;
            score = new ScoreGrid(scoreHeight, scoreWidth);

            aCanvas.Children.Add(score);
            score.SetValue(Canvas.RightProperty, scoreRightLoc + scoreHeight);
            score.SetValue(Canvas.TopProperty, scoreTopLoc);

            score.DataContext = game.ScoreSystem;

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
