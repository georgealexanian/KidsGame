using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCore.UI
{
    public class RepeatButton : Button, IDragHandler
    {
        public float repeatingInterval = 0.1f;
        public float repeatingDelayTime = 0.5f;
        public bool repeatingStart;
        [SerializeField]
        private ButtonClickedEvent onHoldClick = new ButtonClickedEvent();

        /// <summary>
        ///   <para>UnityEvent that is triggered when the Button is pressed.</para>
        /// </summary>
        public ButtonClickedEvent OnHoldClick
        {
            get => onHoldClick;
            set => onHoldClick = value;
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            switch (state)
            {
                case SelectionState.Pressed:
                    if (repeatingStart)
                    {
                        break;
                    }
                    repeatingStart = true;
                    InvokeRepeating(nameof(HoldClick), repeatingDelayTime, repeatingInterval);
                    break;
                case SelectionState.Normal:
                case SelectionState.Highlighted:
                case SelectionState.Selected:
                case SelectionState.Disabled:
                    if (repeatingStart)
                    {
                        CancelInvoke(nameof(HoldClick));
                        repeatingStart = false;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void HoldClick()
        {
            OnHoldClick?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            //For catching parent's drag
        }
    }
}