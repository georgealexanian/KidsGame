using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCore.InputSystem
{
    public abstract class InputComponent : MonoBehaviour, IGameManager
    {
        public enum InputType
        {
            MouseAndKeyboard,
            Controller,
            Mobile
        }
        public enum ControllerButtons
        {
            None,
            Square, // joystick button 0
            X, // joystick button 1
            O, // joystick button 2
            Triangle, // joystick button 3
            LB1, // joystick button 4
            RB1, // joystick button 5
            LB2, // joystick button 6
            RB2, // joystick button 7
            Share, // joystick button 8
            Options, // joystick button 9
            LeftStick, // joystick button 10
            RightStick, // joystick button 11
            PS, //joystick button 12
            TouchPad, // joystick button 13
        }
        public enum ControllerAxes
        {
            None,
            LeftStickHorizontal,
            LeftStickVertical,
            DpadHorizontal,
            DpadVertical,
            RightStickHorizontal,
            RightStickVertical,
            LeftTrigger,
            RightTrigger,
        }

        [Serializable]
        public class InputButton
        {
            public KeyCode key;
            public ControllerButtons controllerButton;
            public JoystickUIButton uiButton;
            public bool Down { get; protected set; }
            public bool Hold { get; protected set; }
            public bool Up { get; protected set; }
            public bool Enabled => _enabled;

            [SerializeField]
            protected bool _enabled = true;

            private bool _gettingInput = true;

            //This is used to change the state of a button (Down, Up) only if at least a FixedUpdate happened between the previous Frame
            //and this one. Since movement are made in FixedUpdate, without that an input could be missed it get press/release between fixedupdate
            private bool _afterFixedUpdateDown;
            private bool _afterFixedUpdateHeld;
            private bool _afterFixedUpdateUp;

            private static readonly Dictionary<int, string> _buttonsToName = new Dictionary<int, string>
            {
                {(int)ControllerButtons.Square, "Square"},
                {(int)ControllerButtons.X, "X"},
                {(int)ControllerButtons.O, "O"},
                {(int)ControllerButtons.Triangle, "Triangle"},
                {(int)ControllerButtons.LB1, "LB1"},
                {(int)ControllerButtons.RB1, "RB1"},
                {(int)ControllerButtons.LB2, "LB2"},
                {(int)ControllerButtons.RB2, "RB2"},
                {(int)ControllerButtons.Share, "Share"},
                {(int)ControllerButtons.Options, "Options"},
                {(int)ControllerButtons.LeftStick, "LeftStick"},
                {(int)ControllerButtons.RightStick, "RightStick"},
                {(int)ControllerButtons.PS, "PS"},
                {(int)ControllerButtons.TouchPad, "TouchPad"},
            };

            public InputButton(KeyCode key, ControllerButtons controllerButton, JoystickUIButton uiButton)
            {
                this.key = key;
                this.controllerButton = controllerButton;
                this.uiButton = uiButton;
            }

            public void Get(bool fixedUpdateHappened, InputType inputType)
            {
                if (!_enabled)
                {
                    Down = false;
                    Hold = false;
                    Up = false;
                    return;
                }

                if (!_gettingInput)
                    return;

                switch (inputType)
                {
                    case InputType.Controller when fixedUpdateHappened:
                        Down = Input.GetButtonDown(_buttonsToName[(int)controllerButton]);
                        Hold = Input.GetButton(_buttonsToName[(int)controllerButton]);
                        Up = Input.GetButtonUp(_buttonsToName[(int)controllerButton]);

                        _afterFixedUpdateDown = Down;
                        _afterFixedUpdateHeld = Hold;
                        _afterFixedUpdateUp = Up;
                        break;
                    case InputType.Controller:
                        Down = Input.GetButtonDown(_buttonsToName[(int)controllerButton]) || _afterFixedUpdateDown;
                        Hold = Input.GetButton(_buttonsToName[(int)controllerButton]) || _afterFixedUpdateHeld;
                        Up = Input.GetButtonUp(_buttonsToName[(int)controllerButton]) || _afterFixedUpdateUp;

                        _afterFixedUpdateDown |= Down;
                        _afterFixedUpdateHeld |= Hold;
                        _afterFixedUpdateUp |= Up;
                        break;
                    case InputType.MouseAndKeyboard when fixedUpdateHappened:
                        Down = Input.GetKeyDown(key);
                        Hold = Input.GetKey(key);
                        Up = Input.GetKeyUp(key);

                        _afterFixedUpdateDown = Down;
                        _afterFixedUpdateHeld = Hold;
                        _afterFixedUpdateUp = Up;
                        break;
                    case InputType.MouseAndKeyboard:
                        Down = Input.GetKeyDown(key) || _afterFixedUpdateDown;
                        Hold = Input.GetKey(key) || _afterFixedUpdateHeld;
                        Up = Input.GetKeyUp(key) || _afterFixedUpdateUp;

                        _afterFixedUpdateDown |= Down;
                        _afterFixedUpdateHeld |= Hold;
                        _afterFixedUpdateUp |= Up;
                        break;
                    case InputType.Mobile when fixedUpdateHappened && uiButton != null:
                        Down = uiButton.Down;
                        Hold = uiButton.Hold;
                        Up = uiButton.Up;

                        _afterFixedUpdateDown = Down;
                        _afterFixedUpdateHeld = Hold;
                        _afterFixedUpdateUp = Up;
                        break;
                    case InputType.Mobile when uiButton != null:
                        Down = uiButton.Down || _afterFixedUpdateDown;
                        Hold = uiButton.Hold || _afterFixedUpdateHeld;
                        Up = uiButton.Up || _afterFixedUpdateUp;

                        _afterFixedUpdateDown |= Down;
                        _afterFixedUpdateHeld |= Hold;
                        _afterFixedUpdateUp |= Up;
                        break;
                }
            }

            public void Enable()
            {
                _enabled = true;
            }

            public void Disable()
            {
                _enabled = false;
            }

            public void GainControl()
            {
                _gettingInput = true;
            }

            public IEnumerator ReleaseControl(bool resetValues)
            {
                _gettingInput = false;

                if (!resetValues)
                    yield break;

                if (Down)
                    Up = true;
                Down = false;
                Hold = false;

                _afterFixedUpdateDown = false;
                _afterFixedUpdateHeld = false;
                _afterFixedUpdateUp = false;

                yield return null;

                Up = false;
            }

            public override string ToString()
            {
                return $"Enable = {Enabled} ; Down = {Down} ; Held = {Hold} ; Up = {Up} ;";
            }
        }

        [Serializable]
        public class InputAxis
        {
            public KeyCode positive;
            public KeyCode negative;
            public ControllerAxes controllerAxis;
            public MobileJoystickController uiJoystickController;
            public float Value { get; protected set; }
            public bool ReceivingInput { get; protected set; }
            public bool Enabled { get; private set; } = true;

            private bool _GettingInput = true;

            private static readonly Dictionary<int, string> k_AxisToName = new Dictionary<int, string> {
                {(int)ControllerAxes.LeftStickHorizontal, "LeftStick Horizontal"},
                {(int)ControllerAxes.LeftStickVertical, "LeftStick Vertical"},
                {(int)ControllerAxes.DpadHorizontal, "Dpad Horizontal"},
                {(int)ControllerAxes.DpadVertical, "Dpad Vertical"},
                {(int)ControllerAxes.RightStickHorizontal, "RightStick Horizontal"},
                {(int)ControllerAxes.RightStickVertical, "RightStick Vertical"},
                {(int)ControllerAxes.LeftTrigger, "Left Trigger"},
                {(int)ControllerAxes.RightTrigger, "Right Trigger"},
            };

            public InputAxis(KeyCode positive, KeyCode negative, ControllerAxes controllerAxis, MobileJoystickController uiJoystickController)
            {
                this.positive = positive;
                this.negative = negative;
                this.controllerAxis = controllerAxis;
                this.uiJoystickController = uiJoystickController;
            }

            public void Get(InputType inputType)
            {
                if (!Enabled)
                {
                    Value = 0f;
                    return;
                }

                if (!_GettingInput)
                    return;

                bool positiveHeld = false;
                bool negativeHeld = false;

                switch (inputType)
                {
                    case InputType.Controller:
                    {
                        Value = Input.GetAxisRaw(k_AxisToName[(int)controllerAxis]);
                        positiveHeld = Value > float.Epsilon;
                        negativeHeld = Value < -float.Epsilon;

                        break;
                    }
                    case InputType.MouseAndKeyboard:
                        positiveHeld = Input.GetKey(positive);
                        negativeHeld = Input.GetKey(negative);
                        
                        if (positiveHeld == negativeHeld)
                            Value = 0f;
                        else if (positiveHeld)
                            Value = 1f;
                        else
                            Value = -1f;
                        
                        break;
                    case InputType.Mobile when uiJoystickController != null:
                        switch (controllerAxis)
                        {
                            case ControllerAxes.LeftStickHorizontal:
                            case ControllerAxes.RightStickHorizontal:
                                Value = uiJoystickController.GetHorizontalAxis();
                                break;
                            case ControllerAxes.LeftStickVertical:
                            case ControllerAxes.RightStickVertical:
                                Value = uiJoystickController.GetVerticalAxis();
                                break;
                        }
                        positiveHeld = Value > float.Epsilon;
                        negativeHeld = Value < -float.Epsilon;
                        
                        break;
                }

                ReceivingInput = positiveHeld || negativeHeld;
            }

            public void Enable()
            {
                Enabled = true;
            }

            public void Disable()
            {
                Enabled = false;
            }

            public void GainControl()
            {
                _GettingInput = true;
            }

            public void ReleaseControl(bool resetValues)
            {
                _GettingInput = false;
                
                if (!resetValues) return;
                
                Value = 0f;
                ReceivingInput = false;
            }

            public override string ToString()
            {
                return $"Enable = {Enabled} ; ReceivingInput = {ReceivingInput} ; Value = {Value} ;";
            }
        }

        public InputType inputType = InputType.MouseAndKeyboard;
        [FormerlySerializedAs("mobileJoystickController")] public MobileJoystickController leftUIJoystickController;
        public MobileJoystickController rightUIJoystickController;
        public MobileButtonsController mobileButtonsController;
        private bool _fixedUpdateHappened;

        protected virtual void Awake()
        {
            ManagersHolder.AddManager(this);
        }

        private void Update()
        {
            GetInputs(_fixedUpdateHappened || Mathf.Approximately(Time.timeScale,0));

            _fixedUpdateHappened = false;
        }

        private void FixedUpdate()
        {
            _fixedUpdateHappened = true;
        }

        protected abstract void GetInputs(bool fixedUpdateHappened);

        public abstract void GainControl();

        public abstract void ReleaseControl(bool resetValues = true);

        protected void GainControl(InputButton inputButton)
        {
            inputButton.GainControl();
        }

        protected void GainControl(InputAxis inputAxis)
        {
            inputAxis.GainControl();
        }

        protected void ReleaseControl(InputButton inputButton, bool resetValues)
        {
            StartCoroutine(inputButton.ReleaseControl(resetValues));
        }

        protected void ReleaseControl(InputAxis inputAxis, bool resetValues)
        {
            inputAxis.ReleaseControl(resetValues);
        }
    }
}
