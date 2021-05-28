using System;
using GameCore.SignalSystem;
using GameCore.UI.WindowSystem;

namespace GameCore.Commands.CustomCommands
{
    public class CommandShowWindow : ICommand
    {
        public Action OnFinished { get; set; }

        private GameManagerReference<IWindowManager> _windowsManager = new GameManagerReference<IWindowManager>();
        private GameManagerReference<ISignalSystem> _signalSystem = new GameManagerReference<ISignalSystem>();

        private readonly string _windowId;
        private readonly object[] _args;
        private bool _executed;
        

        public CommandShowWindow(string windowId, params object[] args)
        {
            _windowId = windowId;
            _args = args;
            _signalSystem.Value.Subscribe<WindowClosedSignal>(OnWindowClosed);
        }

        private void OnWindowClosed(WindowClosedSignal obj)
        {
            if (!_executed)
            {
                return;
            }

            if (obj.windowKey == _windowId)
            {
                OnFinished?.Invoke();
                _signalSystem.Value.Unsubscribe<WindowClosedSignal>(OnWindowClosed);
            }
        }

        public void Execute()
        {
            _executed = true;
            _windowsManager.Value.Open(_windowId, _args);
        }
    }
}