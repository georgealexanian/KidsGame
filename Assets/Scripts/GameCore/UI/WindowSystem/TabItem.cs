using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI.WindowSystem
{
    public class TabItem : MonoBehaviour
    {
        private const float SelectedScale = 1.1f;
        private const float ScaleTime = 0.2f;
        private Color _color;
        private Color _selectedColor;
        
#pragma warning disable CS0649
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private Sprite activeSprite;
        [SerializeField] private Sprite inactiveSprite;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private AudioClip audioClip;

        public GameObject content;
#pragma warning restore
        public Action onTabClicked;

        private bool _isSelected;
        
        private AudioClip AudioClip
        {
            get
            {
                return audioClip;
            }
        }
        
        private void Awake()
        {
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            onTabClicked?.Invoke();
        }

        public void SetSelected(bool selected)
        {
            _isSelected = selected;
            transform.DOScale(_isSelected ? SelectedScale : 1f, ScaleTime);
            image.sprite = _isSelected ? activeSprite : inactiveSprite;
            if (text != null)
            {
                text.color = _isSelected ? _selectedColor : _color;
            }
        }

        public void SetTextColors(Color color, Color selectedColor)
        {
            _color = color;
            _selectedColor = selectedColor;
        }
    }
}
