using System.Collections.Generic;
using UnityEngine;

namespace GameCore.UI.WindowSystem
{
    [CreateAssetMenu(menuName = "GameCore/WindowsHolder")]
    public class WindowsHolder : ScriptableObject, IWindowsHolder
    {
        public List<GameObjectLink> windowsList = new List<GameObjectLink>();
        
        public List<GameObjectLink> GetAllWindows()
        {
            return windowsList;
        }
    }
}