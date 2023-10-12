using System;
using UnityEngine;

namespace Heroes.Data
{
    [Serializable]
    public class HeroData
    {
        public string Id;
        public HeroState HeroState;
        public Sprite Avatar;
        public Sprite AvatarBackground;
        public string Name;
        public bool Selected;
        public int Points;
    }
}