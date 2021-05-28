using System;
using System.Collections;
using GameCore.UI.WindowSystem;
using UnityEngine;

namespace GameCore
{
    public class AntiSpeedHack : MonoBehaviour
    {
        private const int MinTriggerTimes = 4;

        private readonly GameManagerReference<IWindowManager> _windowsManager = new GameManagerReference<IWindowManager>();

        private static float _lastTime;
        public float tolerance = 2f;
        private int _triggerCounter;

//        void Start()
//        {
//            _lastTime = Time.time;
//            StartCoroutine(CheckDeltaTime());
//        }

        private IEnumerator CheckDeltaTime()
        {
            do
            {
                var result = Time.time - _lastTime;
                _lastTime = Time.time;
                if (result < 0)
                {
                    OnSpeedHackDetected();
                    yield return null;
                }

                if (result > (tolerance * 2))
                {
                    Debug.Log($"[AntiSpeedHack] big deltaTime {result - tolerance}");
                    _triggerCounter++;

                    if (_triggerCounter >= MinTriggerTimes)
                    {
                        OnSpeedHackDetected();
                        yield return null;
                    }
                }
                else
                {
                    _triggerCounter = 0;
                }

                yield return new WaitForSeconds(tolerance);
            } while (_triggerCounter < MinTriggerTimes);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
            {
                _lastTime = DateTime.Now.Second;
            }
        }

        private void OnSpeedHackDetected()
        {
            Action callback = OnPopupClosed;
            _windowsManager.Value.Open("WarningPopUp", "Warning", "Cheats detected! \n Please, restart the application.",
                callback); //TODO: "WarningPopup" is hardcode
        }

        private void OnPopupClosed()
        {
            Application.Quit();
        }
    }
}