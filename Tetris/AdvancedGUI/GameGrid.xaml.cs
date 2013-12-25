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
    /// </summary>
    public partial class GameGrid : Grid, IDisplay
    {
        Color[] colorMap = (new Styles.SquareGenerator()).getColorMap();
        Rectangle[,] squaresMatrix; // hold the squares
        int gridHeight;
        int gridWidth;

        // height or width means the maximum number of squares contained
        public GameGrid(int[] gridSize) 
        {
            gridHeight = gridSize[0];
            gridWidth = gridSize[1];

            squaresMatrix = new Rectangle[gridHeight, gridWidth];

            // size definitions
            int _squareContainerSize = 
                Styles.SquareGenerator.squareContainerSize();
            int _squareSize = Styles.SquareGenerator.squareSize();
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
                    /*
                    Border border = new Border();
                    border.BorderBrush = new SolidColorBrush(Colors.Black);
                    border.BorderThickness = new Thickness(1,1,1,1);
                    border.Child = squaresMatrix[i, j];
                    this.Children.Add(border);
                    border.SetValue(Grid.RowProperty, i);
                    border.SetValue(Grid.ColumnProperty, j);
                    */
                    this.Children.Add(squaresMatrix[i, j]);
                    squaresMatrix[i, j].SetValue(Grid.RowProperty, i);
                    squaresMatrix[i, j].SetValue(Grid.ColumnProperty, j);
                    squaresMatrix[i, j].Width = _squareSize;
                    squaresMatrix[i, j].Height = _squareSize;
                    squaresMatrix[i, j].Margin = new Thickness(1, 1, 1, 1);
                }
           // this.testSquares();
        }
        public void OnDrawing(Tetris.GameBase.TetrisGame game,
            Tetris.GameBase.TetrisGame.DrawEventArgs e) 
        {
                Square[,] image = game.Image;
                int i, j;

                for (i = 0; i < game.Height; i++)
                    for (j = 0; j < game.Width; j++)
                    {
                        try
                        {
                            squaresMatrix[i, j].Dispatcher.Invoke(
                                new Action(
                                    delegate
                                    {
                                        squaresMatrix[i, j].Fill = 
                                            new SolidColorBrush(colorMap[image[i, j] == null ? 0 : (image[i, j].Color < colorMap.Length ? image[i, j].Color : colorMap.Length - 1)]);
                                    }
                            ));
                        }
                        catch {}
                    }
        }

        public void testSquares()
        {
            int i = 0;
            int j = 0;
            Random num = new Random();
            for (i = 0; i < gridHeight; i++)
                for (j = 0; j < gridWidth; j++)
                {
                    squaresMatrix[i, j].Fill =
                        new SolidColorBrush(colorMap[num.Next(9)]);
                }
        }
    }
}
