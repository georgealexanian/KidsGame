using System;
using UnityEngine;

namespace GameCore.Configs
{
    [CreateAssetMenu(menuName = "Core/CreateDefaultConfig")]
    [Serializable]
    public class DefaultConfig:ScriptableObject
    {

        public string remoteConfigAddress;
        public TextAsset manifest;
        public TextAsset[] defaultConfigs;
    }
}