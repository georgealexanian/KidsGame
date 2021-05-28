using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace GameCore.UI.Localization
{
    [Serializable]
    public class LocalizationManager
    {
        private static LocalizationManager _instance;
        public static LocalizationManager Instance => _instance ?? (_instance = new LocalizationManager());

        public LocalizationAliases Aliases;
        private string PathToLocaleFile => "Localization/" + _language;
        private string FullPath => "Assets/Resources/" + PathToLocaleFile + ".txt";
        public event Action<SystemLanguage> OnLanguageSwitch;
        private SystemLanguage _language = SystemLanguage.Russian;

        public SystemLanguage Language
        {
            get { return _language; }
            set
            {
                if(_language == value)
                    return;
                
                _language = value;
                OnLanguageSwitch?.Invoke(_language);
            }
        }

        private LocalizationManager()
        {
            OnLanguageSwitch += (lang) => { Debug.Log("Language switch to " + lang.ToString()); };
            Aliases = new LocalizationAliases();
            ImportLocale();
        }

        private void ImportLocale()
        {
            var file = Resources.Load(PathToLocaleFile) as TextAsset;
            if (file == null)
            {
                Debug.LogError("File " + PathToLocaleFile + " not exist ! Create new if in editor.");
#if UNITY_EDITOR
                CreateLocaleFile();
                //AssetDatabase.CreateAsset(text, PathToLocaleFile + ".txt");
#endif
            }
            else
            {
                Debug.LogFormat("Import Locale from path {0} OK!", PathToLocaleFile);
                Aliases = JsonUtility.FromJson<LocalizationAliases>(file.text);
            }
        }

        private void CreateLocaleFile()
        {
            if (!Directory.Exists("Assets/Resources/Localization"))
            {
                Directory.CreateDirectory("Assets/Resources/Localization");
            }
            
            FileStream stream = File.Create(FullPath);
            var buffer = Encoding.ASCII.GetBytes("{}");
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();
        }

        /// <summary>
        /// Use only in EDITOR!
        /// </summary>
        private void SaveLocale()
        {
#if UNITY_EDITOR
            if (!File.Exists(FullPath))
                CreateLocaleFile();
            
            File.WriteAllText(FullPath, JsonUtility.ToJson(Aliases));
#endif
        }

        public string GetText(string alias)
        {
            string text;
            if (!TryGetText(alias, out text))
            {
                text = "error_" + alias;
            }

            return text;
        }

        private bool TryGetText(string alias, out string text)
        {
            text = string.Empty;

            if (!Aliases.ContainsKey(alias)) return false;

            text = Aliases[alias];
            return true;
        }

        public void Edit(string oldAlias, string newAlias, string text)
        {
            if (oldAlias.Equals(newAlias))
            {
                Aliases[newAlias] = text;
            }
            else
            {
                Aliases.Remove(oldAlias);
                Aliases.Add(newAlias, text);
            }

            SaveLocale();
        }

        /// <summary>
        /// Save Apply alias only in editor
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool TryAddAlias(string alias, string text)
        {
            if (!CheckAddEnable(alias, text)) return false;
            Aliases.Add(alias, text);
            SaveLocale();
            return true;
        }

        public bool CheckAddEnable(string alias, string text)
        {
            return !(string.IsNullOrEmpty(alias) || string.IsNullOrEmpty(text) || Aliases.ContainsKey(alias) || Aliases.ContainsValue(text));
        }
    }
}