using System;
using UnityEngine;

namespace Heroes.Data
{
    [Serializable]
    public class HeroData : ICloneable
    {
        public string Name;
        public string Id;
        public HeroState HeroState;
        public Sprite Avatar;
        public Sprite AvatarBackground;
        public bool Selected;
        public int Points;
        
        public object Clone()
        {
            return new HeroData
            {
                Name = Name,
                Id = Id,
                HeroState = HeroState,
                Avatar = Avatar,
                AvatarBackground = AvatarBackground,
                Selected = Selected,
                Points = Points
            };
        }
    }
}