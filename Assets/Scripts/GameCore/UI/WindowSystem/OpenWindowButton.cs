using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI.WindowSystem
{
    [RequireComponent(typeof(Button))]
    public class OpenWindowButton : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private string windowKey;
        [SerializeField] private string windowContent = "default";
#pragma warning restore

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(new ShowWindowSignal{windowKey = windowKey, openParams = new object[]{windowContent}}.Fire);
        }
    }
}
