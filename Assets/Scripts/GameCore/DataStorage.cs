using System;
using System.Collections.Generic;
using GameCore.SaveSystem;

namespace GameCore
{
    [Serializable]
    public class DataStorage : IGameManager, ISaveable
    {
        private Dictionary<string, object> _data = new Dictionary<string, object>();

        public bool TryGetData<T>(string key, out T data)
        {
            data = default;
            if (_data.ContainsKey(key))
            {
                data = (T)_data[key];
                return true;
            }

            return false;
        }
        
        public T TryGetData<T>(string key, T defaultValue)
        {
            if (TryGetData(key, out T data))
            {
                return data;
            }
            
            PutData(key,defaultValue);
            return defaultValue;
        }

        public void PutData(string key, object value)
        {
            if (_data.ContainsKey(key))
            {
                _data[key] = value;
                return;
            }

            _data.Add(key, value);
        }
        
        public object GetSaveContainer()
        {
            return this;
        }
    }
}
