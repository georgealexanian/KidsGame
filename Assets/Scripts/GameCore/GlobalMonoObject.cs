using UnityEngine;

namespace GameCore
{
    public class GlobalMonoObject : MonoBehaviour
    {
        public static GlobalMonoObject Instance = null;

        void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance == this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}