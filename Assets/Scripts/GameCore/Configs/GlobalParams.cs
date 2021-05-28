using System.Collections.Generic;
using Newtonsoft.Json;

namespace GameCore.Configs
{
    [JsonObject(MemberSerialization.Fields)]
    public class GlobalParams
    {
        private const string ConfigPath = "GlobalParams.json";
        private Dictionary<string, object> _values = new Dictionary<string, object>();

        public GlobalParams()
        {
        }

        public GlobalParams(GlobalParams globalParams)
        {
            _values = new Dictionary<string, object>(globalParams._values);
        }

        public int Count => _values.Count;

        public T Get<T>(string key) where T : class
        {
            var result = default(T);
            
            if (_values.ContainsKey(key))
            {
                result = _values[key] as T;
            }
            
            return result;
        }
        
        public void Set<T>(string key, T value)
        {
            if (!_values.ContainsKey(key))
            {
                _values.Add(key, value);
            }
            else
            {
                _values[key] = value;
            }
        }

        public void Remove(string key)
        {
            _values.Remove(key);
        }

        public void Clear()
        {
            _values.Clear();
        }

        public IEnumerable<KeyValuePair<string, object>> GetAll()
        {
            foreach (var value in _values)
            {
                yield return value;
            }
        }
    }
}