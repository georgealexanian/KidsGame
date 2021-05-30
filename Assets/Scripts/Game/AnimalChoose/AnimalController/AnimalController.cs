using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Audio;
using GameCore;
using GameCore.SignalSystem;
using GameCore.UI.WindowSystem;
using Spine;
using Spine.Unity;
using UI.Windows.TailChoose;
using UnityEngine;

namespace Game.AnimalChoose.AnimalController
{
    public class AnimalController : MonoBehaviour
    {
        #pragma warning disable
        public delegate void Talk();
        [SerializeField] private SkeletonGraphic skelGraphic;
        [SerializeField] private string animalName;
        private GameManagerReference<AnimalManager> _animalManager;
        private GameManagerReference<AudioManager> _audioManager;
        private GameManagerReference<WindowsManager> _winManager;
        private Animal _animalInfo;
        private int _wrongAnswerCounter;
        private const int SadnessThreshold = 2;
        private string _lastAnswer;
        private const int MenuSwitchAwaitTime = 3000;
        #pragma warning restore


        private void Awake()
        {
            _animalInfo = _animalManager.Value.GetAnimalInfo(animalName);
            if (_animalInfo != null && skelGraphic != null)
            {
                skelGraphic.AnimationState.Complete += OnSpineAnimationComplete;
                SetUpAnim(true, _animalInfo.anims.idle, true);
                SetUpSkin(new List<string>(){_animalInfo.skins.plain});
            }
        }


        public void StartTalking()
        {
            if (_animalInfo != null && skelGraphic != null)
            {
                SetUpAnim(true, _animalInfo.anims.talk, false);
                _audioManager.Value.PlayClip(_animalInfo.phrases.start, () =>
                {
                    SetUpAnim(true, _animalInfo.anims.idle, true);
                });
            }
        }


        private void SetUpAnim(bool withSetUp, string animName, bool loop)
        {
            if (_animalInfo == null || !skelGraphic)
            {
                return;
            }

            var trackEntry = skelGraphic.AnimationState.GetCurrent(0); 
            if (trackEntry != null && trackEntry.Animation.Name == animName) //if anim already playing
            {
                return;   
            }
            
            skelGraphic.AnimationState.ClearTracks();
            if (withSetUp)
            {
                skelGraphic.Skeleton.SetToSetupPose();
            }
            skelGraphic.AnimationState.SetAnimation(0, animName, loop);
        }
        
        
        private void SetUpSkin(List<string> skins)
        {
            if (skins == null || skins.Count == 0 || !skelGraphic)
            {
                return;
            }
            
            Skin newSkin = new Skin("skin");
            foreach (var skin in skins)
            {
                newSkin.AddSkin(skelGraphic.Skeleton.Data.FindSkin(skin));
            }
            skelGraphic.Skeleton.SetSkin(newSkin); 
            skelGraphic.Skeleton.SetSlotsToSetupPose(); 
            skelGraphic.AnimationState.Apply(skelGraphic.Skeleton); 
        }
        
        
        private void OnSpineAnimationComplete(TrackEntry trackEntry)
        {
            if (trackEntry.Animation.Name == _animalInfo.anims.no)
            {
                SetUpAnim(true, _animalInfo.anims.idle, true);
            }
            if (trackEntry.Animation.Name == _animalInfo.anims.tap)
            {
                SetUpAnim(true, _animalInfo.anims.idle, true);
            }
        }


        private void OnTailChosenSignal(TailChosenSignal signal)
        {
            if (_animalInfo == null)
            {
                return;
            }
            
            if (signal.tailName == _animalInfo.tail) //the player has chosen the CORRECT tail
            {
                CorrectAction(signal.tailName);
            }
            else //the player has chosen an INCORRECT tail
            {
                IncorrectAction(signal.tailName);
            }
        }


        private void IncorrectAction(string tailName)
        {
            Debug.Log($"<color=yellow>{nameof(IncorrectAction)}</color>");

            new ChosenCorrectAnswerSignal{isCorrect = false, tailName = tailName}.Fire();
            if (_lastAnswer == tailName && _wrongAnswerCounter >= SadnessThreshold)
            {
                Debug.Log($"<color=red>THE SAME WRONG ANSWER HAS BEEN CHOSEN MORE THAN {SadnessThreshold} times</color>");
                SetUpAnim(true, _animalInfo.anims.sad, true);
            }
            else
            {
                SetUpAnim(true, _animalInfo.anims.no, false);
            }

            if (_lastAnswer != tailName)
            {
                _wrongAnswerCounter = 0;
            }
            
            _lastAnswer = tailName;
            _wrongAnswerCounter += 1;
            
            _audioManager.Value.PlayRandomClip(_animalManager.Value.GetRandomAnswerPaths(false));
        }


        private async void CorrectAction(string tailName)
        {
            Debug.Log($"<color=yellow>{nameof(CorrectAction)}</color>");

            new ChosenCorrectAnswerSignal{isCorrect = true, tailName = tailName}.Fire();
            SetUpAnim(true, _animalInfo.anims.happy, true);
            SetUpSkin(new List<string>(){_animalInfo.skins.tailed});
            _audioManager.Value.PlayRandomClip(_animalManager.Value.GetRandomAnswerPaths(true));

            _winManager.Value.InvisibleBlockUIInput = true;
            await Task.Delay(MenuSwitchAwaitTime);
            _winManager.Value.InvisibleBlockUIInput = false;
            new StartSceneSignal(Scenes.MenuScene.ToString()).Fire();
        }


        private void OnLongAwaitActionSignal(LongAwaitActionSignal signal)
        {
            switch (signal.inactivityTime)
            {
                case AnimalConfig.FirstInactivityAwait:
                    StartTalking();
                    new PulsateSignal().Fire();
                    break;
            }
        }


        public void OnClick()
        {
            SetUpAnim(true, _animalInfo.anims.tap, false);
        }


        private void OnEnable()
        {
            Signal.Subscribe<TailChosenSignal>(OnTailChosenSignal);
            Signal.Subscribe<LongAwaitActionSignal>(OnLongAwaitActionSignal);
        }
        private void OnDisable()
        {
            Signal.Unsubscribe<TailChosenSignal>(OnTailChosenSignal);
            Signal.Unsubscribe<LongAwaitActionSignal>(OnLongAwaitActionSignal);
        }
        
    }


    [Serializable]
    public class ChosenCorrectAnswerSignal : Signal
    {
        public bool isCorrect;
        public string tailName;
    }
}
