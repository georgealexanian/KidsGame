using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCore;
using GameCore.Extensions;
using GameCore.SignalSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.AnimalChoose
{
    public class AnimalManager : IGameManager
    {
        private const string AnimalConfigLabel = "AnimalConfig";
        private AnimalConfig animalConfig;
        public string ChosenAnimal { get; private set; }


        public AnimalManager()
        {
            Signal.Subscribe<ChooseAnimalSignal>(OnChooseAnimalSignal);
        }
        ~AnimalManager()
        {
            Signal.Unsubscribe<ChooseAnimalSignal>(OnChooseAnimalSignal);
        }
        
        
        public async Task Init()
        {
            var animalsConfigText = await Addressables.LoadAssetAsync<TextAsset>(AnimalConfigLabel).Task;
            animalConfig = animalsConfigText.text.FromJson<AnimalConfig>();
        }


        public string GetAnimalIconsAtlasLabel()
        {
            return animalConfig?.portraitAtlasLabel;
        }


        public string GetTailAtlasLabel()
        {
            return animalConfig?.tailAtlasLabel;
        }

        
        public List<string> GetAnimalsNames()
        {
            if (animalConfig?.animals == null || animalConfig.animals.Count == 0)
            {
                return null;
            }
            var names = animalConfig.animals.Select(x => x.name).ToList();
            return names;
        }


        public Animal GetAnimalInfo(string animalName)
        {
            if (!ExistsSuchAnimal(animalName))
            {
                return null;
            }
            return animalConfig.animals.Find(x => x.name == animalName);
        }


        private bool ExistsSuchAnimal(string animalName)
        {
            if (animalConfig?.animals == null || animalConfig.animals.Count == 0)
            {
                return false;
            }
            return animalConfig.animals.Exists(x => x.name == animalName);
        }


        private void OnChooseAnimalSignal(ChooseAnimalSignal signal)
        {
            ChosenAnimal = signal.name;
            new StartSceneSignal(Scenes.GameScene.ToString()).Fire();
        }
    }



    [Serializable]
    public class ChooseAnimalSignal : Signal
    {
        public string name;
    }
    
}


