using System.Globalization;
using TMPro;
using UnityEngine;

namespace GameCore.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FPSCounter : MonoBehaviour
    {
 
        private float _timer, _avgFrameRate;
        [SerializeField] private float refreshRate = 1f;
        [SerializeField] private string display = "{0} FPS";
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Gradient colors;
 
        private void Start()
        {
            if (text == null)
            {
                text = GetComponent<TextMeshProUGUI>();
            }
        }
 
 
        private void Update()
        {
            var timelapse = Time.smoothDeltaTime;
            _timer = _timer <= 0 ? refreshRate : _timer -= timelapse;

            if (!(_timer <= 0))
            {
                return;
            }
            
            
            _avgFrameRate = (int) (1f / timelapse);
            text.color = colors.Evaluate(_avgFrameRate / 60f);
            text.text = string.Format(display,_avgFrameRate.ToString(CultureInfo.InvariantCulture));

        }
    }
}
