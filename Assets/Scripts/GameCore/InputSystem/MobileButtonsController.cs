using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.InputSystem
{
    public class MobileButtonsController : MonoBehaviour
    {
        public List<ScreenUIButton> screenButtons;

        public JoystickUIButton GetButton(InputComponent.ControllerButtons controllerButtons)
        {
            return screenButtons.Find(s => s.controllerButton == controllerButtons)?.button;
        }
    }

    [Serializable]
    public class ScreenUIButton
    {
        public InputComponent.ControllerButtons controllerButton;
        public JoystickUIButton button;
    }
}