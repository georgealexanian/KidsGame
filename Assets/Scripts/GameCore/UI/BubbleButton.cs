using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public class BubbleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private const float durationScale = 0.1f;
        private const float durationUnscale = 0.35f;
        private const float targetScale = 0.9f;
        [SerializeField] private bool stick;

        private Button _button;
        private Tween _tweenAnimation;
        private Vector3[] _pressCorners = new Vector3[4];

        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_button.interactable || !_button.enabled)
            {
                return;
            }
            KillTween();
            (_button.transform as RectTransform).GetWorldCorners(_pressCorners);

            transform.localScale = Vector3.one;
            _tweenAnimation = transform.DOScale(targetScale, durationScale).SetEase(Ease.InOutSine);
        }

        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_button.interactable || stick || !_button.enabled)
            {
                return;
            }
            KillTween();
            
            var releaseCorners = new Vector3[4]; 
            (_button.transform as RectTransform).GetWorldCorners(releaseCorners);
            if (IsInCorners(eventData.position, _pressCorners) &&
                !IsInCorners(eventData.position, releaseCorners))
                {
                    _button.onClick.Invoke();
                }

            _tweenAnimation = transform.DOScale(1f, durationUnscale).SetEase(Ease.InOutSine);
        }

        
        private bool IsInCorners(Vector2 position, Vector3[] corners)
        {
            return (position.x > corners[0].x && position.x < corners[2].x &&
                position.y > corners[0].y && position.y < corners[2].y);
        }

        
        private void KillTween()
        {
            _tweenAnimation?.Kill();
            _tweenAnimation = null;
        }

        
        public void AnimateWithoutClick()
        {
            transform.DOScale(targetScale, durationScale).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                transform.DOScale(1f, durationUnscale).SetEase(Ease.InOutSine);
            });
        }

        private void OnDestroy()
        {
            KillTween();
        }
    }
}