using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace GameCore.SignalSystem
{
    [Serializable]
    public abstract class Signal
    {
        private static GameManagerReference<ISignalSystem> _sys;
        private static ISignalSystem SignalSystem
        {
            get
            {
                if (_sys.Value == null)
                {
                    Debug.Log($"Signal system null! Add default signal system!");
                    ManagersHolder.AddManager<ISignalSystem>(new SignalSystem());
                }

                return _sys.Value;
            }
        }

        public void Fire()
        {
            //Debug.Log($"Fire signal {GetType().Name}");
            SignalSystem.Fire(this);
        }

        public static void Subscribe<T>(Action<T> action) where T : Signal
        {
            SignalSystem.Subscribe(action);
        }

        public static void Subscribe<T>(Action action) where T : Signal
        {
            SignalSystem.Subscribe<T>(action);
        }
        
        public static void Subscribe(Type sType, Action action)
        {
            SignalSystem.Subscribe(sType,action);
        }
        
        public static void Unsubscribe(Type sType, Action action)
        {
            SignalSystem.Unsubscribe(sType,action);
        }

        public static void Unsubscribe<T>(Action<T> action) where T : Signal
        {
            SignalSystem.Unsubscribe(action);
        }

        public static void Unsubscribe<T>(Action action) where T : Signal
        {
            SignalSystem.Unsubscribe<T>(action);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(
                this, 
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
        }
    }

    [Serializable]
    public abstract class Signal<T> : Signal
    {
        public T value;

        protected Signal(T value)
        {
            this.value = value;
        }
    }
    
    [Serializable]
    public class FireFromMainThread : Signal
    {
        public Signal signal;

        public FireFromMainThread(Signal sig)
        {
            signal = sig;
        }
    }

    public class SignalSystem : ISignalSystem
    {
        public SignalSystem()
        {
            _callbackMap = new Dictionary<Type, Dictionary<SubscriptionId, Action<object>>>();
        }


        private struct SubscriptionId : IEquatable<SubscriptionId>
        {
            private readonly Type _name;
            private readonly object _token;

            public SubscriptionId(Type name, object token)
            {
                _name = name;
                _token = token;
            }

            public bool Equals(SubscriptionId other)
            {
                return _name == other._name && Equals(_token, other._token);
            }

            public override bool Equals(object obj)
            {
                return obj is SubscriptionId other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((_name != null ? _name.GetHashCode() : 0) * 397) ^
                           (_token != null ? _token.GetHashCode() : 0);
                }
            }
        }

        private readonly Dictionary<Type, Dictionary<SubscriptionId, Action<object>>> _callbackMap;

        public void Subscribe<T>(Action<T> callback) where T : Signal
        {
            void Wrapper(object x) => callback((T) x);
            var type = typeof(T);
            SubscribeInternal(type, Wrapper, callback);
        }

        public void Unsubscribe<T>(Action<T> callback) where T : Signal
        {
            var type = typeof(T);
            UnsubscribeInternal(type, callback);
        }

        public void Subscribe<T>(Action callback) where T : Signal
        {
            void Wrapper(object x) => callback();
            var type = typeof(T);
            SubscribeInternal(type, Wrapper, callback);
        }
        
        public void Subscribe(Type sType,Action callback)
        {
            void Wrapper(object x) => callback();
            SubscribeInternal(sType, Wrapper, callback);
        }

        public void Unsubscribe<T>(Action callback) where T : Signal
        {
            var type = typeof(T);
            UnsubscribeInternal(type, callback);
        }

        public void Unsubscribe(Type sType, Action callback)
        {
            UnsubscribeInternal(sType, callback);
        }
        
        public void Fire<T>(T signal) where T : Signal
        {
//            Debug.Log($"tut Fire event {signal.GetType()}");
            var type = signal.GetType();

            if (!_callbackMap.TryGetValue(type, out var result))
            {
                return;
            }

            var subscribers = new Dictionary<SubscriptionId, Action<object>>(result);

            foreach (var subscriber in subscribers)
            {
                try
                {
                    subscriber.Value?.Invoke(signal);
                }
                catch (Exception e)
                {
                    Debug.LogError($"type of crash signal {type}");
                    Debug.LogError(e); 
                    throw;
                }
            }
        }

        private void SubscribeInternal(Type type, Action<object> callback, object token)
        {
            if (!_callbackMap.ContainsKey(type))
            {
                _callbackMap.Add(type, new Dictionary<SubscriptionId, Action<object>>());
            }

            var subscriptionId = new SubscriptionId(type, token);
            var subscriptionBucket = _callbackMap[type];
            if (!subscriptionBucket.ContainsKey(subscriptionId))
            {
                subscriptionBucket.Add(subscriptionId, callback);
            }
        }

        private void UnsubscribeInternal(Type type, object token)
        {
            if (!_callbackMap.ContainsKey(type))
            {
                return;
            }

            var subscriptionId = new SubscriptionId(type, token);
            var subscriptionBucket = _callbackMap[type];
            subscriptionBucket.Remove(subscriptionId);
        }
    }
}