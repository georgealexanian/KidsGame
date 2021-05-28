using UnityEngine;

namespace GameCore.AI.BehaviourTree
{
    public class RandomSequenceNode : BlockNode
    {
        private readonly int[] _weight;
        private int[] _addedWeight;

        /// <summary>
        /// Will select one random child every fireTime it get triggered again
        /// </summary>
        /// <param name="weight">Leave null so that all child node have the same weight. 
        /// If there is less weight than children, all subsequent child will have weight = 1</param>
        public RandomSequenceNode(int[] weight = null)
        {
            ActiveChild = -1;

            _weight = weight;
        }

        public override BranchNode OpenBranch(params BehTreeNode[] children)
        {
            _addedWeight = new int[children.Length];

            for (int i = 0; i < children.Length; ++i)
            {
                int weight = 0;
                int previousWeight = 0;

                if (_weight == null || _weight.Length <= i)
                {//if we don't have weight for that one, we set the weight to one
                    weight = 1;
                }
                else
                    weight = _weight[i];

                if (i > 0)
                    previousWeight = _addedWeight[i - 1];

                _addedWeight[i] = weight + previousWeight;
            }

            return base.OpenBranch(children);
        }

        public override BtState Tick()
        {
            if (ActiveChild == -1)
                PickNewChild();

            var result = Children[ActiveChild].Tick();

            switch (result)
            {
                case BtState.Continue:
                    return BtState.Continue;
                default:
                    PickNewChild();
                    return result;
            }
        }

        private void PickNewChild()
        {
            int choice = Random.Range(0, _addedWeight[_addedWeight.Length - 1]);

            for (int i = 0; i < _addedWeight.Length; ++i)
            {
                if (choice - _addedWeight[i] <= 0)
                {
                    ActiveChild = i;
                    break;
                }
            }
        }

        public override string ToString()
        {
            return "Random Sequence : " + ActiveChild + "/" + Children.Count;
        }
    }
}