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
        readonly Color[] colors = {Colors.Transparent, Colors.DarkOrange, Colors.Red, Colors.Blue, Colors.Green, Colors.Aquamarine, Colors.Olive, Colors.Violet};

        public GameGrid(int height, int width)
        {
            InitializeComponent();
            Height = 25 * height;
            Width = 25 * width;
            ColumnDefinition[] col = new ColumnDefinition[width];
            RowDefinition[] row = new RowDefinition[height];
            GridLength gl = new GridLength(25, GridUnitType.Pixel);
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
                    rect[i, j].Width = 24;
                    rect[i, j].Height = 24;
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
                                    rect[i, j].Fill = new SolidColorBrush(colors[image[i, j] == null ? 0 : image[i, j].Color]);
                                    //Trace.WriteLine(String.Format("{0}, {1}: {2}", i, j, image[i, j] == null ? 0 : image[i, j].Color));
                                }
                        ));
                    }
                    catch
                    {
                    }
                }
            /*
            if (game.Block == null)
                return;
            Tetris.GameBase.Block block = game.Block;
            for (i = 0; i < block.Height; i++)
                for (j = 0; j < block.Width; j++)
                {
                    if ((block.LPos + i < 0) || (block.LPos + i >= game.Height) || (block.RPos + j < 0) || (block.RPos + j >= game.Width))
                        continue;
                        rect[block.LPos + i, block.RPos + j].Dispatcher.Invoke(
                            new Action(
                                delegate
                                {
                                    rect[block.LPos + i, block.RPos + j].Fill = new SolidColorBrush(colors[block.SquareAt(i, j) == null ? 0 : block.SquareAt(i, j).Color]);
                                    //Trace.WriteLine(String.Format("{0}, {1}", i, j));
                                }
                        ));
           
                }*/
        }
    }
}
