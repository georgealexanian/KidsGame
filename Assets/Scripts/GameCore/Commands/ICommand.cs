using System;
using UnityEngine.SocialPlatforms.Impl;

namespace GameCore.Commands
{
    public interface ICommand
    {
        Action OnFinished { get; set; }
        void Execute();
    }
}