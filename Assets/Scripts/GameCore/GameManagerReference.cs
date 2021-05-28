using System;

namespace GameCore
{
    /// <summary>
    /// Lazy initialization of <typeparamref><name>GameManagerReference</name></typeparamref>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <example>
    ///     Same example how to use GameManagerProperty
    /// <code>
    ///    private GameManagerProperty<typeparamref><name>ISignalSystem</name></typeparamref>> _signalSystem;
    ///    
    ///    private void Start()
    ///    {
    ///        _signalSystem.Value.Subscribe<typeparamref><name>Signal</name></typeparamref>>(OnSignal);
    ///    }
    /// </code>
    /// </example>
    [Serializable]
    public struct GameManagerReference<T> where T : class, IGameManager
    {
        [NonSerialized]
        private T _managerReference;

        public T Value
        {
            get
            {
                if (_managerReference == null)
                {
                    ManagersHolder.TryGetManager(out _managerReference);
                    ManagersHolder.OnRemove += OnRemove;
                }

                return _managerReference;
            }
        }

        private void OnRemove(IGameManager obj)
        {
            if (obj == _managerReference)
            {
                _managerReference = null;
            }
        }

        public static implicit operator T(GameManagerReference<T> a)
        {
            return a.Value;
        }
    }
}
