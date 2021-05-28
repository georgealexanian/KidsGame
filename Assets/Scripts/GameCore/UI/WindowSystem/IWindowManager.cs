using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCore.UI.WindowSystem
{
    public interface IWindowManager : IGameManager
    {
        bool ShowPreloader { get; set; }
        bool InvisibleBlockUIInput { get; set; }
        void Open(string windowKey, params object[] args);
        void Close(string windowKey);
        bool IsOpenAnyWindow();
        bool IsOpen(string windowKey);
        void Back(string windowKey);
        void CloseAllWindows();
        IWindowSequence CreateSequence();
    }
}