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

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// NextBlockGrid.xaml 的交互逻辑
    /// </summary>
    public partial class NextBlockGrid : Grid, IDisplay
    {
        Color[] colorMap = (new Styles.SquareGenerator()).getColorMap();
        Rectangle[,] squaresMatrix; // hold the squares

        int gridHeight;
        int gridWidth;
        int id = -1;

        int _squareContainerSize =
                Styles.SquareGenerator.squareContainerSize();
        int _squareSize = Styles.SquareGenerator.squareSize();

        // height or width means the maximum number of squares contained
        public NextBlockGrid(int[] gridSize)
        {
            gridHeight = gridSize[0];
            gridWidth = gridSize[1];

            squaresMatrix = new Rectangle[gridHeight, gridWidth];

            // size definitions
            
            GridLength _gridLen = new GridLength(1,
                GridUnitType.Auto);  // square size
            // initialize the grid
            int i = 0;
            int j = 0;
            for (i = 0; i < gridHeight; i++)
            {
                RowDefinition aRow = new RowDefinition();
                aRow.Height = _gridLen;
                this.RowDefinitions.Add(aRow);
            }

            for (j = 0; j < gridWidth; j++)
            {
                ColumnDefinition aCol = new ColumnDefinition();
                aCol.Width = _gridLen;
                this.ColumnDefinitions.Add(aCol);
            }
            this.Background = new SolidColorBrush(Colors.White);
            // add squares into the grid
            for (i = 0; i < gridHeight; i++)
                for (j = 0; j < gridWidth; j++)
                {
                    squaresMatrix[i, j] = new Rectangle();
                    this.Children.Add(squaresMatrix[i, j]);
                    squaresMatrix[i, j].SetValue(Grid.RowProperty, i);
                    squaresMatrix[i, j].SetValue(Grid.ColumnProperty, j);
                    squaresMatrix[i, j].Width = _squareSize;
                    squaresMatrix[i, j].Height = _squareSize;
                    squaresMatrix[i, j].Margin = new Thickness(1, 1, 1, 1);
                }
        }

        public double[] getSize() 
        {
            double height = gridHeight * _squareSize;
            double width = gridWidth * _squareSize;
            return (new double[2] { height, width });
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
            int x, y;
            int dx, dy;
            // To make the block in the center of grid
            dx = (gridHeight - 5) / 2;
            dy = (gridWidth - 5) / 2;
            x = dx + 2 - block.Width / 2;
            y = dy + 2 - block.Height / 2;

            for (i = dx; i < gridHeight - dx; i++)
                for (j = dy; j < gridWidth - dy; j++)
                {
                    try
                    {
                        squaresMatrix[i, j].Dispatcher.Invoke(
                            new Action(
                                delegate
                                {
                                    squaresMatrix[i, j].Fill =
                                        new SolidColorBrush(colorMap[block.SquareAt(block.Height - j - 1 + y, i - x) == null ? 0 :
                                            (block.SquareAt(block.Height - j - 1 + y, i - x).Color < colorMap.Length ?
                                            block.SquareAt(block.Height - j - 1 + y, i - x).Color : colorMap.Length - 1)]);
                                }
                        ));
                    }
                    catch { }
                }
        }
    }
}
