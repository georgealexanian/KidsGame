using System;

namespace GameCore.AI.BehaviourTree
{
    public class ConditionalBranchNode : BlockNode
    {
        private readonly Func<bool> _func;
        private bool _tested;
        public ConditionalBranchNode(Func<bool> func)
        {
            _func = func;
        }
        public override BtState Tick()
        {
            if (!_tested)
            {
                _tested = _func();
            }

            if (!_tested) return BtState.Failure;
            
            var result = base.Tick();
            if (result == BtState.Continue)
                return BtState.Continue;
                
            _tested = false;
            return result;

        }

        public override string ToString()
        {
            return "ConditionalBranchNode : " + _func.Method;
        }
    }
}