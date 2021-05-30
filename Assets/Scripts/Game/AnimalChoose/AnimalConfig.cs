using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Game.AnimalChoose
{
    [Serializable]
    public class AnimalConfig
    {
        [JsonIgnore] public const int FirstInactivityAwait = 7;
        [JsonIgnore] public const int SecondInactivityAwait = 14;
        public string tailAtlasLabel;
        public string portraitAtlasLabel;
        public List<string> correctAction;
        public List<string> inCorrectAction;
        public List<Animal> animals;
    }


    [Serializable]
    public class Animal
    {
        public string name;
        public string tail;
        public string uiPrefabPath;
        public Anims anims;
        public Skins skins;
        public Phrases phrases;
    }


    [Serializable]
    public class Anims
    {
        public string no;
        public string sad;
        public string happy;
        public string tap;
        public string idle;
        public string talk;
    }


    [Serializable]
    public class Skins
    {
        public string plain;
        public string tailed;
    }


    [Serializable]
    public class Phrases
    {
        public string start;
    }
}