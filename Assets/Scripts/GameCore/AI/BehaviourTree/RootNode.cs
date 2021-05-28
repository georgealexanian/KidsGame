namespace GameCore.AI.BehaviourTree
{
    public class RootNode : BlockNode
    {
        private bool _isTerminated;

        public override BtState Tick()
        {
            if (_isTerminated) return BtState.Abort;
            while (true)
            {
                switch (Children[ActiveChild].Tick())
                {
                    case BtState.Continue:
                        return BtState.Continue;
                    case BtState.Abort:
                        _isTerminated = true;
                        return BtState.Abort;
                    default:
                        ActiveChild++;
                        if (ActiveChild == Children.Count)
                        {
                            ActiveChild = 0;
                            return BtState.Success;
                        }
                        continue;
                }
            }
        }
    }
}