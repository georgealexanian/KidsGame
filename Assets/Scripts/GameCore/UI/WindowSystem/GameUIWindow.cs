using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCore.UI.WindowSystem
{
    public abstract class GameUIWindow : UIWindow
    {
        public abstract Task Init(params object[] args);

        public override async Task Init(IWindowManager winManager, params object[] args)
        {
            OnWindowFocus();
            await Init(args);
            await base.Init(winManager, args);
            await Task.Yield();
        }
        
        public override void OnWindowFocus()
        {
        }

    }
}