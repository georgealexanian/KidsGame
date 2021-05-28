using System;
using System.Reflection;
using UnityEngine;

namespace GameCore.SignalSystem
{
    public class OnMouseDownFireSignal : MonoBehaviour
    {
        private readonly GameManagerReference<ISignalSystem> _signalSystem = new GameManagerReference<ISignalSystem>();
        public string typeName;
        public object[] signalParams;
        private void OnMouseUpAsButton()
        {
            var instance = Activator.CreateInstance(Assembly.GetAssembly(GetType()).GetType(typeName), signalParams);
        
            if (instance is Signal fireSignal)
            {
                _signalSystem.Value?.Fire(fireSignal);
            }
            else
            {
                Debug.Log($"Type name {typeName} is not child type of Signal.");
            }
        }
    }
}