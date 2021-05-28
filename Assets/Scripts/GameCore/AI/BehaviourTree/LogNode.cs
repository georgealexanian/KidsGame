using UnityEngine;

namespace GameCore.AI.BehaviourTree
{
    /// <summary>
    /// 
    /// </summary>
    public class LogNode : BehTreeNode
    {
        private readonly string _msg;

        public LogNode(string msg)
        {
            _msg = msg;
        }

        public override BtState Tick()
        {
            Debug.Log(_msg);
            return BtState.Success;
        }
    }
}