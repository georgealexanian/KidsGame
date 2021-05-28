using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI
{
    [RequireComponent(typeof(LayoutGroup))]
    public class ForceUpdateLayoutGroup : MonoBehaviour
    {
        private void Start()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.transform as RectTransform);
        }

        private void OnEnable()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.transform as RectTransform);
        }
    }
}
