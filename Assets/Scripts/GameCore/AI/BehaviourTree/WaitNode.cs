using UnityEngine;

namespace GameCore.AI.BehaviourTree
{
    /// <summary>
    /// Pause execution for a number of seconds.
    /// </summary>
    public class WaitNode : BehTreeNode
    {
        public float Seconds;
        private readonly bool _unscaledTime;
        private float _future = -1;
        public WaitNode(float seconds, bool unscaledTime = false)
        {
            Seconds = seconds;
            _unscaledTime = unscaledTime;
        }

        public override BtState Tick()
        {
            if (_future < 0)
            {
                _future = _unscaledTime ? Time.unscaledTime : Time.time + Seconds;
            }

            if ((_unscaledTime ? Time.unscaledTime : Time.time)  >= _future)
            {
                _future = -1;
                return BtState.Success;
            }

            return BtState.Continue;
        }

        public override string ToString()
        {
            return "Wait : " + (_future - Time.time) + " / " + Seconds;
        }
    }
}