using UnityEngine;

namespace GameCore.AI.BehaviourTree
{
    /// <summary>
    /// Activate a trigger on an animator.
    /// </summary>
    public class TriggerNode : BehTreeNode
    {
        private readonly Animator _animator;
        private readonly int _id;
        private readonly string _triggerName;
        private readonly bool _set;

        //if set == false, it reset the trigger instead of setting it.
        public TriggerNode(Animator animator, string name, bool set = true)
        {
            _id = Animator.StringToHash(name);
            _animator = animator;
            _triggerName = name;
            _set = set;
        }

        public override BtState Tick()
        {
            if (_set)
                _animator.SetTrigger(_id);
            else
                _animator.ResetTrigger(_id);

            return BtState.Success;
        }

        public override string ToString()
        {
            return "Trigger : " + _triggerName;
        }
    }
}