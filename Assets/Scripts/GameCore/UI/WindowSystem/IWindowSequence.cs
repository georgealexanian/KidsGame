using System;

namespace GameCore.UI.WindowSystem
{
    public interface IWindowSequence
    {
        IWindowSequence AddWindow(string windowKey, params object[] args);
        void Start();
        
        event Action OnSequenceCompleted;
    }
}