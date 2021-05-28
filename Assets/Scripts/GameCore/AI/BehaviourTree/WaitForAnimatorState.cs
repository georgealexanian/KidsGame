using UnityEngine;

namespace GameCore.AI.BehaviourTree
{
    /// <summary>
    /// Wait for an animator to reach a state.
    /// </summary>
    public class WaitForAnimatorState : BehTreeNode
    {
        private readonly Animator _animator;
        private readonly int _id;
        private readonly int _layer;
        private readonly string _stateName;

        public WaitForAnimatorState(Animator animator, string name, int layer = 0)
        {
            _id = Animator.StringToHash(name);
            if (!animator.HasState(layer, _id))
            {
                Debug.LogError("The animator does not have state: " + name);
            }
            _animator = animator;
            _layer = layer;
            _stateName = name;
        }

        public override BtState Tick()
        {
            var state = _animator.GetCurrentAnimatorStateInfo(_layer);
            if (state.fullPathHash == _id || state.shortNameHash == _id)
                return BtState.Success;
            return BtState.Continue;
        }

        public override string ToString()
        {
            return "Wait For State : " + _stateName;
        }
    }
}