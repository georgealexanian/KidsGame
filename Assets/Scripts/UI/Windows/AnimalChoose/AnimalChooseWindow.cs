using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Game;
using Game.AnimalChoose;
using GameCore;
using GameCore.SignalSystem;
using GameCore.UI.WindowSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;

namespace UI.Windows.AnimalChoose
{
    public class AnimalChooseWindow : GameUIWindow
    {
        public const string PrefabKey = "AnimalChooseWindow";
        #pragma warning disable
        [SerializeField] private Transform contentTr;
        private GameManagerReference<AnimalManager> _animalManager;
        private readonly List<GameObject> _cachedAnimalCells = new List<GameObject>();
        #pragma warning restore
        
        
        public override async Task Init(params object[] args)
        {
            var animalIconsAtlas = await Addressables.LoadAssetAsync<SpriteAtlas>(_animalManager.Value.GetAnimalIconsAtlasLabel()).Task;
            if (animalIconsAtlas != null)
            {
                var animalNames = _animalManager.Value.GetAnimalsNames();
                if (animalNames != null)
                {
                    foreach (var animalName in animalNames)
                    {
                        var instGo = await Addressables.InstantiateAsync(AnimalCell.PrefabKey, contentTr).Task;
                        _cachedAnimalCells.Add(instGo);
                        instGo.GetComponent<AnimalCell>().Init(animalIconsAtlas.GetSprite(animalName), animalName);
                    }
                }
            }
        }


        private void OnDestroy()
        {
            foreach (var cachedAnimalCell in _cachedAnimalCells)
            {
                Addressables.Release(cachedAnimalCell);
            }
            _cachedAnimalCells.Clear();
        }
        
        
        private void OnStartSceneSignal()
        {
            Close();
        }


        private void OnEnable()
        {
            Signal.Subscribe<StartSceneSignal>(OnStartSceneSignal);
        }
        private void OnDisable()
        {
            Signal.Unsubscribe<StartSceneSignal>(OnStartSceneSignal);
        }
    }
}
