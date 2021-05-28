using System;
using System.Collections.Generic;

namespace GameCore.AI.BehaviourTree
{
    /// <summary>
    /// Call a method, or run a coroutine.
    /// </summary>
    public class ActionNode : BehTreeNode
    {
        private readonly Action _action;
        private readonly Func<IEnumerator<BtState>> _coroutineFactory;
        private IEnumerator<BtState> _coroutine;
        public ActionNode(Action action)
        {
            _action = action;
        }
        public ActionNode(Func<IEnumerator<BtState>> coroutineFactory)
        {
            _coroutineFactory = coroutineFactory;
        }
        public override BtState Tick()
        {
            if (_action != null)
            {
                _action();
                return BtState.Success;
            }

            if (_coroutine == null)
                _coroutine = _coroutineFactory();
            
            if (!_coroutine.MoveNext())
            {
                _coroutine = null;
                return BtState.Success;
            }
            
            var result = _coroutine.Current;
            if (result == BtState.Continue)
                return BtState.Continue;
            
            _coroutine = null;
            return result;
        }

        public override string ToString()
        {
            return "Action : " + _action.Method;
        }
    }
}