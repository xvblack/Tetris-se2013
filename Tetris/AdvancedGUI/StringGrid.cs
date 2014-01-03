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
using Tetris.AdvancedGUI.Styles;
using System.Windows.Media.Effects;

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// set up a string, built by multiple pixels; poor design
    /// </summary>
    public partial class StringGrid : ContentGridGenerator
    {

        Dictionary<char, int[,]> fontLibrary = new Dictionary<char,int[,]>()
        {
             
            { 
                'A',                 
                new int[,]{{0, 0, 0, 0, 1, 0, 0, 0, 0},
                           {0, 0, 0, 1, 0, 1, 0, 0, 0},
                           {0, 0, 1, 0, 0, 0, 1, 0, 0},
                           {0, 1, 0, 0, 0, 0, 0, 1, 0},
                           {0, 1, 0, 0, 0, 0, 0, 1, 0},
                           {0, 1, 1, 1, 1, 1, 1, 1, 0},
                           {0, 1, 0, 0, 0, 0, 0, 1, 0},
                           {0, 1, 0, 0, 0, 0, 0, 1, 0},
                           {0, 1, 0, 0, 0, 0, 0, 1, 0}}
             },           
            {
                'D',
                new int[,]{{0, 1, 1, 1, 1, 1, 0, 0, 0},
                           {0, 1, 0, 0, 0, 0, 1, 0, 0},
                           {0, 1, 0, 0, 0, 0, 0, 1, 0},
                           {0, 1, 0, 0, 0, 0, 0, 1, 0},
                           {0, 1, 0, 0, 0, 0, 0, 1, 0},
                           {0, 1, 0, 0, 0, 0, 0, 1, 0},
                           {0, 1, 0, 0, 0, 0, 0, 1, 0},
                           {0, 1, 0, 0, 0, 0, 1, 0, 0},
                           {0, 1, 1, 1, 1, 1, 0, 0, 0}}
            },
            {
                'R',
                new int[,]{{0, 1, 1, 1, 1, 1, 0, 0, 0},
                           {0, 1, 0, 0, 0, 0, 1, 0, 0},
                           {0, 1, 0, 0, 0, 0, 1, 0, 0},
                           {0, 1, 0, 0, 0, 0, 1, 0, 0},
                           {0, 1, 1, 1, 1, 1, 0, 0, 0},
                           {0, 1, 0, 0, 1, 0, 0, 0, 0},
                           {0, 1, 0, 0, 0, 1, 0, 0, 0},
                           {0, 1, 0, 0, 0, 0, 1, 0, 0},
                           {0, 1, 0, 0, 0, 0, 0, 1, 0}}
            },
            {
                'E',
                new int[,]{{0, 1, 1, 1, 1, 1, 1, 1, 0},
                           {0, 1, 0, 0, 0, 0, 0, 0, 0},
                           {0, 1, 0, 0, 0, 0, 0, 0, 0},
                           {0, 1, 0, 0, 0, 0, 0, 0, 0},
                           {0, 1, 1, 1, 1, 1, 1, 1, 0},
                           {0, 1, 0, 0, 0, 0, 0, 0, 0},
                           {0, 1, 0, 0, 0, 0, 0, 0, 0},
                           {0, 1, 0, 0, 0, 0, 0, 0, 0},
                           {0, 1, 1, 1, 1, 1, 1, 1, 0}}
            },                      
            {
                'Y',
                new int[,]{{1, 0, 0, 0, 0, 0, 0, 0, 1},
                           {0, 1, 0, 0, 0, 0, 0, 1, 0},
                           {0, 0, 1, 0, 0, 0, 1, 0, 0},
                           {0, 0, 0, 1, 0, 1, 0, 0, 0},
                           {0, 0, 0, 0, 1, 0, 0, 0, 0},
                           {0, 0, 0, 0, 1, 0, 0, 0, 0},
                           {0, 0, 0, 0, 1, 0, 0, 0, 0},
                           {0, 0, 0, 0, 1, 0, 0, 0, 0},
                           {0, 0, 0, 0, 1, 0, 0, 0, 0}}
           },
           {
               'G',
               new int[,]{{0, 0, 0, 1, 1, 1, 1, 0, 0},
                          {0, 0, 1, 0, 0, 0, 0, 1, 0},
                          {0, 1, 0, 0, 0, 0, 0, 1, 0},
                          {0, 1, 0, 0, 0, 0, 0, 0, 0},
                          {0, 1, 0, 0, 0, 1, 1, 1, 0},
                          {0, 1, 0, 0, 0, 0, 0, 1, 0},
                          {0, 1, 0, 0, 0, 0, 0, 1, 0},
                          {0, 0, 1, 0, 0, 0, 0, 1, 0},
                          {0, 0, 0, 1, 1, 1, 1, 0, 0}}
           },
           {
               'O',
               new int[,]{{0, 0, 0, 1, 1, 1, 0, 0, 0},
                          {0, 0, 1, 0, 0, 0, 1, 0, 0},
                          {0, 1, 0, 0, 0, 0, 0, 1, 0},
                          {0, 1, 0, 0, 0, 0, 0, 1, 0},
                          {0, 1, 0, 0, 0, 0, 0, 1, 0},
                          {0, 1, 0, 0, 0, 0, 0, 1, 0},
                          {0, 1, 0, 0, 0, 0, 0, 1, 0},
                          {0, 0, 1, 0, 0, 0, 1, 0, 0},
                          {0, 0, 0, 1, 1, 1, 0, 0, 0}}
           },
           {
               'T',
               new int[,]{{1,1,1,1,1,1,1},
                          {0,0,0,1,0,0,0},
                          {0,0,0,1,0,0,0},
                          {0,0,0,1,0,0,0},
                          {0,0,0,1,0,0,0},
                          {0,0,0,1,0,0,0},
                          {0,0,0,1,0,0,0},
                          {0,0,0,1,0,0,0},
                          {0,0,0,1,0,0,0}}
           },
            {
                't',
                new int[,]{{0, 0, 1, 0, 0, 0},
                           {0, 0, 1, 0, 0, 0},
                           {0, 1, 1, 1, 1, 0},
                           {0, 0, 1, 0, 0, 0},
                           {0, 0, 1, 0, 0, 0},
                           {0, 0, 1, 0, 0, 0},
                           {0, 0, 1, 0, 0, 0},
                           {0, 0, 1, 0, 0, 0},
                           {0, 0, 1, 1, 1, 0}}
            },
            {
                'e',
                new int[,]{
                           {0, 0, 0, 0, 0, 0,0},
                           {0, 0, 0, 0, 0, 0,0},
                           {0, 1, 1, 1, 1, 0,0},
                           {1, 0, 0, 0, 0, 1,0},
                           {1, 0, 0, 0, 0, 1,0},
                           {1, 1, 1, 1, 1, 1,0},
                           {1, 0, 0, 0, 0, 0,0},
                           {1, 0, 0, 0, 0, 0,0},
                           {0, 1, 1, 1, 1, 0,0}}
            },
            {
                'r',
                new int[,]{{0,0, 0, 0, 0,0},
                           {0,0, 0, 0, 0,0},
                           {0,1, 0, 1, 1,0},
                           {0,1, 1, 0, 0,0},
                           {0,1, 0, 0, 0,0},
                           {0,1, 0, 0, 0,0},
                           {0,1, 0, 0, 0,0},
                           {0,1, 0, 0, 0,0},
                           {0,1, 0, 0, 0,0}}
            },
            {
                'i',
                new int[,]{
                           {0, 0, 0},
                           {0, 1, 0},
                           {0, 0, 0},
                           {0, 1, 0},
                           {0, 1, 0},
                           {0, 1, 0},
                           {0, 1, 0},
                           {0, 1, 0},
                           {0, 1, 0}}
            },
            {
                's',
                new int[,]{
                           {0,0, 0, 0, 0, 0},
                           {0,0, 0, 0, 0, 0},
                           {0,0, 1, 1, 1, 0},
                           {0,1, 0, 0, 0, 0},
                           {0,1, 0, 0, 0, 0},
                           {0,0, 1, 1, 0, 0},
                           {0,0, 0, 0, 1, 0},
                           {0,0, 0, 0, 1, 0},
                           {0,1, 1, 1, 1, 0}}
            },
            {
                'v',
                new int[,]{
                    	   {0, 0, 0, 0, 0, 0, 0},
                           {0, 0, 0, 0, 0, 0, 0},
                           {0, 1, 0, 0, 0, 1, 0},
                           {0, 1, 0, 0, 0, 1, 0},
                           {0, 1, 0, 0, 0, 1, 0},
                           {0, 1, 0, 0, 0, 1, 0},
                           {0, 0, 1, 0, 1, 0, 0},
                           {0, 0, 1, 0, 1, 0, 0},
                           {0, 0, 0, 1, 0, 0, 0}}
           },
           {
                'a',
                new int[,]{
                           {0, 0, 0, 0, 0, 0, 0},
                           {0, 0, 0, 0, 0, 0, 0},
                           {0, 1, 1, 1, 1, 0, 0},
                           {0, 0, 0, 0, 0, 1, 0},
                           {0, 0, 0, 0, 0, 1, 0},
                           {0, 1, 1, 1, 1, 1, 0},
                           {0, 1, 0, 0, 0, 1, 0},
                           {0, 1, 0, 0, 0, 1, 0},
                           {0, 1, 1, 1, 1, 1, 0}
                }
          },
          {
                'm',
                new int[,]{
                           {0,0, 0, 0, 0, 0, 0, 0,0},
                           {0,0, 0, 0, 0, 0, 0, 0,0},
                           {0,0, 1, 1, 0, 1, 1, 0,0},
                           {0,1, 0, 0, 1, 0, 0, 1,0},
                           {0,1, 0, 0, 1, 0, 0, 1,0},
                           {0,1, 0, 0, 1, 0, 0, 1,0},
                           {0,1, 0, 0, 1, 0, 0, 1,0},
                           {0,1, 0, 0, 1, 0, 0, 1,0},
                           {0,1, 0, 0, 1, 0, 0, 1,0}
                }
           },
          {
                ' ',
                new int[,]{
                           {0, 0, 0},
                           {0, 0, 0},
                           {0, 0, 0},
                           {0, 0, 0},
                           {0, 0, 0},
                           {0, 0, 0},
                           {0, 0, 0},
                           {0, 0, 0},
                           {0, 0, 0}}
        }
          

        };

        int[,] contentMap;
        ColorAnimationUsingKeyFrames[,] ca ;
        public Storyboard story;
        public bool pauseState = false;

        public StringGrid(String content, double size)
        {
            gridHeight = 9;
            story = new Storyboard();
            contentMap = getContentString(content);
            initializeGrid(gridHeight, gridWidth, size, 1);
        }
            

        public double getWidth()
        {
            return (squareSize * gridWidth);

        }

        public double getHeight()
        {
            return (squareSize * gridHeight);
        }

        // set up the string with no animation
        public void noAnimation()
        {
            Random num = new Random();
            brushesMatrix = new SolidColorBrush[gridHeight, gridWidth];

            for (int i = 0; i < gridHeight; i++)
            {
                for (int j = 0; j < gridWidth; j++)
                {

                    Color fromColor = colorMap[0];
                    Color toColor = colorMap[num.Next(5) + 1];

                    if (contentMap[i, j] == 1)
                    {
                        squaresMatrix[i, j].Dispatcher.Invoke(
                            new Action(
                                delegate
                                {
                                    squaresMatrix[i, j].Fill = new SolidColorBrush(toColor);
                                    squaresMatrix[i, j].Stroke = new SolidColorBrush(Colors.Gray);
                                    squaresMatrix[i, j].StrokeThickness = 1;
                                }));
                            
                    }
                    else
                    {
                        brushesMatrix[i, j] = new SolidColorBrush(fromColor);
                    }
                }

            }
        }

        // pause the animation when game paused
        public void pauseAnimation()
        {
            story.Pause();
        }

        // set up the string with animation
        public void startAnimation(int timeStep, int timeDelay)
        {
            Random num = new Random();
            int beginTime = 0;
            NameScope.SetNameScope(this, new NameScope());
            ca = new ColorAnimationUsingKeyFrames[gridHeight, gridWidth];
            brushesMatrix = new SolidColorBrush[gridHeight, gridWidth];

            for (int i = 0; i < gridHeight; i++)
            {
                for (int j = 0; j < gridWidth; j++)
                {

                    Color fromColor = colorMap[0];
                    Color toColor = colorMap[num.Next(5) + 1];
                    Color strokeColor = Colors.Gray;

                    beginTime = num.Next(300);
                    
                    if (contentMap[i, j] == 1)
                    {
                        //story = new Storyboard();
                        ca[i, j] = new ColorAnimationUsingKeyFrames();
                        brushesMatrix[i, j] = new SolidColorBrush();
                        SolidColorBrush b = new SolidColorBrush();

                        // animation for square content
                        ca[i, j].KeyFrames.Add(new SplineColorKeyFrame(fromColor,
                            TimeSpan.FromMilliseconds(beginTime + timeDelay)));
                        ca[i, j].KeyFrames.Add(new SplineColorKeyFrame(toColor,
                            TimeSpan.FromMilliseconds(beginTime + timeDelay)));
                        ca[i, j].KeyFrames.Add(new SplineColorKeyFrame(toColor,
                            TimeSpan.FromMilliseconds(timeStep + beginTime + timeDelay)));
                        ca[i, j].KeyFrames.Add(new SplineColorKeyFrame(fromColor,
                            TimeSpan.FromMilliseconds(timeStep + beginTime + timeDelay)));

                        story.Children.Add(ca[i, j]);
                            
                        String name = "brush" + i.ToString() + '_' + j.ToString();
                        this.RegisterName(name, brushesMatrix[i, j]);

                        Storyboard.SetTargetName(ca[i, j], name);
                        Storyboard.SetTargetProperty(ca[i, j],
                            new PropertyPath("Color"));

                        squaresMatrix[i, j].Fill = brushesMatrix[i, j];

                        ColorAnimationUsingKeyFrames ca_stroke =
                            new ColorAnimationUsingKeyFrames();

                        // animation for square border
                        ca_stroke.KeyFrames.Add(new SplineColorKeyFrame(fromColor,
                            TimeSpan.FromMilliseconds(beginTime + timeDelay)));
                        ca_stroke.KeyFrames.Add(new SplineColorKeyFrame(strokeColor,
                            TimeSpan.FromMilliseconds(beginTime + timeDelay)));
                        ca_stroke.KeyFrames.Add(new SplineColorKeyFrame(strokeColor,
                            TimeSpan.FromMilliseconds(timeStep + beginTime + timeDelay)));
                        ca_stroke.KeyFrames.Add(new SplineColorKeyFrame(fromColor,
                            TimeSpan.FromMilliseconds(timeStep + beginTime + timeDelay)));

                        name = "stroke" + i.ToString() + '_' + j.ToString();
                        SolidColorBrush stroke = new SolidColorBrush();
                        squaresMatrix[i, j].Stroke = stroke;
                        squaresMatrix[i, j].StrokeThickness = 1;
                        
                        this.RegisterName(name, stroke);

                        story.Children.Add(ca_stroke);

                        Storyboard.SetTargetName(ca_stroke, name);
                        Storyboard.SetTargetProperty(ca_stroke,
                            new PropertyPath("Color"));
                    }
                }

            }
        }

        // start the animation
        public void beginAnimation() { 
            story.Dispatcher.Invoke(
                new Action(
                    delegate
                    {
                        story.Begin(this, true);
                    }));
        }

        // get the fonts from a string.
        private int[,] getContentString(String s)
        {
            char[] content = s.ToCharArray();

            gridWidth = 0;
            for (int n = 0; n < content.Length; n++)
            {
                int[,] font = fontLibrary[content[n]];
                gridWidth += font.GetLength(1);
            }
            int[,] contentMap = new int[gridHeight, gridWidth];
            int index = 0;
            for (int n = 0; n < content.Length; n++)
            {
                int[,] font = fontLibrary[content[n]];
                for (int i = 0; i < gridHeight; i++)
                    for (int j = 0; j < font.GetLength(1); j++)
                        contentMap[i, j + index] = font[i, j];
                index += font.GetLength(1);
            }
            return contentMap;
        }

    }
}
