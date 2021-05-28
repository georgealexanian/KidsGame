using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.SignalSystem
{
    public class MainThreadSignalBridge : MonoBehaviour
    {

        private readonly List<Signal> _signals = new List<Signal>();
        private bool _hasNew;

        public void Start()
        {
            Signal.Subscribe<FireFromMainThread>(AddToList);
        }
        public void OnDestroy()
        {
            Signal.Unsubscribe<FireFromMainThread>(AddToList);
        }
        private void AddToList(FireFromMainThread obj)
        {
            lock (_signals)
            {
                _signals.Add(obj.signal);
                _hasNew = true;
            }
        }

        public void Update()
        {
            if (_hasNew)
            {
                lock (_signals)
                {
                    foreach (var signal in _signals)
                    {
                        Debug.Log($"Fire event {signal.GetType()}   data: {signal.ToJson()}");
                        signal.Fire();
                    }
                    
                    _signals.Clear();
                }
                
                _hasNew = false;
            }
        }
    }
}