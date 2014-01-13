using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Tetris.GameBase
{
    /// <summary>
    /// 使用Sleep实现的简单引擎
    /// </summary>
    class SimpleEngine : IEngine, INotifyPropertyChanged
    {
        public double Interval { set; private get; }
        public event TickHandler TickEvent;
        private bool _enabled;
        private Thread thread;
        public bool Enabled {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                if (_enabled)
                {
                    thread.Start();
                }
                else
                {
                    thread.Abort();
                }
            }
        }

        public int Fps { get; private set; }

        public SimpleEngine()
        {
            _enabled = false;
            Interval = 1;
            thread = new Thread(delegate()
            {
                while (true)
                {
                    if (_enabled)
                    {
                        TickEvent.Invoke(this, 0);
                    }
                    Thread.Sleep((int)(Interval * 1000)); // 至少等待Interval秒
                }
            });
            var time = DateTime.Now;
            TickEvent += (sender, tick) =>
            {
                var newtime = DateTime.Now;
                //Trace.WriteLine(newtime-time); // 测试函数，显示间隔
#if DEBUG
                Fps = (int)(1000/(newtime - time).TotalMilliseconds);
                Notify("Fps");
#endif
                time = newtime;
                Trace.Flush();
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,new PropertyChangedEventArgs(propName));
            }
        }
    }
}
