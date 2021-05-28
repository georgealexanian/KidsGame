namespace GameCore.AI.BehaviourTree
{
    public class TerminateNode : BehTreeNode
    {

        public override BtState Tick()
        {
            return BtState.Abort;
        }

    }
}