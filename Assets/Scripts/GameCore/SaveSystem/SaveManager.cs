using System.Collections;
using GameCore.SignalSystem;
using UnityEngine;

namespace GameCore.SaveSystem
{
    public class SaveGameSignal : Signal
    {
    }
    
    public class DeleteSaveSignal : Signal
    {
    }

    public class SaveManager : MonoBehaviour
    {
        private bool _isSaved;
        private const float UpdateTime = 5f;
        private bool _needSave;

        private GameManagerReference<ISignalSystem> _signalSystem = new GameManagerReference<ISignalSystem>();
        private GameManagerReference<ISaveSystem> _saveSystem=new GameManagerReference<ISaveSystem>();

        private void Start()
        {
            //_signalSystem.Value?.Subscribe<InAppPurchasedSignal>(OnInappPurchased);
            StartCoroutine(UpdateSave());
            _signalSystem.Value?.Subscribe<SaveGameSignal>(Save);
            
            //_signalSystem.Value.Subscribe<CampusWalletUpdate>(Save);
            _signalSystem.Value?.Subscribe<DeleteSaveSignal>(DeleteSaveAndQuit);
        }

        private void DeleteSaveAndQuit(DeleteSaveSignal obj)
        {
            _saveSystem.Value.DeleteSaveAndQuit();
        }

     
        private void Save()
        {

            _needSave = true;
        }

        private void LateUpdate()
        {
            if (_needSave)
            {
                _needSave = false;
                _saveSystem.Value.Save();
            }
        }

        // private void OnInappPurchased(InAppPurchasedSignal obj)
        // {
        //     _signalSystem.Value?.Fire(new SaveGameSignal());
        // }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _signalSystem.Value?.Fire(new SaveGameSignal());
                _isSaved = true;
            }
            else
            {
                _isSaved = false;
            }
        }

        private void OnDestroy()
        {
            //_signalSystem.Value?.Unsubscribe<InAppPurchasedSignal>(OnInappPurchased);
            _signalSystem.Value?.Unsubscribe<SaveGameSignal>(Save);
            _signalSystem.Value?.Unsubscribe<DeleteSaveSignal>(DeleteSaveAndQuit);
            if (_isSaved)
            {
                return;
            }

            _saveSystem.Value.Save();
            _isSaved = true;
        }

        private void OnApplicationQuit()
        {
            if (_isSaved)
            {
                return;
            }

            _saveSystem.Value.Save();
            _needSave = true;
            _isSaved = true;
        }

        private IEnumerator UpdateSave()
        {
            while (true)
            {
                yield return new WaitForSeconds(UpdateTime);
                _signalSystem.Value?.Fire(new SaveGameSignal());
            }
        }
    }
}