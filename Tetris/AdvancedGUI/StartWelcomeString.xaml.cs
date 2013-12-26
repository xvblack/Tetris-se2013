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

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// StartWelcomeString.xaml 的交互逻辑
    /// </summary>
    public partial class StartWelcomeString : Grid
    {
        
        private Dictionary<char, int[,]> fontLibrary;  // store the bit maps of capital letters
        const int fontHeight = 9;
        const int fontWidth = 9;
        private SolidColorBrush[,] brush;
        private int gridLen;
        private Color[] colorMap = (new Styles.SquareGenerator()).getColorMap();
        int[,] contentMap;
        private Rectangle[,] rect;

        public StartWelcomeString(String content)
        {   
            InitializeComponent();
            this.setFontLibrary();

            contentMap  = getWelcomeString(content);

            gridLen = content.Length * fontWidth;

            GridLength gl = new GridLength(0, GridUnitType.Auto);

            int i = 0;
            int j = 0;
            for (i = 0; i < gridLen; i++)
            {
                ColumnDefinition aCol = new ColumnDefinition();
                aCol.Width = gl;
                this.ColumnDefinitions.Add(aCol);
            }
            for (j = 0; j < fontHeight; j++)
            {
                RowDefinition aRow = new RowDefinition();
                aRow.Height = gl;
                this.RowDefinitions.Add(aRow);
            }
            
            int _squareSize = Styles.SquareGenerator.squareSize();
            rect = new Rectangle[fontHeight, gridLen];
            brush = new SolidColorBrush[fontHeight, gridLen];
            for (i = 0; i < fontHeight; i++)
                for (j = 0; j < gridLen; j++)
                {
                    rect[i, j] = new Rectangle();
                    
                    /*
                    Border border = new Border();
                    border.BorderThickness = new Thickness(0.5, 0.5, 0.5, 0.5);
                    border.Child = rect[i, j];

                    this.Children.Add(border);
                    border.SetValue(Grid.RowProperty, i);
                    border.SetValue(Grid.ColumnProperty, j);
                    */

                    this.Children.Add(rect[i,j]);
                    rect[i, j].SetValue(Grid.RowProperty, i);
                    rect[i, j].SetValue(Grid.ColumnProperty, j);

                    rect[i, j].Width = _squareSize / 2;
                    rect[i, j].Height = _squareSize / 2;
                    rect[i, j].Margin = new Thickness(1, 1, 1, 1);

                    rect[i, j].Stroke = new SolidColorBrush(Colors.Transparent);

                    if (contentMap[i, j] == 1) {
                       // border.BorderBrush = new SolidColorBrush(Colors.Black);
                        brush[i, j] = new SolidColorBrush();
                        rect[i,j].Fill = brush[i, j];
                    }
                }
        }

        public double getWidth() {
            return ((rect[0, 0].Width+1) * gridLen);

        }

        public double getHeight() {
            return ((rect[0, 0].Height+1) * fontHeight);
        }

        public void startAnimation(int timeStep, int timeDelay) {
            Random num = new Random();
            int beginTime = 0;
            ColorAnimationUsingKeyFrames[,] ca = new ColorAnimationUsingKeyFrames[fontHeight, gridLen];

            for (int i = 0; i < fontHeight; i++)
            {
                for (int j = 0; j < gridLen; j++) {
                    
                    Color fromColor = colorMap[0];
                    Color toColor = colorMap[num.Next(5) + 1];
                    Color strokeColor = Colors.Gray;

                    beginTime = num.Next(300);
                 
                    if (contentMap[i, j] == 1)
                    {
                        ca[i, j] = new ColorAnimationUsingKeyFrames();

                        ca[i, j].KeyFrames.Add(new SplineColorKeyFrame(fromColor,
                            TimeSpan.FromMilliseconds(beginTime + timeDelay)));
                        ca[i, j].KeyFrames.Add(new SplineColorKeyFrame(toColor,
                            TimeSpan.FromMilliseconds(beginTime + timeDelay)));
                        ca[i, j].KeyFrames.Add(new SplineColorKeyFrame(toColor,
                            TimeSpan.FromMilliseconds(timeStep + beginTime + timeDelay)));
                        ca[i, j].KeyFrames.Add(new SplineColorKeyFrame(fromColor,
                            TimeSpan.FromMilliseconds(timeStep + beginTime + timeDelay)));

                        brush[i, j].BeginAnimation(SolidColorBrush.ColorProperty, ca[i, j]);

                        ColorAnimationUsingKeyFrames ca_stroke = 
                            new ColorAnimationUsingKeyFrames();

                        ca_stroke.KeyFrames.Add(new SplineColorKeyFrame(fromColor,
                            TimeSpan.FromMilliseconds(beginTime + timeDelay)));
                        ca_stroke.KeyFrames.Add(new SplineColorKeyFrame(strokeColor,
                            TimeSpan.FromMilliseconds(beginTime + timeDelay)));
                        ca_stroke.KeyFrames.Add(new SplineColorKeyFrame(strokeColor,
                            TimeSpan.FromMilliseconds(timeStep + beginTime + timeDelay)));
                        ca_stroke.KeyFrames.Add(new SplineColorKeyFrame(fromColor,
                            TimeSpan.FromMilliseconds(timeStep + beginTime + timeDelay)));

                        rect[i, j].Stroke.BeginAnimation(SolidColorBrush.ColorProperty, ca_stroke);
                    }
                }
            }
        }

        private int[,] getWelcomeString(String s) {
            char[] content = s.ToCharArray();
            int[,] contentMap = new int[fontHeight, fontWidth * content.Length];

            int[,] font = new int[fontHeight, fontWidth];
            for (int n = 0; n < content.Length; n++)
            {
                font = fontLibrary[content[n]];
                for (int i = 0; i < fontHeight; i++)
                {
                    for (int j = 0; j < fontWidth; j++)
                    {
                        contentMap[i, j + n * fontWidth] = font[i, j];
                    }
                }
            }
         /* // test code
            for (int i = 0; i < fontHeight; i++)
            {
                for (int j = 0; j < content.Length * fontWidth; j++)
                {
                    if (contentMap[i, j] == 0)
                        Console.Write(' ');
                    else
                        Console.Write('#');

                }
                Console.Write('\n');
            }
            */
            return contentMap;
        }

        private void setFontLibrary() {
            fontLibrary = new Dictionary<char,int[,]>();
            int[,] aFont;
            aFont = new int[fontHeight, fontWidth]{{0, 0, 0, 0, 1, 0, 0, 0, 0},
                                                   {0, 0, 0, 1, 0, 1, 0, 0, 0},
                                                   {0, 0, 1, 0, 0, 0, 1, 0, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 1, 1, 1, 1, 1, 1, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0}};
            fontLibrary.Add('A', aFont);

            aFont = new int[fontHeight, fontWidth]{{0, 1, 1, 1, 1, 1, 0, 0, 0},
                                                   {0, 1, 0, 0, 0, 0, 1, 0, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 1, 0, 0},
                                                   {0, 1, 1, 1, 1, 1, 0, 0, 0}};
            fontLibrary.Add('D', aFont);

            aFont = new int[fontHeight, fontWidth]{{0, 1, 1, 1, 1, 1, 0, 0, 0},
                                                   {0, 1, 0, 0, 0, 0, 1, 0, 0},
                                                   {0, 1, 0, 0, 0, 0, 1, 0, 0},
                                                   {0, 1, 0, 0, 0, 0, 1, 0, 0},
                                                   {0, 1, 1, 1, 1, 1, 0, 0, 0},
                                                   {0, 1, 0, 0, 1, 0, 0, 0, 0},
                                                   {0, 1, 0, 0, 0, 1, 0, 0, 0},
                                                   {0, 1, 0, 0, 0, 0, 1, 0, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0}};
            fontLibrary.Add('R', aFont);

            aFont = new int[fontHeight, fontWidth]{{0, 1, 1, 1, 1, 1, 1, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 0, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 0, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 0, 0},
                                                   {0, 1, 1, 1, 1, 1, 1, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 0, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 0, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 0, 0},
                                                   {0, 1, 1, 1, 1, 1, 1, 1, 0}};
            fontLibrary.Add('E', aFont);

            aFont = new int[fontHeight, fontWidth]{{1, 0, 0, 0, 0, 0, 0, 0, 1},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 0, 1, 0, 0, 0, 1, 0, 0},
                                                   {0, 0, 0, 1, 0, 1, 0, 0, 0},
                                                   {0, 0, 0, 0, 1, 0, 0, 0, 0},
                                                   {0, 0, 0, 0, 1, 0, 0, 0, 0},
                                                   {0, 0, 0, 0, 1, 0, 0, 0, 0},
                                                   {0, 0, 0, 0, 1, 0, 0, 0, 0},
                                                   {0, 0, 0, 0, 1, 0, 0, 0, 0}};
            fontLibrary.Add('Y', aFont);

            aFont = new int[fontHeight, fontWidth]{{0, 0, 0, 1, 1, 1, 1, 0, 0},
                                                   {0, 0, 1, 0, 0, 0, 0, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 0, 0},
                                                   {0, 1, 0, 0, 0, 1, 1, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 0, 1, 0, 0, 0, 0, 1, 0},
                                                   {0, 0, 0, 1, 1, 1, 1, 0, 0}};
            fontLibrary.Add('G', aFont);

            aFont = new int[fontHeight, fontWidth]{{0, 0, 0, 1, 1, 1, 0, 0, 0},
                                                   {0, 0, 1, 0, 0, 0, 1, 0, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 1, 0, 0, 0, 0, 0, 1, 0},
                                                   {0, 0, 1, 0, 0, 0, 1, 0, 0},
                                                   {0, 0, 0, 1, 1, 1, 0, 0, 0}};
            fontLibrary.Add('O', aFont);

            /* // test code
            foreach (KeyValuePair<char, int[,]>temp in fontLibrary){
            for (int i = 0; i < mapHeight; i++) {
                for (int j = 0; j < mapWidth; j++)
                {
                    if (temp.Value[i, j] == 0)
                        Console.Write(' ');
                    else
                        Console.Write('#');

                }
                Console.Write('\n');
            }
            Console.Write('\n');
            }
            */
        }
    }
}
