using System;
using DG.Tweening;

namespace GameCore.Commands.CustomCommands
{
    public class CommandWait : ICommand
    {
        public Action OnFinished { get; set; }

        private readonly float _seconds;

        public CommandWait(float seconds)
        {
            _seconds = seconds;
        }

        public void Execute()
        {
            DOTween.Sequence().AppendInterval(_seconds).AppendCallback(() => OnFinished.Invoke());
        }
    }
}