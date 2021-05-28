using System;

namespace GameCore.AI.BehaviourTree
{
    /// <summary>
    /// Run all children, while method returns true.
    /// </summary>
    public class WhileNode : BlockNode
    {
        private readonly Func<bool> _fn;

        public WhileNode(Func<bool> fn)
        {
            _fn = fn;
        }

        public override BtState Tick()
        {
            if (_fn())
                base.Tick();
            else
            {
                //if we exit the loop
                ResetChildren();
                return BtState.Failure;
            }

            return BtState.Continue;
        }

        public override string ToString()
        {
            return "While : " + _fn.Method;
        }
    }
}