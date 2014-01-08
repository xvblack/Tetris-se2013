using System;
using System.Windows.Input;

namespace Tetris.GameControl
{
    public interface IPlayerController : IController
    {
        ControllerConfig GetConfig();
        void SetConfig(ControllerConfig config);
        void OnKeyDown(KeyEventArgs e);
        void OnKeyUp(KeyEventArgs e);
    }
}