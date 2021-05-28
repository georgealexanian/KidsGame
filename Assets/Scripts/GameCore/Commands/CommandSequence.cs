using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Commands
{
    public class CommandSequence
    {
        public Queue<ICommand> Commands { get; private set; }

        public bool IsCompleted { get; private set; }

        public CommandSequence()
        {
            Commands = new Queue<ICommand>();
        }

        public CommandSequence Append(ICommand command)
        {
            Commands.Enqueue(command);
            if (IsCompleted)
            {
                IsCompleted = false;
                ExecuteNextCommand();
            }

            return this;
        }

        public CommandSequence Start()
        {
            ExecuteNextCommand();
            return this;
        }

        private void ExecuteNextCommand()
        {
            if (Commands.Count <= 0)
            {
                IsCompleted = true;
                return;
            }

            var command = Commands.Dequeue();
            command.OnFinished += ExecuteNextCommand;
            command.Execute();
        }
    }
}