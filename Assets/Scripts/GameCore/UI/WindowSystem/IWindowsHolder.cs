using System.Collections.Generic;
//using UnityEngine.AddressableAssets;

namespace GameCore.UI.WindowSystem
{
    public interface IWindowsHolder
    {
        List<GameObjectLink> GetAllWindows();
        //AssetReference GetWindow(string windowKey);
    }
}