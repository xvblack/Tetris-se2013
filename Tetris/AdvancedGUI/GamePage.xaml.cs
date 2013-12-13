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
    /// singleModeGamePage.xaml 的交互逻辑
    /// </summary>
    public partial class GamePage : Page
    {
        int _height;
        int _width;

        Color[] colorMap;
        GameGrid g;
        
        public GamePage(int pageHeight, int pageWidth)
        {
            InitializeComponent();

            // size of gameGrid
            _height = pageHeight;
            _width = pageWidth;
            colorMap = (new Styles.SquareGenerator()).getColorMap();
            g = new GameGrid(10, 10, colorMap); // grid containing the game
            this.gameGrid.Children.Add(g);

        }

        // Grid to show the game
        private class GameGrid : Grid, IDisplay {

            Rectangle[,] squaresMatrix; // hold the squares
            Color[] colorMap;
            int gridHeight;
            int gridWidth;
            
            // height or width means the maximum number of squares contained
            public GameGrid(int height, int width, Color[] cMap) {
                gridHeight = height;
                gridWidth = width;

                squaresMatrix = new Rectangle[gridHeight, gridWidth];

                // colors used to show squares
                colorMap = cMap;

                // size definitions
                int _squareContainerSize = 
                    Styles.SquareGenerator.squareContainerSize();
                int _squareSize = Styles.SquareGenerator.squareSize();
                GridLength _gridLen = new GridLength(_squareContainerSize,
                    GridUnitType.Pixel);  // square size

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
                    for (j = 0; j < gridWidth; j++) {
                        squaresMatrix[i, j] = new Rectangle();
                        this.Children.Add(squaresMatrix[i, j]);
                        squaresMatrix[i, j].SetValue(Grid.RowProperty, i);
                        squaresMatrix[i, j].SetValue(Grid.ColumnProperty, j);
                        squaresMatrix[i, j].Width = _squareSize;
                        squaresMatrix[i, j].Height = _squareSize;
                    }

                this.ShowGridLines = true;

                this.testSquares();
            }
            public void OnDrawing(Tetris.GameBase.TetrisGame game,
                Tetris.GameBase.TetrisGame.DrawEventArgs e) {
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
                                            //Trace.WriteLine(String.Format("{0}, {1}: {2}", i, j, image[i, j] == null ? 0 : image[i, j].Color));
                                        }
                                ));
                            }
                            catch
                            {
                            }
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

        public void testColors() {
            gameGrid.Children.Clear();
            gameGrid.RowDefinitions.Clear();
            gameGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < colorMap.Length; i++)
            {
                gameGrid.RowDefinitions.Add(new RowDefinition());
                Rectangle rect = new Rectangle();
                gameGrid.Children.Add(rect);
                rect.SetValue(Grid.RowProperty, i);
                rect.Fill = new SolidColorBrush(colorMap[i]);
            }
        }

    }
}
