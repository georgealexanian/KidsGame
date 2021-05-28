using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.SignalSystem
{
    [Serializable]
    [RequireComponent(typeof(Button))]
    public class ButtonSignalTrigger : MonoBehaviour
    {
        private Button _button;

#pragma warning disable
        [SerializeField] private Signal signal;
#pragma warning restore
        
        private ISignalSystem _signalSystem;

        // Start is called before the first frame update
        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
            ManagersHolder.TryGetManager(out _signalSystem);
        }

        private void OnClick()
        {
            if (_signalSystem != null && signal != null)
            {
                _signalSystem.Fire(signal);
            }
        }
    }
}