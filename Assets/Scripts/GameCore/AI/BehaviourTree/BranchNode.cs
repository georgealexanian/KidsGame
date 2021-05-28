using System.Collections.Generic;

namespace GameCore.AI.BehaviourTree
{
    public abstract class BranchNode : BehTreeNode
    {
        protected int ActiveChild;
        protected readonly List<BehTreeNode> Children = new List<BehTreeNode>();
        
        public List<BehTreeNode> AllChildren => Children;
        public int GetActiveChild => ActiveChild;

        public virtual BranchNode OpenBranch(params BehTreeNode[] children)
        {
            foreach (var btNode in children)
                Children.Add(btNode);

            return this;
        }
        
        public virtual void ResetChildren()
        {
            ActiveChild = 0;
            foreach (var children in Children)
            {
                if (children is BranchNode b)
                {
                    b.ResetChildren();
                }
            }
        }
    }
}