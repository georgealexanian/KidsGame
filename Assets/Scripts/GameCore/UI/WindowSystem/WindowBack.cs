using System;
using TMPro;
using UnityEngine;

namespace GameCore.UI.WindowSystem
{
    public class WindowBack : MonoBehaviour
    {
        public event Action OnBackClickEvent;
        public event Action OnCloseClickEvent;

        public TextMeshProUGUI tittleLbl;

      

        public void OnBackClick()
        {
            OnBackClickEvent?.Invoke();
        }
    
        public void OnCloseClick()
        {
            OnCloseClickEvent?.Invoke();
        }
    }
}
