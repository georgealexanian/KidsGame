using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.AnimalChoose
{
    public class AnimalCell : MonoBehaviour
    {
        #pragma warning disable
        [SerializeField] private Image image;
        #pragma warning restore


        public void Init(Sprite sprite)
        {
            image.sprite = sprite;
        }


        public void OnClick()
        {
            
        }
    }
}
