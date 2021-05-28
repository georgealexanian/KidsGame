namespace GameCore.AI.BehaviourTree
{
    public abstract class DecoratorNode : BehTreeNode
    {
        protected BehTreeNode Child;
        public DecoratorNode Do(BehTreeNode child)
        {
            Child = child;
            return this;
        }
    }
}