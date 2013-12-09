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
using System.Diagnostics;

namespace Tetris.GameGUI
{
    /// <summary>
    /// GameGrid.xaml 的交互逻辑
    /// </summary>
    public partial class GameGrid : Grid, IDisplay
    {
        private Rectangle[,] rect;
        private const int _square = 30; // size of display square
        readonly Color[] colors = {Colors.Transparent, Colors.DarkOrange, Colors.Red, Colors.Blue, Colors.Green, Colors.Aquamarine, Colors.Olive, Colors.Violet, Colors.Black};
        // The last color is for non-defined color
        public GameGrid(int height, int width)
        {
            InitializeComponent();
            Height = _square * height;
            Width = _square * width;
            ColumnDefinition[] col = new ColumnDefinition[width];
            RowDefinition[] row = new RowDefinition[height];
            GridLength gl = new GridLength(_square, GridUnitType.Pixel);
            int i, j;
            for (i = 0; i < width; i++)
            {
                col[i] = new ColumnDefinition();
                col[i].Width = gl;
                this.ColumnDefinitions.Add(col[i]);
            }
            for (i = 0; i < height; i++)
            {
                row[i] = new RowDefinition();
                row[i].Height = gl;
                this.RowDefinitions.Add(row[i]);
            }
            this.ShowGridLines = false;
            this.Background = new SolidColorBrush(Color.FromArgb(255, 221, 255, 255));
            rect = new Rectangle[height, width];
            for (i = 0; i < height; i++)
                for (j = 0; j < width; j++)
                {
                    rect[i, j] = new Rectangle();
                    rect[i, j].Width = _square - 1;
                    rect[i, j].Height = _square - 1;
                    rect[i, j].SetValue(Grid.RowProperty, i);
                    rect[i, j].SetValue(Grid.ColumnProperty, j);
                    rect[i, j].Fill = new SolidColorBrush(colors[0]);
                    this.Children.Add(rect[i, j]);
                }
        }

        public void OnDrawing(TetrisGame game, TetrisGame.DrawEventArgs e)
        {
            // Repaint all ( May be ineffient --> Only repaint the region of block? )
            Square[,] image = game.Image;
            int i, j;
            
            for (i = 0; i < game.Height; i++)
                for (j = 0; j < game.Width; j++)
                {
                    try
                    {
                        rect[i, j].Dispatcher.Invoke(
                            new Action(
                                delegate
                                {
                                    rect[i, j].Fill = new SolidColorBrush(colors[image[i, j] == null ? 0 : (image[i, j].Color < colors.Length ? image[i, j].Color : colors.Length - 1)]);
                                    //Trace.WriteLine(String.Format("{0}, {1}: {2}", i, j, image[i, j] == null ? 0 : image[i, j].Color));
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
