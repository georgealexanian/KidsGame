using UnityEngine;

namespace GameCore.AI.BehaviourTree
{
    /// <summary>
    /// Wait for a signal to be received from a SendSignal state machine behaviour on an animator.
    /// </summary>
    public class WaitForAnimatorSignal : BehTreeNode
    {
        internal bool IsSet;
        private readonly string _name;

        public WaitForAnimatorSignal(Animator animator, string name, string state, int layer = 0)
        {
            _name = name;
            var id = Animator.StringToHash(state);
            if (!animator.HasState(layer, id))
            {
                Debug.LogError("The animator does not have state: " + state);
            }
            else
            {
                SendSignal.Register(animator, name, this);
            }
        }

        public override BtState Tick()
        {
            if (!IsSet)
            {
                return BtState.Continue;
            }
            
            IsSet = false;
            return BtState.Success;

        }

        public override string ToString()
        {
            return "Wait For Animator Signal : " + _name;
        }
    }
}