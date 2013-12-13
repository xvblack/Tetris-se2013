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

namespace Tetris.AdvancedGUI
{
    /// <summary>
    /// SingleModePage.xaml 的交互逻辑
    /// </summary>
    public partial class SingleModePage : Page
    {
        public Window holderWin {get; set;}

        public SingleModePage()
        {
            InitializeComponent();
            int[] gridSize = new int[2] { 10, 10 };

            Frame aFrame = new Frame();
            outerGrid.Children.Add(aFrame);
            aFrame.SetValue(Grid.RowProperty, 1);
            aFrame.SetValue(Grid.ColumnProperty, 2);

            GamePage gamePage = new GamePage(gridSize);
            aFrame.Resources.Add(Guid.NewGuid(), gamePage);
            aFrame.Navigate(gamePage);
        }
        private void changeSize(object sender, RoutedEventArgs e) {
        }
    }
}
