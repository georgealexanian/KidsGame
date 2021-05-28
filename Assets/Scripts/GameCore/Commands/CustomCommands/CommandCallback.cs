using System;

namespace GameCore.Commands.CustomCommands
{
    public class CommandCallback : ICommand
    {
        public Action OnFinished { get; set; }

        private Action _callback;
    
        public CommandCallback(Action callback)
        {
            _callback = callback;
        }
    
        public void Execute()
        {
            _callback?.Invoke();
            OnFinished.Invoke();
        }
    }
}
