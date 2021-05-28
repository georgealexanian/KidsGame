using UnityEngine;

namespace GameCore.AI.BehaviourTree
{
    /// <summary>
    /// Set a gameobject active flag.
    /// </summary>
    public class SetActiveNode : BehTreeNode
    {
        private readonly GameObject _gameObject;
        private readonly bool _active;

        public SetActiveNode(GameObject gameObject, bool active)
        {
            _gameObject = gameObject;
            _active = active;
        }

        public override BtState Tick()
        {
            _gameObject.SetActive(_active);
            return BtState.Success;
        }

        public override string ToString()
        {
            return "Set Active : " + _gameObject.name + " = " + _active;
        }
    }
}