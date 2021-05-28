using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCore.InputSystem
{
    [RequireComponent(typeof(Image))]
    public class MobileJoystickController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
#pragma warning disable CS0649
        [SerializeField] private string joyTag;
        [SerializeField] private bool isAlwaysVisible;
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform directionButton;
#pragma warning restore
        private Vector3 _defaultJoystickPosition;
        private Vector3 _defaultDirectionButtonPosition;
        private Vector3 _currentPosition;
        private float _dragRadius;

        private void Awake()
        {
            _dragRadius = background.rect.width / 2f;
            _defaultJoystickPosition = background.anchoredPosition;
            _defaultDirectionButtonPosition = directionButton.anchoredPosition;

            background.gameObject.SetActive(isAlwaysVisible);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 dragPosition = eventData.position;
            _currentPosition = Vector3.ClampMagnitude(dragPosition - background.position, _dragRadius);
            directionButton.transform.position = background.transform.TransformPoint(_currentPosition);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            background.transform.position = eventData.position;

            background.gameObject.SetActive(isAlwaysVisible || !background.gameObject.activeSelf);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            background.anchoredPosition = _defaultJoystickPosition;
            directionButton.anchoredPosition = _defaultDirectionButtonPosition;
            _currentPosition = Vector3.zero;

            background.gameObject.SetActive(isAlwaysVisible);
        }

        public float GetHorizontalAxis()
        {
            return _currentPosition.normalized.x;
        }

        public float GetVerticalAxis()
        {
            return _currentPosition.normalized.y;
        }
    }
}