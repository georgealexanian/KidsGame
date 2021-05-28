using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameCore.AI.BehaviourTree
{
    /// <summary>
    /// Execute each child until a child succeeds, then return success.
    /// If no child succeeds, return a failure.
    /// </summary>
    public class SelectorNode : BranchNode
    {
        public SelectorNode(bool shuffle)
        {
            if (!shuffle) return;
            
            var n = Children.Count;
            while (n > 1)
            {
                n--;
                var k = Mathf.FloorToInt(Random.value * (n + 1));
                var value = Children[k];
                Children[k] = Children[n];
                Children[n] = value;
            }
        }

        public override BtState Tick()
        {
            var childState = Children[ActiveChild].Tick();
            switch (childState)
            {
                case BtState.Success:
                    ActiveChild = 0;
                    return BtState.Success;
                case BtState.Failure:
                    ActiveChild++;
                    if (ActiveChild == Children.Count)
                    {
                        ActiveChild = 0;
                        return BtState.Failure;
                    }
                    else
                        return BtState.Continue;
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