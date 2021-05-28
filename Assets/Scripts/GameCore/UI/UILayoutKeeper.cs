using System;
using UnityEngine;

namespace GameCore.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UILayoutKeeper : MonoBehaviour
    {
        public LayoutDataKeeper portrait;
        public LayoutDataKeeper landscape;

        private RectTransform _rectTransform;
        private ScreenOrientation _deviceOrientation;

        public RectTransform RectTransform =>
            _rectTransform ? _rectTransform : _rectTransform = (RectTransform) transform;

        private void Update()
        {
            var devOri = Screen.orientation;
            if (devOri == _deviceOrientation) return;
            _deviceOrientation = devOri;
            ApplyLayout();
        }

        private void ApplyLayout()
        {
            if (_deviceOrientation == ScreenOrientation.Portrait)
            {
                LoadCurrentFromPortrait();
            }
            else
            {
                LoadCurrentFromLandscape();
            }
        }

        public void SaveCurrentToPortrait()
        {
            portrait.FromRectTransform(RectTransform);
        }

        public void SaveCurrentToLandscape()
        {
            landscape.FromRectTransform(RectTransform);
        }

        public void LoadCurrentFromPortrait()
        {
            portrait.ToRectTransform(RectTransform);
        }

        public void LoadCurrentFromLandscape()
        {
            landscape.ToRectTransform(RectTransform);
        }
    }

    [Serializable]
    public class LayoutDataKeeper : IEquatable<RectTransform>
    {
        [SerializeField] private Vector2 _anchorMin;
        [SerializeField] private Vector2 _anchorMax;
        [SerializeField] private Vector2 _anchoredPosition;
        [SerializeField] private Vector2 _sizeDelta;
        [SerializeField] private Vector2 _pivot;
        [SerializeField] private Vector3 _anchoredPosition3D;
        [SerializeField] private Vector3 _localScale;
        [SerializeField] private Quaternion _localRotation;

        public void FromRectTransform(RectTransform rectTransform)
        {
            _anchorMin = rectTransform.anchorMin;
            _anchorMax = rectTransform.anchorMax;
            _anchoredPosition = rectTransform.anchoredPosition;
            _sizeDelta = rectTransform.sizeDelta;
            _pivot = rectTransform.pivot;
            _anchoredPosition3D = rectTransform.anchoredPosition3D;
            _localScale = rectTransform.localScale;
            _localRotation = rectTransform.localRotation;
        }

        public void ToRectTransform(RectTransform rectTransform)
        {
            rectTransform.anchorMin = _anchorMin;
            rectTransform.anchorMax = _anchorMax;
            rectTransform.anchoredPosition = _anchoredPosition;
            rectTransform.sizeDelta = _sizeDelta;
            rectTransform.pivot = _pivot;
            rectTransform.anchoredPosition3D = _anchoredPosition3D;
            rectTransform.localScale = _localScale;
            rectTransform.localRotation = _localRotation;
        }

        public bool Equals(RectTransform rectTransform)
        {
            return rectTransform != null &&
                   (rectTransform.anchorMin == _anchorMin &&
                    rectTransform.anchorMax == _anchorMax &&
                    rectTransform.anchoredPosition == _anchoredPosition &&
                    rectTransform.sizeDelta == _sizeDelta &&
                    rectTransform.pivot == _pivot &&
                    rectTransform.anchoredPosition3D == _anchoredPosition3D &&
                    rectTransform.localScale == _localScale &&
                    rectTransform.localRotation == _localRotation);
        }
        
        public class FieldInfoKeeper
        {
            public string TypeInfo;
            public string FieldInfo;
            public object Value;
        }
    }
    
    
}