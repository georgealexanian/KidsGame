using System;
using UnityEngine;

namespace GameCore.UI.UIContentFitter
{
    public class UIFitter : CanvasBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private Camera cam;
        [SerializeField] private UIFitterBound reference;
        [SerializeField] private float placeObjOnDistance = 10f;
        [SerializeField] private UpdateType updateType;
#pragma warning restore
        
        public enum UpdateType
        {
            OnStart,
            OnUpdate
        }

        private Canvas _canvas;
        private Vector3 _lastPosition;

        private void Start()
        {
            _canvas = GetComponentInParent<Canvas>();
            FitToElement();
            _lastPosition = RectTransform.position;
            enabled = updateType == UpdateType.OnStart;
        }

        public void Update()
        {
            if (_lastPosition != RectTransform.position)
            {
                FitToElement();
                _lastPosition = RectTransform.position;
            }
        }

        private void FitToElement()
        {
            var rTrans = (RectTransform) transform;

            switch (_canvas.renderMode)
            {
                case RenderMode.ScreenSpaceOverlay:
                    RectTransformInOverlaySpace(rTrans);
                    break;
                case RenderMode.ScreenSpaceCamera:
                    RectTransformInCanvasCamera(rTrans);
                    break;
            }
        }


        private void RectTransformInOverlaySpace(RectTransform trans)
        {
            var result = RectTransformUtility.PixelAdjustRect(trans, _canvas);
            result.center = trans.position;
            var dist = cam.nearClipPlane + placeObjOnDistance;
            var pos = cam.ScreenToWorldPoint(new Vector3(result.center.x, result.center.y,dist));
            reference.transform.position = pos;
            var start = cam.ScreenToWorldPoint(new Vector3(result.xMin, result.yMin,dist));
            var end = cam.ScreenToWorldPoint(new Vector3(result.xMax, result.yMax,dist));
            var scale = Mathf.Min((end - start).x / reference.bounds.x, (end - start).y / reference.bounds.y);
            reference.transform.localScale = Vector3.one * scale;
        }

        private void RectTransformInCanvasCamera(RectTransform trans)
        {
            var arr = new Vector3[4];
            trans.GetWorldCorners(arr);
            var center = ((arr[3] + arr[0]) / 2.0f + (arr[2] + arr[1]) / 2f) / 2f;
            var referenceTransform = reference.transform;
            var scale = Mathf.Min(arr[3].x - arr[0].x, arr[2].y - arr[0].y);
            referenceTransform.localScale = scale * Vector3.one;
            referenceTransform.position = center + cam.transform.forward * scale / 2f;
        }
    }
}