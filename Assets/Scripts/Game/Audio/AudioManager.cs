using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCore;
using GameCore.SignalSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Audio
{
    public class AudioManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private AudioSource audioSource;


        private void Awake()
        {
            ManagersHolder.AddManager(this);
            Signal.Subscribe<PlayClipSignal>(OnPlayClipSignal);
        }


        private void OnPlayClipSignal(PlayClipSignal signal)
        {
            PlayClip(signal.clipPath);
        }


        public async void PlayClip(string path, Action callBack = null)
        {
            audioSource.Stop();
            AudioClip clip = Resources.Load<AudioClip>(path);
            audioSource.PlayOneShot(clip);

            await Task.Delay(Mathf.CeilToInt(clip.length) * 1000);
            callBack?.Invoke();
        }


        public void PlayRandomClip(List<string> paths)
        {
            var path = paths[Random.Range(0, paths.Count)];
            PlayClip(path);
        }


        private void OnDestroy()
        {
            Signal.Unsubscribe<PlayClipSignal>(OnPlayClipSignal);
        }
    }
}


[Serializable]
public class PlayClipSignal : Signal
{
    public string clipPath;
}
