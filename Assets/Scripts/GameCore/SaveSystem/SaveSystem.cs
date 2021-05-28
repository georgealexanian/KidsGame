using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using GameCore.SignalSystem;
using UnityEngine;

namespace GameCore.SaveSystem
{
    public class SaveSystem : ISaveSystem
    {
        private readonly string SavePath;
        private List<ISaveable> _saveables = new List<ISaveable>();

        private SaveContainer _saveContainer;
        private bool _isWriting;

        public SaveSystem()
        {
            SavePath = Path.Combine(Application.persistentDataPath, "save.dat");
            _saveContainer = new SaveContainer();

          
        }
        public void Save()
        {
            if (_isWriting)
            {
                return;
            }

            if (_saveables == null || _saveables.Count == 0)
            {
                Debug.Log($"[{nameof(SaveSystem)}] Save do not containst any objects to save!");
                return;
            }

            var saveContainer = new SaveContainer();

            foreach (var saveable in _saveables)
            {
                saveContainer.containers.Add(saveable.GetSaveContainer());
            }

            saveContainer.saveTime = DateTime.Now;

            Task.Run(() => WriteToFile(saveContainer));
            _saveContainer = saveContainer;
        }

        private async Task WriteToFile(SaveContainer saveContainer)
        {
            if (_isWriting)
            {
                Debug.Log("Writing in progress !!!! return state!!!");
                return;
            }

            _isWriting = true;
            try
            {
                using (var file = new FileStream(SavePath, FileMode.OpenOrCreate))
                {
                    var byteArray = await Task.Run(() =>
                    {
                        var binaryFormatter = new BinaryFormatter();
                        using (var memStream = new MemoryStream())
                        {
                            binaryFormatter.Serialize(memStream, saveContainer);
                            return memStream.ToArray();
                        }
                    });

                    await file.WriteAsync(byteArray, 0, byteArray.Length);
                    _isWriting = false;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                _isWriting = false;
                throw;
            }
        }

        public void AddSaveable(ISaveable saveable)
        {
            _saveables.Add(saveable);
        }

        public void RemoveSaveable(ISaveable saveable)
        {
            _saveables.Remove(saveable);
        }

        public void RemoveAllSaveables()
        {
            _saveables = new List<ISaveable>();
        }

        public T TryGetSave<T>(bool childs = false)
        {
            foreach (var container in _saveContainer.containers)
            {
                if (childs)
                {
                    if (container is T)
                    {
                        return (T) container;
                    }
                }
                else
                {
                    if (container.GetType() == typeof(T))
                    {
                        return (T) container;
                    }
                }
            }

            return default;
        }

        public DateTime GetSaveTime()
        {
            return _saveContainer.saveTime;
        }

        public bool Load()
        {
            Debug.Log($"Try load saveContainer from path : {SavePath}");
            if (!File.Exists(SavePath))
            {
                if (_saveContainer == null)
                {
                    _saveContainer = new SaveContainer();
                }

                return false;
            }

            Debug.Log($"File exist");
            var binaryFormatter = new BinaryFormatter();
            using (var fs = new FileStream(SavePath, FileMode.OpenOrCreate))
            {
                Debug.Log($"Try parse file.");
                try
                {
                    _saveContainer = (SaveContainer) binaryFormatter.Deserialize(fs);
                    Debug.Log($"Parse file complete. Status : {_saveContainer != null}");
                }
                catch (Exception exception)
                {
                    Debug.Log($"Deserialize fail : {exception.Message}");
                    _saveContainer = new SaveContainer();
                    return false;
                }
            }

            Debug.Log($"Load saveContainer finish.");
            return true;
        }

        public void DeleteSaveAndQuit()
        {
            RemoveAllSaveables();
            Application.Quit();
        }
    }


    [Serializable]
    public class SaveContainer
    {
        public DateTime saveTime;
        public List<object> containers = new List<object>();
    }
}