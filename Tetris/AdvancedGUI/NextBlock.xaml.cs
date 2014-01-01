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
using Tetris.GameBase;
using Tetris.AdvancedGUI.Styles;

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// NextBlockGrid.xaml 的交互逻辑
    /// </summary>
    public partial class NextBlock : Border, IDisplay
    {
        Color[] colorMap;
        Rectangle[,] squaresMatrix; // hold the squares

        private Brush[] brushesMap = SquareGenerator.brushesMap;

        Grid nextBlockGrid;

        double canvasHeight;
        double canvasWidth;
        int id = -1;

        int _gridHeight = 4;
        int _gridWidth = 3;

        double _squareSize;

        // height or width means the maximum number of squares contained
        public NextBlock(double h, double w)
        {
            InitializeComponent();

            colorMap = SquareGenerator.colorMap;

            _squareSize = SquareGenerator.squareSize;
            canvasHeight = h;
            canvasWidth = w;

            nextBlockGrid = new Grid();
            Canvas aCanvas = new Canvas();
            aCanvas.Children.Add(nextBlockGrid);

            this.Height = canvasHeight;
            this.Width = canvasWidth;

            this.Child = aCanvas;

            aCanvas.Background = new SolidColorBrush(Colors.Transparent);

            squaresMatrix = new Rectangle[_gridHeight, _gridWidth];

            // initialize the grid
            
            GridLength _gridLen = new GridLength(1, GridUnitType.Auto); 
           
            int i = 0;
            int j = 0;
            for (i = 0; i < _gridHeight; i++)
            {
                RowDefinition aRow = new RowDefinition();
                aRow.Height = _gridLen;
                nextBlockGrid.RowDefinitions.Add(aRow);
            }

            for (j = 0; j < _gridWidth; j++)
            {
                ColumnDefinition aCol = new ColumnDefinition();
                aCol.Width = _gridLen;
                nextBlockGrid.ColumnDefinitions.Add(aCol);
            }

            nextBlockGrid.Background = new SolidColorBrush(Colors.Transparent);

            // add squares into the grid
            for (i = 0; i < _gridHeight; i++)
                for (j = 0; j < _gridWidth; j++)
                {
                    squaresMatrix[i, j] = new Rectangle();
                    nextBlockGrid.Children.Add(squaresMatrix[i, j]);
                    squaresMatrix[i, j].SetValue(Grid.RowProperty, i);
                    squaresMatrix[i, j].SetValue(Grid.ColumnProperty, j);
                    squaresMatrix[i, j].Width = _squareSize;
                    squaresMatrix[i, j].Height = _squareSize;
                    squaresMatrix[i, j].Margin = new Thickness(1, 1, 1, 1);
                    // put this into square generator
                }
        }

        public double[] getSize() 
        {
            return (new double[2] { canvasHeight, canvasWidth });
        }

        public void OnDrawing(Tetris.GameBase.TetrisGame game,
            Tetris.GameBase.TetrisGame.DrawEventArgs e)
        {
            var factory = game.Factory as CacheFactory;
            var block = factory.NextBlock();
            if (block.Id == id)
                return;
            id = block.Id;
            int i, j;

            try
            {
                nextBlockGrid.Dispatcher.Invoke(
                    new Action(
                        delegate
                        {
                            
                            nextBlockGrid.SetValue(Canvas.TopProperty, 
                                (this.Height - 6 - _squareSize * block.Width) / 2 );
                            nextBlockGrid.SetValue(Canvas.LeftProperty,
                                (this.Width - 6 - _squareSize * block.Height) / 2);
                             
                        }
                    ));
            }
            catch { }

            for (i = 0; i < _gridHeight; i++)
                for (j = 0; j < _gridWidth; j++)
                {
                    try
                    {
                        squaresMatrix[i, j].Dispatcher.Invoke(
                            new Action(
                                delegate
                                {
                                    /*
                                    squaresMatrix[i, j].Fill =
                                        //new SolidColorBrush(Colors.Transparent);
                                       
                                        new SolidColorBrush(colorMap[block.SquareAt(j, i) == null ? 0 :
                                            (block.SquareAt(j, i).Color < colorMap.Length ?
                                            block.SquareAt(j, i).Color : colorMap.Length - 1)]);
                                    */
                                    squaresMatrix[i, j].Fill = SquareGenerator.brushClone(
                                           brushesMap[
                                               block.SquareAt(j, i) == null
                                                   ? 0
                                                       : (block.SquareAt(j, i).Color + 
                                                            block.SquareAt(j, i).SubId
                                                                    < brushesMap.Length
                                                           ? block.SquareAt(j, i).Color + 
                                                                    block.SquareAt(j, i).SubId
                                                           : brushesMap.Length - 1)]);
                                        

                                }
                        ));
                    }
                    catch { }
                }
        }
    }
}
