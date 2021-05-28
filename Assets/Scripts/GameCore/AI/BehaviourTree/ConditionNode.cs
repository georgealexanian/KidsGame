using System;

namespace GameCore.AI.BehaviourTree
{
    /// <summary>
    /// Call a method, returns success if method returns true, else returns failure.
    /// </summary>
    public class ConditionNode : BehTreeNode
    {
        private readonly Func<bool> _func;

        public ConditionNode(Func<bool> func)
        {
            _func = func;
        }
        public override BtState Tick()
        {
            return _func() ? BtState.Success : BtState.Failure;
        }

        public override string ToString()
        {
            return "ConditionNode : " + _func.Method;
        }
    }
}