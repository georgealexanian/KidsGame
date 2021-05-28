using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCore.InputSystem
{
    [RequireComponent(typeof(Graphic))]
    public class JoystickUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool Down { get; private set; }
        public bool Hold { get; private set; }
        public bool Up { get; private set; }

        private void Awake()
        {
            ResetState();
        }

        private void OnEnable()
        {
            ResetState();
        }

        private void OnDisable()
        {
            ResetState();
        }

        private void ResetState()
        {
            Down = false;
            Hold = false;
            Up = false;
        }

        private void LateUpdate()
        {
            if (Down)
            {
                Down = false;
            }
            
            if (Up)
            {
                Up = false;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Down = true;
            Hold = true;
            Up = false;
            Debug.Log(name+" : "+this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Down = false;
            Hold = false;
            Up = true;
            Debug.Log(name+" : "+this);
        }

        public override string ToString()
        {
            return $"Down = {Down} ; Hold = {Hold} ; Up = {Up}";
        }
    }
}
