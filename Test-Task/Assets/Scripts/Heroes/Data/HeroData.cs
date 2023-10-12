using System;
using UnityEngine;

namespace Heroes.Data
{
    [Serializable]
    public struct HeroData
    {
        public string Id;
        public Sprite Avatar;
        public Sprite AvatarBackground;
        public string Name;
        public int Points;
    }
}