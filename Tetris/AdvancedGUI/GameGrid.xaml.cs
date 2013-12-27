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
        Color[] colorMap;
        Rectangle[,] squaresMatrix; // hold the squares
        int gridHeight;
        int gridWidth;
        double _squareSize;
        private int[,] imageCache = null;

        // height or width means the maximum number of squares contained
        public GameGrid(int[] gridSize) 
        {
            gridHeight = gridSize[0];
            gridWidth = gridSize[1];

            squaresMatrix = new Rectangle[gridHeight, gridWidth];

            // size definitions
            Styles.SquareGenerator squareGen = new Styles.SquareGenerator();

            colorMap = squareGen.colorMap();

            _squareSize = squareGen.squareSize();
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

        public double[] getGameGridSize()
        {
            double height = gridHeight * _squareSize;
            double width = gridWidth * _squareSize;
            return (new double[2]{height, width});
        }

        public void OnDrawing(Tetris.GameBase.TetrisGame game,
            Tetris.GameBase.TetrisGame.DrawEventArgs e) 
        {
                Square[,] image = game.Image;
            if (imageCache == null)
            {
                imageCache=new int[image.GetUpperBound(0)+1,image.GetUpperBound(1)+1];
                Array.Clear(imageCache, 0, (image.GetUpperBound(0) + 1)*(image.GetUpperBound(1)+1));
            }
                int i, j;

                for (i = 0; i < game.Height; i++)
                    for (j = 0; j < game.Width; j++)
                    {
                        if ((image[i, j] == null && imageCache[i, j] != 0) || (image[i, j] != null&&image[i, j].Color != imageCache[i, j]))
                        {
                            imageCache[i, j] = image[i,j]==null?0:image[i, j].Color;
                            try
                            {
                                squaresMatrix[i, j].Dispatcher.Invoke(
                                    new Action(
                                        delegate
                                        {
                                            squaresMatrix[i, j].Fill =
                                                new SolidColorBrush(
                                                    colorMap[
                                                        image[i, j] == null
                                                            ? 0
                                                            : (image[i, j].Color < colorMap.Length
                                                                ? image[i, j].Color
                                                                : colorMap.Length - 1)]);
                                        }
                                        ));
                            }
                            catch
                            {
                            }
                        }
                    }
        }
    }
}
