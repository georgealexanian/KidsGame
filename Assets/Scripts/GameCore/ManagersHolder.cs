using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    /// <summary>
    /// Base game manager holder. Use to collect all game needs managers.
    /// </summary>
    /// <example>
    /// Use example :
    /// <code>
    ///
    /// Registration
    /// void GameLoad()
    /// {
    ///    IAudioManager audioManager = new AudioManager();
    ///    IResourceManager resManager = new ResourceManager();
    ///    ManagerHolder.AddManager(audioManager, resManager);
    /// }
    /// 
    /// or
    ///
    /// private void Awake()
    /// {
    ///    ManagersHolder.AddManager(this);
    /// }
    ///
    /// Get manager
    ///
    /// private void Start()
    /// {
    ///    ManagersHolder.TryGetManager(out ISameManager manager);
    /// }
    /// </code>
    /// </example>
    public static class ManagersHolder
    {
        private static Dictionary<Type, IGameManager> _managers = new Dictionary<Type, IGameManager>();
        public static event Action<IGameManager> OnRemove;
        public static void AddManager(IGameManager manager)
        {
            var type = manager.GetType();
            Debug.Log($"<color=red>[ManagersHolder] Add new manager of type : {type} !</color>");
            AddManagerInternal(manager, type);
        }
        public static void AddManager<T>(IGameManager manager) where T:IGameManager
        {
            var type = typeof(T);
            AddManagerInternal(manager, type);
        }
        private static void AddManagerInternal(IGameManager manager, Type type)
        {
            if (_managers.ContainsKey(type))
            {
                _managers[type] = manager;
            }
            else
            {
                _managers.Add(type, manager);
            }
        }

        public static void AddManagers(params IGameManager[] managerList)
        {
            foreach (var gameManager in managerList)
            {
                AddManager(gameManager);
            }
        }

        public static void RemoveManager(IGameManager manager)
        {
            OnRemove?.Invoke(manager);
            var type = manager.GetType();
            if (_managers.ContainsKey(type))
            {
                _managers.Remove(type);
            }
            Debug.Log($"<color=red>[ManagersHolder] Remove manager of type : {type} !</color>");
        }

        public static bool TryGetManager<T>(out T manager, GetManagerFilter filter = GetManagerFilter.AnyAssignable) where T : class, IGameManager
        {
            var typeKey = typeof(T);
            manager = default;
            switch (filter)
            {
                case GetManagerFilter.SpecificType:
                {
                    if (_managers.ContainsKey(typeKey) && _managers[typeKey] is T)
                    {
                        manager =  (T) _managers[typeKey];
                        return true;
                    }
                    ShowWarningMessage<T>(filter);
                    return false;
                }
                case GetManagerFilter.AnyAssignable:
                    foreach (var type in _managers.Keys)
                    {
                        if (typeKey.IsAssignableFrom(type))
                        {
                            manager = (T) _managers[type];
                            return true;
                        }
                    }
                    ShowWarningMessage<T>(filter);
                    return false;
                default:
                {
                    ShowWarningMessage<T>(filter);
                    return false;
                }
                    
            }
        }

        private static void ShowWarningMessage<T>(GetManagerFilter filter) where T : class, IGameManager
        {
            Debug.LogWarning($"Couldn't get manager {typeof(T)} with filter {filter}");
        }
        

        public static void Clear()
        {
            _managers = new Dictionary<Type, IGameManager>();
        }
    }
}