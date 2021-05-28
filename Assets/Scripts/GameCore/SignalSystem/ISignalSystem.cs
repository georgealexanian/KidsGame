using System;

namespace GameCore.SignalSystem
{
    public interface ISignalSystem : IGameManager
    {
        void Subscribe<T>(Action<T> callback) where T : Signal;
        void Unsubscribe<T>(Action<T> callback) where T : Signal;
        void Subscribe<T>(Action callback) where T : Signal;
        void Subscribe(Type sType, Action callback);
        void Unsubscribe<T>(Action callback) where T : Signal;
        void Unsubscribe(Type sType, Action callback);
        void Fire<T>(T signal) where T : Signal;
    }
}