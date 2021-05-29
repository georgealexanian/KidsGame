using System.Threading.Tasks;
using Game.AnimalChoose;
using GameCore;
using UnityEngine;

namespace Game
{
    public class GameEntry : MonoBehaviour
    {
        private async void Awake()
        {
            await InitializeManagers();
            
            new StartSceneSignal(Scenes.MenuScene.ToString()).Fire();
        }


        private async Task InitializeManagers()
        {
            var animalManager = new AnimalManager();
            await animalManager.Init();
            ManagersHolder.AddManager(animalManager);

            var sceneLoader = new SceneLoader();
            sceneLoader.Init();
            ManagersHolder.AddManager(sceneLoader);
        }
    }
}
