using System;
using System.Threading.Tasks;
using GameCore.SignalSystem;
using UnityEngine;

namespace GameCore.UI.WindowSystem
{
    public abstract class UIWindow : CanvasBehaviour
    {
#pragma warning disable
        [SerializeField] protected WindowBack back;
        public string windowKey;
        protected IWindowManager WManager;
        private bool _windowInitEnd;
        protected event Action OnCloseWindow;
#pragma warning restore

        public virtual void Awake()
        {
            if (back == null)
            {
                return;
            }

            back.OnBackClickEvent += OnCloseClickEvent; // TODO Implement Back Button
            back.OnCloseClickEvent += OnCloseClickEvent;
        }

        public virtual async Task Init(IWindowManager winManager, params object[] args)
        {
            await Task.Yield(); //spike to warning CS1998
            WManager = winManager;
            _windowInitEnd = true;
        }

        public void SetWManager(IWindowManager winManager)
        {
            WManager = winManager;
        }

        private void OnCloseClickEvent()
        {
            Close();
        }

        public virtual void OnWindowFocus()
        {
            
        }

        protected virtual void Close()
        {
            if (!_windowInitEnd)
            {
                return;
            }

            OnCloseWindow?.Invoke();
            WManager.Close(windowKey);
        }

        protected void CloseOnSignal<T>() where T : Signal
        {
            Signal.Subscribe<T>(Close);
        }

        private void OnBackClickEvent()
        {
            Back();
        }

        protected virtual void Back()
        {
            WManager.Back(windowKey);
        }
    }
}