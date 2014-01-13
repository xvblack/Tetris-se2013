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
using System.Windows.Media.Animation;
using Tetris.GameBase;

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// the grid holding the display of the game
    /// </summary>
    public partial class GameGrid : ContentGridGenerator, IDisplay
    {
        private int[,] imageCache = null;

        private Brush[] brushesMap = SquareGenerator.brushesMap;

        public GameGrid(int[] gridSize)
        {
            initializeGrid(gridSize[0], gridSize[1], SquareGenerator.squareSize, 1);
            this.Background = new SolidColorBrush(Colors.White);
        }

        public double[] getGameGridSize()
        {
            double height = (gridHeight) * (2 + squareSize);
            double width = (gridWidth) * (2 + squareSize);
            return (new double[2] { height, width });
        }

        public void gameEndEffect(object sender, Tetris.GameBase.TetrisGame.GameEndEventArgs e)
        {
            this.Dispatcher.Invoke(
                new Action(
                    delegate
                    {
                        DoubleAnimation d = new DoubleAnimation();
                        d.From = 255;
                        d.To = 1;
                        d.Duration = new Duration(TimeSpan.FromMilliseconds(2000));
                        this.BeginAnimation(Grid.OpacityProperty, d);
                    }
                )
            );
        }

        // used to draw the game
        public void OnDrawing(Tetris.GameBase.TetrisGame game,
            Tetris.GameBase.TetrisGame.DrawEventArgs e)
        {
            Square[,] image = game.Image;
            if (imageCache == null)
            {
                imageCache = new int[image.GetUpperBound(0) + 1, image.GetUpperBound(1) + 1];
                Array.Clear(imageCache, 0, (image.GetUpperBound(0) + 1) * (image.GetUpperBound(1) + 1));
            }
            int i, j;

            for (i = 0; i < game.Height; i++)
                for (j = 0; j < game.Width; j++)
                {
                    // only draw the changed part
                    if ((image[i, j] == null && imageCache[i, j] != 0) || (image[i, j] != null && image[i, j].Color+image[i,j].SubId != imageCache[i, j]))
                    {

                        imageCache[i, j] = image[i, j] == null ? 0 : image[i, j].Color + image[i,j].SubId;
                        try
                        {
                            squaresMatrix[i, j].Dispatcher.Invoke(
                                new Action(
                                    delegate
                                    {
                                        /*
                                        squaresMatrix[i, j].Fill =
                                            new SolidColorBrush(
                                                colorMap[
                                                    image[i, j] == null
                                                        ? 0
                                                        : (image[i, j].Color < colorMap.Length
                                                            ? image[i, j].Color
                                                            : colorMap.Length - 1)]);
                                        */
                                        squaresMatrix[i, j].Fill = SquareGenerator.brushClone(
                                            brushesMap[
                                                image[i, j] == null
                                                    ? 0
                                                        : (image[i, j].Color + image[i, j].SubId 
                                                                            < brushesMap.Length
                                                            ? image[i, j].Color + +image[i, j].SubId
                                                            : brushesMap.Length - 1)]);
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
