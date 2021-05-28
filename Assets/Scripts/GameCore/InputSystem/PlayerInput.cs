using UnityEngine;

namespace GameCore.InputSystem
{
    public class PlayerInput : InputComponent
    {
        public bool HaveControl => _haveControl;

        public InputButton pause;
        public InputButton secondButton;
        public InputButton firstButton;
        public InputButton thirdButton;
        public InputButton fourthButton;
        public InputAxis leftHorizontal;
        public InputAxis leftVertical;
        public InputAxis rightHorizontal;
        public InputAxis rightVertical;
    
        private bool _haveControl = true;
#if UNITY_EDITOR
        [SerializeField] private bool debugMenuIsOpen;
#endif
        protected override void Awake ()
        {
            base.Awake();
            
#if !UNITY_EDITOR
            inputType = InputType.MouseAndKeyboard;

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                inputType = InputType.Mobile;
// TODO Check leftUIJoystickController rightUIJoystickController mobileButtonsController and throw message if null!!
            }
            
            if (Input.GetJoystickNames().Length > 0) 
            {
                inputType = InputType.Controller;
            }
#endif
            
            pause = new InputButton(KeyCode.Escape, ControllerButtons.Options, mobileButtonsController.GetButton(ControllerButtons.Options));
            secondButton = new InputButton(KeyCode.O, ControllerButtons.Triangle, mobileButtonsController.GetButton(ControllerButtons.Triangle));
            firstButton = new InputButton(KeyCode.K, ControllerButtons.Square,mobileButtonsController.GetButton(ControllerButtons.Square));
            thirdButton = new InputButton(KeyCode.P, ControllerButtons.O, mobileButtonsController.GetButton(ControllerButtons.O));
            fourthButton = new InputButton(KeyCode.L, ControllerButtons.X, mobileButtonsController.GetButton(ControllerButtons.X));
            leftHorizontal = new InputAxis(KeyCode.D, KeyCode.A, ControllerAxes.LeftStickHorizontal, leftUIJoystickController);
            leftVertical = new InputAxis(KeyCode.W, KeyCode.S, ControllerAxes.LeftStickVertical, leftUIJoystickController);
            rightHorizontal = new InputAxis(KeyCode.D, KeyCode.A, ControllerAxes.RightStickHorizontal, rightUIJoystickController);
            rightVertical = new InputAxis(KeyCode.W, KeyCode.S, ControllerAxes.RightStickVertical, rightUIJoystickController);
        

        
        }
        

        protected override void GetInputs(bool fixedUpdateHappened)
        {
            pause.Get(fixedUpdateHappened, inputType);
            secondButton.Get(fixedUpdateHappened, inputType);
            firstButton.Get(fixedUpdateHappened, inputType);
            thirdButton.Get(fixedUpdateHappened, inputType);
            leftHorizontal.Get(inputType);
            leftVertical.Get(inputType);
            rightHorizontal.Get(inputType);
            rightVertical.Get(inputType);
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F12))
            {
                debugMenuIsOpen = !debugMenuIsOpen;
            }
#endif
        }

        public override void GainControl()
        {
            _haveControl = true;

            GainControl(pause);
            GainControl(secondButton);
            GainControl(firstButton);
            GainControl(thirdButton);
            GainControl(leftHorizontal);
            GainControl(leftVertical);
            GainControl(rightHorizontal);
            GainControl(rightVertical);
        }

        public override void ReleaseControl(bool resetValues = true)
        {
            _haveControl = false;

            ReleaseControl(pause, resetValues);
            ReleaseControl(secondButton, resetValues);
            ReleaseControl(firstButton, resetValues);
            ReleaseControl(thirdButton, resetValues);
            ReleaseControl(leftHorizontal, resetValues);
            ReleaseControl(leftVertical, resetValues);
            ReleaseControl(rightHorizontal, resetValues);
            ReleaseControl(rightVertical, resetValues);
        }

        public void Disable()
        {
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        public void Enable()
        {
            var canvasGroup = GetComponent<CanvasGroup>();
            if (inputType != InputType.Mobile)
            {
                return;
            }
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        
#if UNITY_EDITOR
        private void OnGUI() 
        {
            if (!debugMenuIsOpen) return;
        
            const float height = 400;

            GUILayout.BeginArea(new Rect(30, 30, 500, height));

            GUILayout.BeginVertical("box");
            GUILayout.Label("Press F12 to close");

            bool meleeAttackEnabled = GUILayout.Toggle(firstButton.Enabled, "Enable Attack");

            if (meleeAttackEnabled != firstButton.Enabled)
            {
                if (meleeAttackEnabled)
                    firstButton.Enable();
                else
                    firstButton.Disable();
            }
        
            GUILayout.Label("Pause Btn = "+pause);
            GUILayout.Label("Attack Btn = "+firstButton);
            GUILayout.Label("Jump Btn = "+thirdButton);
            GUILayout.Label("Interact Btn = "+secondButton);
        
            GUILayout.Label("Left Horizontal axis = "+leftHorizontal);
            GUILayout.Label("Left Vertical axis = "+leftVertical);
        
            GUILayout.Label("Right Horizontal axis = "+rightHorizontal);
            GUILayout.Label("Right Vertical axis = "+rightVertical);
        
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
#endif
    }
}