using System;
using UnityEngine;

namespace Heroes.Data
{
    [Serializable]
    public class HeroData
    {
        public string Name;
        public string Id;
        public HeroState HeroState;
        public Sprite Avatar;
        public Sprite AvatarBackground;
        public bool Selected;
        public int Points;
    }
}