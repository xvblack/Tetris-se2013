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
using Tetris.GameControl;
using Tetris.GameBase;

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// Setting Page
    /// </summary>
    public partial class SettingPage : Page
    {
        NavigationService nav;
        public MainWindow holderWin;

        ControllerConfig config1 = new ControllerConfig(Properties.Settings.Default.Player1Path);// key map 1
        ControllerConfig config2 = new ControllerConfig(Properties.Settings.Default.Player2Path);// key map 2

        Dictionary<String, Key> keyDict = new Dictionary<String, Key>(); // used to record which key has been used 
                                                                         // preventing the same key mapped

        Dictionary<String, TetrisGame.GameAction> refDict1 =   // use to bind the option and the true action, player 1
            new Dictionary<string, TetrisGame.GameAction>();
        Dictionary<String, TetrisGame.GameAction> refDict2 =  // player 2
            new Dictionary<string, TetrisGame.GameAction>();

        public SettingPage()
        {
            InitializeComponent();

            this.FontSize = WindowSizeGenerator.fontSizeMedium;
            p1l.FontSize = WindowSizeGenerator.fontSizeLarge;
            p2l.FontSize = WindowSizeGenerator.fontSizeLarge;

            refDict1.Add(down1.Name, TetrisGame.GameAction.Down);
            refDict1.Add(left1.Name, TetrisGame.GameAction.Left);
            refDict1.Add(right1.Name, TetrisGame.GameAction.Right);
            refDict1.Add(rotate1.Name, TetrisGame.GameAction.Rotate);
            refDict1.Add(stop1.Name, TetrisGame.GameAction.Pause);

            refDict2.Add(down2.Name, TetrisGame.GameAction.Down);
            refDict2.Add(left2.Name, TetrisGame.GameAction.Left);
            refDict2.Add(right2.Name, TetrisGame.GameAction.Right);
            refDict2.Add(rotate2.Name, TetrisGame.GameAction.Rotate);
            refDict2.Add(stop2.Name, TetrisGame.GameAction.Pause);

            initializeMap(down1, refDict1, config1); // bind the option, the key and the action
            initializeMap(left1, refDict1, config1);
            initializeMap(right1, refDict1, config1);
            initializeMap(rotate1, refDict1,config1);
            initializeMap(stop1, refDict1, config1);

            initializeMap(down2, refDict2, config2); 
            initializeMap(left2, refDict2, config2);
            initializeMap(right2, refDict2, config2);
            initializeMap(rotate2, refDict2, config2);
            initializeMap(stop2, refDict2, config2);

            CustomizedButton2 okayButton =
                new CustomizedButton2("确  定", SquareGenerator.colorMap[3]);
            okayButton.button.Click += okay_Click;
            okayButton.SetValue(Grid.RowProperty, 3);
            okayButton.SetValue(Grid.ColumnProperty, 1);
            okayButton.button.Focusable = false;
            
            CustomizedButton2 backButton =
                new CustomizedButton2("后  退", SquareGenerator.colorMap[5]);
            backButton.button.Click += back_Click;
            backButton.SetValue(Grid.RowProperty, 3);
            backButton.SetValue(Grid.ColumnProperty, 2);
            backButton.button.Focusable = false;

            contentGrid.Children.Add(backButton);
            contentGrid.Children.Add(okayButton);

            foreach (FrameworkElement i in player1.Children)
            {
                i.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            }
            
            foreach (FrameworkElement i in player2.Children)
            {
                i.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            }


        }

        // switch to next key setup
        private void lostFocusAnimation(object sender, RoutedEventArgs e)
        {
            Button b = (Button)(e.Source);
            b.BeginAnimation(OpacityProperty, null);
        }

        // blink the focused label
        private void focusedAnimation(object sender, RoutedEventArgs e)
        {
            Button b = (Button)(e.Source);
            DoubleAnimation a = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.6));
            a.AutoReverse = true;
            a.RepeatBehavior = RepeatBehavior.Forever;
            b.BeginAnimation(OpacityProperty, a);
        }

        // correspone a pressed key, and show the name of the key
        private void keyPressed(object sender, KeyEventArgs e)
        {
            try
            {
                Button i = (Button)(Keyboard.FocusedElement);
                e.Handled = true;
                if (keyDict.ContainsValue(e.Key))
                {
                    if (keyDict[i.Name] == e.Key) 
                    { 
                        return;
                    }
                }
                // record which key mapped
                keyDict.Remove(i.Name);
                keyDict.Add(i.Name, e.Key);
                i.Content = e.Key;
                i.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                
            }
            catch
            { }
            
        }

        // store the setting and go back to navigation page
        private void okay_Click(object sender, RoutedEventArgs e)
        {
            config1.Clear();
            config1.inversedKeyAndValue.Clear();

            config2.Clear();
            config2.inversedKeyAndValue.Clear();
            
            foreach (String name in refDict1.Keys)
            { 
                if (keyDict.ContainsKey(name))
                {
                    config1.Put(keyDict[name], refDict1[name]);
                }
            }
            foreach (String name in refDict2.Keys)
            {
                if (keyDict.ContainsKey(name))
                {
                    config2.Put(keyDict[name], refDict2[name]);
                }
            }
            config1.Save(Properties.Settings.Default.Player1Path);
            config2.Save(Properties.Settings.Default.Player2Path);

            back_Click(sender, e);
        }

        // Click to go to the last page
        private void back_Click(object sender, RoutedEventArgs e)
        {
            nav = NavigationService.GetNavigationService(this);
            NavigationPage backPage = new NavigationPage();
            backPage.holderWin = holderWin;
            nav.Navigate(backPage);  
        }

        // use to read the infomation of the last key mapping 
        private void initializeMap(Button b, Dictionary<String, TetrisGame.GameAction> dict, ControllerConfig config)
        {
            keyDict[b.Name] = config.inversedKeyAndValue[dict[b.Name]];
            b.Content = keyDict[b.Name]; 
        }
    }
}
