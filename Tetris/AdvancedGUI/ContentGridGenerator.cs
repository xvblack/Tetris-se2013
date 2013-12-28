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
    public partial class ContentGridGenerator: Grid
    {
        
        protected Color[] colorMap;
        protected Rectangle[,] squaresMatrix; // hold the squares
        protected Brush[,] brushesMatrix;

        protected int gridHeight = 100;
        protected int gridWidth = 100;
        protected double squareSize;

        // height or width means the maximum number of squares contained
        public ContentGridGenerator() { }

        protected void initializeGrid(int height, int width, double size, double thick)
        {
            // size definitions

            gridHeight = height;
            gridWidth = width;

            squaresMatrix = new Rectangle[gridHeight, gridWidth];

            colorMap = SquareGenerator.colorMap;
            squareSize = size;

            GridLength _gridLen = new GridLength(1, GridUnitType.Auto);  // square size

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

            // add squares into the grid
            for (i = 0; i < gridHeight; i++)
                for (j = 0; j < gridWidth; j++)
                {
                    squaresMatrix[i, j] = new Rectangle();
                    this.Children.Add(squaresMatrix[i, j]);
                    squaresMatrix[i, j].SetValue(Grid.RowProperty, i);
                    squaresMatrix[i, j].SetValue(Grid.ColumnProperty, j);
                    squaresMatrix[i, j].Width = squareSize;
                    squaresMatrix[i, j].Height = squareSize;
                    squaresMatrix[i, j].Margin = new Thickness(thick, thick, thick, thick);
                }

            this.Background = new SolidColorBrush(Colors.Transparent);
        }

        public int[] getPicSize() { return (new int[2] { gridHeight, gridWidth }); }
    }
}
