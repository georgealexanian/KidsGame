namespace GameCore.AI.BehaviourTree
{
    public abstract class BlockNode : BranchNode
    {
        public override BtState Tick()
        {
            switch (Children[ActiveChild].Tick())
            {
                case BtState.Continue:
                    return BtState.Continue;
                default:
                {
                    ActiveChild++;
                    if (ActiveChild != Children.Count)
                        return BtState.Continue;

                    ActiveChild = 0;
                    return BtState.Success;
                }
            }
        }
    }
}