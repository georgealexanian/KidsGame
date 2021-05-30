using System;
using Game.AnimalChoose.AnimalController;
using GameCore.SignalSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.TailChooseWindow
{
    public class TailCell : MonoBehaviour
    {
        public const string PrefabKey = "TailCell";
        #pragma warning disable
        [SerializeField] private Image icon;
        private string _tailName;
        #pragma warning restore

        
        public void Init(Sprite sprite, string tailName)
        {
            _tailName = tailName;
            icon.sprite = sprite;
            icon.SetNativeSize();
        }


        private void OnChosenCorrectAnswerSignal(ChosenCorrectAnswerSignal signal)
        {
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


        public void OnClick()
        {
            new TailChosenSignal(_tailName).Fire();
        }


        private void OnEnable()
        {
            Signal.Subscribe<ChosenCorrectAnswerSignal>(OnChosenCorrectAnswerSignal);
        }
        private void OnDisable()
        {
            Signal.Unsubscribe<ChosenCorrectAnswerSignal>(OnChosenCorrectAnswerSignal);
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
    
}
