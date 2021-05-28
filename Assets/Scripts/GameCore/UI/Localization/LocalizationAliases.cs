using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.UI.Localization
{
    [Serializable]
    public class LocalizationAliases : IEnumerable<LocalizationPairInfo>
    {
        [SerializeField] private List<LocalizationPairInfo> _pairs = new List<LocalizationPairInfo>();

        public IEnumerator<LocalizationPairInfo> GetEnumerator()
        {
            return _pairs.GetEnumerator();
        }

        public void Add(string key, string value)
        {
            LocalizationPairInfo pairInfo = new LocalizationPairInfo {Alias = key, Text = value};
            if (!_pairs.Contains(pairInfo))
            {
                _pairs.Add(pairInfo);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsKey(string alias)
        {
            return _pairs.Find(pair => pair.Alias.Equals(alias)) != null;
        }

        public string this[string alias]
        {
            get { return _pairs.Find(pair => pair.Alias.Equals(alias))?.Text; }
            set {
                var pairInfo = _pairs.Find(pair => pair.Alias.Equals(alias));
                if(pairInfo != null)
                    pairInfo.Text = value;
                else 
                    Add(alias,value);
            }
        }

        public void Remove(string alias)
        {
            var pairInfo = _pairs.Find(pair => pair.Alias.Equals(alias));
            if(pairInfo != null)
                _pairs.Remove(pairInfo);
        }

        public bool ContainsValue(string text)
        {
            return _pairs.Find(pair => pair.Text.Equals(text)) != null;
        }
    }


    [Serializable]
    public class LocalizationPairInfo
    {
        public string Alias;
        public string Text;
    }
}