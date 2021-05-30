using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game;
using Game.AnimalChoose;
using Game.AnimalChoose.AnimalController;
using GameCore;
using GameCore.Extensions;
using GameCore.SignalSystem;
using GameCore.UI.WindowSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;
using UnityEngine.UI;


namespace UI.Windows.TailChoose
{
    public class TailChooseWindow : GameUIWindow
    {
        public const string PrefabKey = "TailChooseWindow";
        #pragma warning disable
        [SerializeField] private List<TailPanelInfo> tailPanels;
        [SerializeField] private Button homeBtn;
        [SerializeField] private Transform animalTr;
        private GameManagerReference<AnimalManager> _animalManager;
        private Animal _animalInfo;
        private readonly List<GameObject> _cachedAddressables = new List<GameObject>();
        public event AnimalController.Talk TalkEvent;
        private AnimalController _animalController;
        private Coroutine _coroutine;
        #pragma warning restore
        
         
        public override async Task Init(params object[] args)
        {
            var tailNames = _animalManager.Value.GetTailNames();
            _animalInfo = _animalManager.Value.GetAnimalInfo(_animalManager.Value.ChosenAnimal);
            var tailsAtlas = await Addressables.LoadAssetAsync<SpriteAtlas>(_animalManager.Value.GetTailAtlasLabel()).Task;
            
            if (tailNames != null && _animalInfo != null && tailsAtlas != null)
            {
                homeBtn.interactable = false;
                
                tailNames = tailNames.OrderBy(x => x).ToList();
                await PopulateTails(tailNames, tailsAtlas);
                await SetUpAnimalPrefab();
                
                homeBtn.interactable = true;
                
                _coroutine = StartCoroutine(LongAwaitChecker((AnimalConfig.FirstInactivityAwait)));
            }
            else
            {
                Debug.Log($"<color=red>ERROR!!! Either animal info or tail name list or tail atlas is empty!!!</color>");
            }
        }


        private IEnumerator LongAwaitChecker(int awaitTime)
        {
            yield return new WaitForSeconds(awaitTime);
            new LongAwaitActionSignal{inactivityTime = awaitTime}.Fire();
            yield return new WaitForSeconds(awaitTime);
            new LongAwaitActionSignal{inactivityTime = awaitTime * 2}.Fire();
        }


        private void OnTailChosenSignal()
        {
            StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(LongAwaitChecker((AnimalConfig.FirstInactivityAwait)));
        }


        private async Task SetUpAnimalPrefab()
        {
            if (!_animalInfo.uiPrefabPath.IsNullOrEmpty())
            {
                var instGo = await Addressables.InstantiateAsync(_animalInfo.uiPrefabPath, animalTr).Task;
                _animalController = instGo.GetComponent<AnimalController>();
                TalkEvent += _animalController.StartTalking;
                _cachedAddressables.Add(instGo);
            }
            
            TalkEvent?.Invoke();
        }


        private async Task PopulateTails(List<string> tailNames, SpriteAtlas tailsAtlas)
        {
            for (int i = 0; i < tailPanels.Count; i++)
            {
                var startCount = tailPanels.Take(i).Sum(x => x.maxTailCount);
                var upperCount = startCount + tailPanels[i].maxTailCount;
                if (startCount > tailNames.Count - 1)
                {
                    break;
                }
                
                for (int j = startCount; j < upperCount; j++)
                {
                    if (j >= tailNames.Count)
                    {
                        break;
                    }
                    var instGo = await Addressables.InstantiateAsync(TailCell.PrefabKey, tailPanels[i].content).Task;
                    instGo.GetComponent<TailCell>().Init(tailsAtlas.GetSprite(tailNames[j]), tailNames[j], _animalInfo.tail == tailNames[j]);
                    _cachedAddressables.Add(instGo);
                }
            }
        }


        public void HomeBtn()
        {
            new StartSceneSignal(Scenes.MenuScene.ToString()).Fire();
        }


        private void OnStartSceneSignal()
        {
            Close();
        }


        private void OnDestroy()
        {
            foreach (var cachedAddressable in _cachedAddressables)
            {
                Addressables.Release(cachedAddressable);
            }
            _cachedAddressables.Clear();
            StopCoroutine(_coroutine);
            
            TalkEvent -= _animalController.StartTalking;
        }


        private void OnEnable()
        {
            Signal.Subscribe<StartSceneSignal>(OnStartSceneSignal);
            Signal.Subscribe<TailChosenSignal>(OnTailChosenSignal);
        }
        private void OnDisable()
        {
            Signal.Unsubscribe<StartSceneSignal>(OnStartSceneSignal);
            Signal.Unsubscribe<TailChosenSignal>(OnTailChosenSignal);
        }
        
    }
}


[Serializable]
public class TailPanelInfo
{
    public Transform content;
    public int maxTailCount;
}


[Serializable]
public class LongAwaitActionSignal : Signal
{
    public int inactivityTime;
}
