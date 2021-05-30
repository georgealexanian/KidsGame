using System;
using DG.Tweening;
using Game.AnimalChoose.AnimalController;
using GameCore;
using GameCore.SignalSystem;
using GameCore.UI.WindowSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UI.Windows.TailChoose
{
    public class HandPointer : MonoBehaviour
    {
        public const string PrefabKey = "HandPointer";
        private Sequence _animSequence;
        private static bool _created;
        
        
        private void Awake()
        {
            if (_animSequence == null)
            {
                _animSequence = DOTween.Sequence().Join(transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 1, 1, 0f));
                _animSequence.SetLoops(-1, LoopType.Yoyo);
            }
        }

        
        private void OnDestroy()
        {
            _animSequence?.Kill();
            Addressables.Release(gameObject);
            _created = false;
        }

        
        public static async void InstantiateHand(Vector3 position)
        {
            if (_created)
            {
                return;
            }
            
            ManagersHolder.TryGetManager(out WindowsManager windowsManager);
            var instGo = await Addressables.InstantiateAsync(PrefabKey, windowsManager.windowRoot).Task;
            instGo.transform.position = position;
            _created = true;
        }
        
        
        private void OnChosenCorrectAnswerSignal(ChosenCorrectAnswerSignal signal)
        {
            if (signal.isCorrect)
            {
                _animSequence?.Kill();
                Destroy(gameObject);
            }
        }


        private void OnWindowClosedSignal(WindowClosedSignal signal)
        {
            if (signal.windowKey == TailChooseWindow.PrefabKey)
            {
                Destroy(gameObject);
            }
        }
        

        private void OnEnable()
        {
            Signal.Subscribe<ChosenCorrectAnswerSignal>(OnChosenCorrectAnswerSignal);
            Signal.Subscribe<WindowClosedSignal>(OnWindowClosedSignal);
        }
        private void OnDisable()
        {
            Signal.Unsubscribe<ChosenCorrectAnswerSignal>(OnChosenCorrectAnswerSignal);
            Signal.Unsubscribe<WindowClosedSignal>(OnWindowClosedSignal);
        }
    }
}


