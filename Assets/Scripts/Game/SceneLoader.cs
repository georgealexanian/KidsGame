using System;
using GameCore;
using GameCore.SignalSystem;
using GameCore.UI.WindowSystem;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class SceneLoader : IGameManager
    {
        public SceneLoader()
        {
            Signal.Subscribe<StartSceneSignal>(OnStartSceneSignal);
        }
        ~SceneLoader()
        {
            Signal.Unsubscribe<StartSceneSignal>(OnStartSceneSignal);
        }
        
        
        public void Init()
        {
        }
        

        private void OnStartSceneSignal(StartSceneSignal signal)
        {
            Addressables.LoadSceneAsync(signal.sceneName);
        }
    }


    [Serializable]
    public class StartSceneSignal : Signal
    {
        public string sceneName;

        public StartSceneSignal(string sceneName)
        {
            this.sceneName = sceneName;
        }
    }


    public enum Scenes
    {
        Unknown = 0,
        GameScene = 1,
        MenuScene = 2,
        LoadingScene = 3
    }
}
