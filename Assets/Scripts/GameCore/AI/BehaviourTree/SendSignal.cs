using System.Collections.Generic;
using UnityEngine;

namespace GameCore.AI.BehaviourTree
{
    public class SendSignal : StateMachineBehaviour
    {

        public string signal = "";
        [Range(0, 1)]
        public float time;
        public bool fired;
        private readonly List<WaitForAnimatorSignal> _listeners = new List<WaitForAnimatorSignal>();

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            fired = false;
            SetFalse();
        }

        private void SetFalse()
        {
            foreach (var n in _listeners)
            {
                n.IsSet = false;
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            SetFalse();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!fired && stateInfo.normalizedTime >= time)
            {
                foreach (var n in _listeners)
                {
                    n.IsSet = true;
                }
                fired = true;
            }
        }

        public static void Register(Animator animator, string name, WaitForAnimatorSignal node)
        {
            var found = false;
            foreach (var ss in animator.GetBehaviours<SendSignal>())
            {
                if (ss.signal.Equals(name))
                {
                    found = true;
                    ss._listeners.Add(node);
                }
            }

            if (!found)
            {
                Debug.LogError("Signal does not exist in animator: " + name);
            }
        }
    }
}