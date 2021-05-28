using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.UI
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
