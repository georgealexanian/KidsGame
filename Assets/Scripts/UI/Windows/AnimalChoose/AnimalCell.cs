using System;
using DG.Tweening;
using Game.AnimalChoose;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.AnimalChoose
{
    public class AnimalCell : MonoBehaviour
    {
        public const string PrefabKey = "AnimalCell";
        #pragma warning disable
        [SerializeField] private Image image;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float cellAppearTime = 0.5f;
        private Sequence seq;
        private string animalName;
        #pragma warning restore


        public void Init(Sprite sprite, string anName)
        {
            image.sprite = sprite;
            animalName = anName;

            AnimateAnimalCells();
        }
        
        
        private void AnimateAnimalCells()
        {
            transform.DOScale(0, 0);
            canvasGroup.DOFade(0, 0);
            
            seq = DOTween.Sequence();
            seq.Join(canvasGroup.DOFade(1, cellAppearTime));
            seq.Append(transform.DOScale(1.2f, cellAppearTime).OnComplete(() =>
            {
                transform.DOScale(1f, cellAppearTime);
            }));
            seq.OnComplete(() =>
            {
                seq?.Kill();
            });
        }


        private void OnDestroy()
        {
            seq?.Kill();
        }


        public void OnClick()
        {
            new ChooseAnimalSignal{name = animalName}.Fire();
        }
    }
}
