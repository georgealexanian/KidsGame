using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCore.SignalSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore.UI.WindowSystem
{

    [RequireComponent(typeof(Canvas))]
    public class WindowsManager : MonoBehaviour, IWindowManager
    {
#pragma warning disable 0649
        public Transform windowRoot;
        [SerializeField] private GameObject uiBlockerGo;
        [SerializeField] private GameObject uiInvisibleBlockerGo;
        [SerializeField] private string keyComingSoon = "comingSoonWindow";
        private readonly LinkedList<UIWindow> _linkedWindows = new LinkedList<UIWindow>();
        private readonly List<string> _exclusionList = new List<string>
        {
            
        };

        public int CountOpenWindow => _linkedWindows.Count;
#pragma warning restore 0649
        
        public bool ShowPreloader
        {
            get => uiBlockerGo == null || uiBlockerGo.activeSelf;
            set
            {
                if (uiBlockerGo && uiBlockerGo.activeSelf != value)
                {
                    uiBlockerGo.SetActive(value);
                }
            }
        }
        
        public bool InvisibleBlockUIInput
        {
            get => uiInvisibleBlockerGo == null || uiInvisibleBlockerGo.activeSelf;
            set
            {
                Debug.Log($"InvisibleBlockUIInput uiInvisibleBlockerGo {(bool)uiInvisibleBlockerGo}  uiInvisibleBlockerGo.activeSelf {uiInvisibleBlockerGo.activeSelf}  value {value}");
                if (uiInvisibleBlockerGo && uiInvisibleBlockerGo.activeSelf != value)
                {
                    uiInvisibleBlockerGo.SetActive(value);
                }
            }
        }

        public void Awake()
        {
            ManagersHolder.AddManager(this);
            ShowPreloader = false;
        }
        

        public void OnEnable()
        {
            Signal.Subscribe<ShowWindowSignal>(OpenSignal);
            Signal.Subscribe<ShowCustomWindowSignal>(OpenCustomWindowSignal);
            Signal.Subscribe<ShowPopUpSignal>(OpenPopUpSignal);
        }
        private void OnDisable()
        {
            Signal.Unsubscribe<ShowWindowSignal>(OpenSignal);
            Signal.Unsubscribe<ShowPopUpSignal>(OpenPopUpSignal);
            Signal.Unsubscribe<ShowCustomWindowSignal>(OpenCustomWindowSignal);
        }

        
        
        private void OpenSignal(ShowWindowSignal obj)
        {
            Debug.Log("open window signal");
            try
            {
                Open(obj.windowKey, obj.openParams);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }
        private void OpenCustomWindowSignal(ShowCustomWindowSignal obj)
        {
            OpenCustomWindow(obj.windowKey, obj.openParams);
        }
        private void OpenPopUpSignal(ShowPopUpSignal obj)
        {
            OpenPopUp(obj.windowKey, obj.openParams);
        }

        public async void OpenPopUp(string windowKey, params object[] args)
        {
            if (!_exclusionList.Contains(windowKey))
            {
                ShowPreloader = true;
            }
            await InstantiatePopUp(windowKey, args);
            await Task.Yield();
            new WindowOpenedSignal{windowKey = windowKey}.Fire();
            ShowPreloader = false;
        }
        public async void Open(string windowKey, params object[] args)
        {
            if (!_exclusionList.Contains(windowKey))
            {
                ShowPreloader = true;
            }
            await InstantiateWindow(windowKey, args);
            await Task.Yield();
            new WindowOpenedSignal{windowKey = windowKey}.Fire();
            ShowPreloader = false;
        }
        public async void OpenCustomWindow(string windowKey, params object[] args)
        {
            if (!_exclusionList.Contains(windowKey))
            {
                ShowPreloader = true;
            }
            await InstantiateCustomWindow(windowKey, args);
            await Task.Yield();
            new WindowOpenedSignal{windowKey = windowKey}.Fire();
            ShowPreloader = false;
        }
        
        
        
        private async Task InstantiatePopUp(string key, params object[] args)
        {
            if (_linkedWindows.Count > 0 &&_linkedWindows.Any(w=> w.windowKey.Equals(key)))
            {
                return;
            }
            var window = await Addressables.InstantiateAsync(key, windowRoot).Task;
            if (!window)
            {
                key = keyComingSoon;
                window = await Addressables.InstantiateAsync(key, windowRoot).Task;
            }
            var uiWindow = window.GetComponent<UIWindow>();
            _linkedWindows.AddLast(uiWindow);

            await uiWindow.Init(this, args);
        }
        private async Task InstantiateWindow(string key, params object[] args)
        {
            if (_linkedWindows.Count > 0 &&_linkedWindows.Any(w=> w.windowKey.Equals(key)))
            {
                Debug.Log($"Window {key} already opened !!!");
                return;
            }

            var window = await Addressables.InstantiateAsync(key, windowRoot).Task;
            if (!window)
            {
                key = keyComingSoon;
                window = await Addressables.InstantiateAsync(key, windowRoot).Task;
            }

            var uiWindow = window.GetComponent<UIWindow>();
            var node = _linkedWindows.AddLast(uiWindow);
            node.Previous?.Value.gameObject.SetActive(false);

            Debug.Log("init window");
            await uiWindow.Init(this, args);
        }
        private async Task InstantiateCustomWindow(string key, params object[] args)
        {
            Debug.Log($"{key}   {args}");
            var window = await Addressables.InstantiateAsync(key, windowRoot).Task;
            if (!window)
            {
                key = keyComingSoon;
                window = await Addressables.InstantiateAsync(key, windowRoot).Task;
            }
            var uiWindow = window.GetComponent<UIWindow>();
            await uiWindow.Init(this, args);
        }
        
        
        public void Close(string windowKey)
        {
            var uiWindow = _linkedWindows.First(w=>w.windowKey.Equals(windowKey));
            if(uiWindow)
            {
                var node = _linkedWindows.Find(uiWindow);
                var previousValue = node?.Previous?.Value;
                if (previousValue)
                {
                    previousValue.gameObject.SetActive(true);
                    previousValue.OnWindowFocus();
                }

                _linkedWindows.Remove(uiWindow);

                if (uiWindow)
                {
                    Addressables.Release(uiWindow.gameObject);
                }
            }
            new WindowClosedSignal{windowKey = windowKey}.Fire();
            new WindowFocusedSignal{windowKey = GetFocusedWindow()}.Fire();
        }


        public string GetFocusedWindow()
        {
            return _linkedWindows != null && _linkedWindows.Count > 0 ? _linkedWindows.Last(w=> w.gameObject.activeInHierarchy).windowKey : "";
        }

        
        public bool IsOpenAnyWindow()
        {
            return _linkedWindows.Count > 0;
        }

        
        public bool IsOpen(string windowKey)
        {
            return _linkedWindows.FirstOrDefault(w=>w.windowKey.Equals(windowKey)) != null;
        }


        public void CloseAllWindows()
        {
            var keys = _linkedWindows.Select(uiWindow => uiWindow.windowKey).ToList();
            keys.ForEach(Close);
        }

        public void CloseAllWindowsExcept(List<string> exceptions = null)
        {
            var keys = _linkedWindows.Select(uiWindow => uiWindow.windowKey).ToList();
            if (exceptions != null && exceptions.Count > 0)
            {
                foreach (var key in keys)
                {
                    if (!exceptions.Contains(key))
                    {
                        Close(key);
                    }
                }
            }
            else
            {
                keys.ForEach(Close);
            }
        }

        
        public IWindowSequence CreateSequence()
        {
            return new WindowSequence(this);
        }

        
        public void Back(string windowKey)
        {
            Close(windowKey);
        }
    }

    
    public class WindowSequence : IWindowSequence
    {
        private readonly Queue<Tuple<string, object[]>> _queue = new Queue<Tuple<string, object[]>>();
        private readonly IWindowManager _windowsManager;

        private Tuple<string, object[]> _currentWindow;

        public event Action OnSequenceCompleted;

        public WindowSequence(IWindowManager windowManager)
        {
            _windowsManager = windowManager;
            Signal.Subscribe<WindowClosedSignal>(OnWindowClosed);
        }

        ~WindowSequence()
        {
            Signal.Unsubscribe<WindowClosedSignal>(OnWindowClosed);
        }

        public IWindowSequence AddWindow(string windowKey, params object[] args)
        {
            _queue.Enqueue(new Tuple<string, object[]>(windowKey, args));
            return this;
        }

        
        public void Start()
        {
            ShowNextWindow();
        }
        

        private void OnWindowClosed(WindowClosedSignal signal)
        {
            if (signal.windowKey == _currentWindow.Item1)
            {
                ShowNextWindow();
            }
        }

        
        private void ShowNextWindow()
        {
            if (_queue.Count <= 0)
            {
                CompleteSequence();
                return;
            }

            _currentWindow = _queue.Dequeue();
            _windowsManager.Open(_currentWindow.Item1, _currentWindow.Item2);
        }
        

        private void CompleteSequence()
        {
            Signal.Unsubscribe<WindowClosedSignal>(OnWindowClosed);
            OnSequenceCompleted?.Invoke();
        }
    }
}



[Serializable]
public class ShowWindowSignal : Signal
{
    public string windowKey;
    public object[] openParams;

    public ShowWindowSignal()
    {
            
    }
        
    public ShowWindowSignal(string windowKey, params object[] openParams)
    {
        this.windowKey = windowKey;
        this.openParams = openParams;
    }
}


[Serializable]
public class ShowCustomWindowSignal : Signal
{
    public string windowKey;
    public object[] openParams;
    public ShowCustomWindowSignal(string windowKey, params object[] openParams)
    {
        this.windowKey = windowKey;
        this.openParams = openParams;
    }
}


[Serializable]
public class ShowPopUpSignal : Signal
{
    public string windowKey;
    public object[] openParams;
    public ShowPopUpSignal(string windowKey, params object[] openParams)
    {
        this.windowKey = windowKey;
        this.openParams = openParams;
    }
}


[Serializable]
public class WindowOpenedSignal : Signal
{
    public string windowKey;
}


[Serializable]
public class WindowClosedSignal : Signal
{
    public string windowKey;
}

[Serializable]
public class WindowFocusedSignal : Signal
{
    public string windowKey;
}