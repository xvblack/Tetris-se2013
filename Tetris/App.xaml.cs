using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Tetris.GameSystem;

namespace Tetris
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        [DllImport("Kernel32.dll")]
        private static extern bool AllocConsole();
        protected override void OnStartup(StartupEventArgs e)
        {
            AllocConsole();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            AchievementSystem.Save();
            base.OnExit(e);
        }
    }
}
