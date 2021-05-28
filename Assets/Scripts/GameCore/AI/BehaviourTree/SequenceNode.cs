using System;

namespace GameCore.AI.BehaviourTree
{
    public class SequenceNode : BranchNode
    {
        public override BtState Tick()
        {
            var childState = Children[ActiveChild].Tick();
            switch (childState)
            {
                case BtState.Success:
                    ActiveChild++;
                    if (ActiveChild == Children.Count)
                    {
                        ActiveChild = 0;
                        return BtState.Success;
                    }
                    else
                        return BtState.Continue;
                case BtState.Failure:
                    ActiveChild = 0;
                    return BtState.Failure;
                case BtState.Continue:
                    return BtState.Continue;
                case BtState.Abort:
                    ActiveChild = 0;
                    return BtState.Abort;
            }
            throw new Exception("This should never happen, but clearly it has.");
        }
    }
}