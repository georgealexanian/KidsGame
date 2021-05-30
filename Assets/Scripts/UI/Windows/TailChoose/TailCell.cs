using System;
using DG.Tweening;
using Game.AnimalChoose;
using Game.AnimalChoose.AnimalController;
using GameCore.SignalSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.TailChoose
{
    public class TailCell : MonoBehaviour
    {
        public const string PrefabKey = "TailCell";
        #pragma warning disable
        [SerializeField] private Image icon;
        private string _tailName;
        private bool _isCorrectTail;
        private Sequence _animSequence;
#pragma warning restore

        
        public void Init(Sprite sprite, string tailName, bool isCorrectTail)
        {
            _tailName = tailName;
            icon.sprite = sprite;
            _isCorrectTail = isCorrectTail;
            icon.SetNativeSize();
        }


        private void OnChosenCorrectAnswerSignal(ChosenCorrectAnswerSignal signal)
        {
            DOTween.Kill(transform);
            
            if (signal.tailName == _tailName && !signal.isCorrect)
            {
                icon.color = Color.red;
            }
            else if(signal.tailName == _tailName && signal.isCorrect)
            {
                icon.color = Color.green;
            }
            else
            {
                icon.color = Color.white;
            }
        }


        private void OnPulsateSignal()
        {
            if (_animSequence == null)
            {
                _animSequence = DOTween.Sequence().Join(transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 2, 1, 0f));
                _animSequence.SetLoops(-1, LoopType.Yoyo);
            }
        }
        
        
        private void OnLongAwaitActionSignal(LongAwaitActionSignal signal)
        {
            if (_isCorrectTail)
            {
                switch (signal.inactivityTime)
                {
                    case AnimalConfig.SecondInactivityAwait:
                        HandPointer.InstantiateHand(transform.position);
                        break;
                }
            }
        }


        public void OnClick()
        {
            new TailChosenSignal(_tailName).Fire();
        }


        private void OnDestroy()
        {
            _animSequence?.Kill();
        }
        

        private void OnEnable()
        {
            Signal.Subscribe<ChosenCorrectAnswerSignal>(OnChosenCorrectAnswerSignal);
            Signal.Subscribe<PulsateSignal>(OnPulsateSignal);
            Signal.Subscribe<LongAwaitActionSignal>(OnLongAwaitActionSignal);
        }
        private void OnDisable()
        {
            Signal.Unsubscribe<ChosenCorrectAnswerSignal>(OnChosenCorrectAnswerSignal);
            Signal.Unsubscribe<PulsateSignal>(OnPulsateSignal);
            Signal.Unsubscribe<LongAwaitActionSignal>(OnLongAwaitActionSignal);
        }
    }


    [Serializable]
    public class TailChosenSignal : Signal
    {
        public string tailName;

        public TailChosenSignal(string tailName)
        {
            this.tailName = tailName;
        }
    }


    [Serializable]
    public class PulsateSignal : Signal
    {
        
    }
    
}
