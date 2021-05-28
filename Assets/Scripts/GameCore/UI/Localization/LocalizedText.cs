using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameCore.UI.Localization
{
    [System.Serializable]
    public class LocalizedText : Text
    {
        [FormerlySerializedAs("TextType")] public TextType TxtType;
        private string _alias = string.Empty;

        public string Alias
        {
            get { return _alias; }
            set
            {
                TxtType = TextType.Localized;
                _alias = value;
                text = LocalizationManager.Instance.GetText(_alias);
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                TxtType = TextType.Simple;
                text = value;
            }
        }

        public enum TextType
        {
            Simple,
            Localized
        }

        protected override void Awake()
        {
            base.Awake();
            LocalizationManager.Instance.OnLanguageSwitch += OnLanguageSwitch;
        }

        private void OnLanguageSwitch(SystemLanguage language)
        {
            text = LocalizationManager.Instance.GetText(Alias);
        }
    }
}