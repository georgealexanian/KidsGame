using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.AI.BehaviourTree
{
    public static class BT
    {
        public static RootNode Root() { return new RootNode(); }
        public static SequenceNode Sequence() { return new SequenceNode(); }
        public static SelectorNode Selector(bool shuffle = false) { return new SelectorNode(shuffle); }
        public static ActionNode RunCoroutine(Func<IEnumerator<BtState>> coroutine) { return new ActionNode(coroutine); }
        public static ActionNode Call(Action fn) { return new ActionNode(fn); }
        public static ConditionalBranchNode If(Func<bool> fn) { return new ConditionalBranchNode(fn); }
        public static WhileNode While(Func<bool> fn) { return new WhileNode(fn); }
        public static ConditionNode Condition(Func<bool> fn) { return new ConditionNode(fn); }
        public static RepeatNode Repeat(int count) { return new RepeatNode(count); }
        public static WaitNode Wait(float seconds) { return new WaitNode(seconds); }
        public static TriggerNode Trigger(Animator animator, string name, bool set = true) { return new TriggerNode(animator, name, set); }
        public static WaitForAnimatorState WaitForAnimatorState(Animator animator, string name, int layer = 0) { return new WaitForAnimatorState(animator, name, layer); }
        public static SetBoolNode SetBool(Animator animator, string name, bool value) { return new SetBoolNode(animator, name, value); }
        public static SetActiveNode SetActive(GameObject gameObject, bool active) { return new SetActiveNode(gameObject, active); }
        public static WaitForAnimatorSignal WaitForAnimatorSignal(Animator animator, string name, string state, int layer = 0) { return new WaitForAnimatorSignal(animator, name, state, layer); }
        public static TerminateNode Terminate() { return new TerminateNode(); }
        public static LogNode Log(string msg) { return new LogNode(msg); }
        public static RandomSequenceNode RandomSequence(int[] weights = null) { return new RandomSequenceNode(weights); }

    }
}