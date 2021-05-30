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
        public AnimalConfig AnimalConfig { get; private set; }
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
            AnimalConfig = animalsConfigText.text.FromJson<AnimalConfig>();
        }


        public List<string> GetRandomAnswerPaths(bool correctAnswers)
        {
            if (AnimalConfig == null)
            {
                return null;
            }
            
            if (correctAnswers)
            {
                return AnimalConfig.correctAction;
            }
            else
            {
                return AnimalConfig.inCorrectAction;
            }
        }


        public string GetAnimalIconsAtlasLabel()
        {
            return AnimalConfig?.portraitAtlasLabel;
        }


        public string GetTailAtlasLabel()
        {
            return AnimalConfig?.tailAtlasLabel;
        }

        
        public List<string> GetAnimalsNames()
        {
            if (AnimalConfig?.animals == null || AnimalConfig.animals.Count == 0)
            {
                return null;
            }
            var names = AnimalConfig.animals.Select(x => x.name).ToList();
            return names;
        }


        public List<string> GetTailNames()
        {
            if (AnimalConfig?.animals == null || AnimalConfig.animals.Count == 0)
            {
                return null;
            }
            var tailNames = AnimalConfig.animals.Select(x => x.tail).ToList();
            return tailNames;
        }


        public Animal GetAnimalInfo(string animalName)
        {
            if (!ExistsSuchAnimal(animalName))
            {
                return null;
            }
            return AnimalConfig.animals.Find(x => x.name == animalName);
        }


        private bool ExistsSuchAnimal(string animalName)
        {
            if (AnimalConfig?.animals == null || AnimalConfig.animals.Count == 0)
            {
                return false;
            }
            return AnimalConfig.animals.Exists(x => x.name == animalName);
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


