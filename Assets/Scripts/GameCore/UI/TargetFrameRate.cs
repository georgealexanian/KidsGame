using UnityEngine;

namespace GameCore.UI
{
    public class TargetFrameRate : MonoBehaviour
    {
        [Range(1, 120)] public int targetRate = 60;
        private void Awake()
        {
            Application.targetFrameRate = targetRate;
        }
    }
}
