namespace GameCore.AI.BehaviourTree
{
    /// <summary>
    /// Run a block of children a number of times.
    /// </summary>
    public class RepeatNode : BlockNode
    {
        private readonly int _count;
        private int _currentCount;
        public RepeatNode(int count)
        {
            _count = count;
        }
        public override BtState Tick()
        {
            if (_count > 0 && _currentCount < _count)
            {
                var result = base.Tick();
                switch (result)
                {
                    case BtState.Continue:
                        return BtState.Continue;
                    default:
                        _currentCount++;
                        if (_currentCount == _count)
                        {
                            _currentCount = 0;
                            return BtState.Success;
                        }
                        return BtState.Continue;
                }
            }
            return BtState.Success;
        }

        public override string ToString()
        {
            return "Repeat Until : " + _currentCount + " / " + _count;
        }
    }
}