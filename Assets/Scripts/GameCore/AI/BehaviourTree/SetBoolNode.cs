using UnityEngine;

namespace GameCore.AI.BehaviourTree
{
    /// <summary>
    /// Set a boolean on an animator.
    /// </summary>
    public class SetBoolNode : BehTreeNode
    {
        private readonly Animator _animator;
        private readonly int _id;
        private readonly bool _value;
        private readonly string _triggerName;

        public SetBoolNode(Animator animator, string name, bool value)
        {
            _id = Animator.StringToHash(name);
            _animator = animator;
            _value = value;
            _triggerName = name;
        }

        public override BtState Tick()
        {
            _animator.SetBool(_id, _value);
            return BtState.Success;
        }

        public override string ToString()
        {
            return "SetBool : " + _triggerName + " = " + _value;
        }
    }
}