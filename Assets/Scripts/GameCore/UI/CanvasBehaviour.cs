using UnityEngine;

namespace GameCore.UI
{
    public class CanvasBehaviour : MonoBehaviour
    {
        
        private RectTransform _rectTransform;
        protected RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = (RectTransform) transform;
                }


                return _rectTransform;
            }
        }
    }
}