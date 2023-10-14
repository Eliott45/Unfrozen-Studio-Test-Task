using System;
using UnityEngine;

namespace Missions.Data
{
    [Serializable]
    public class MissionInfo : ICloneable
    {
        public string Id;
        public Sprite Preview;
        public Vector2 MapPosition;
        public string MapDisplayName;
        public string Name;
        public string PreMissionDescription;
        public string Description;
        public string PlayerSide;
        public string EnemySide;
        public MissionReward Reward;
        public bool Selected;
        
        public object Clone()
        {
            return new MissionInfo
            {
                Id = Id,
                Preview = Preview,
                MapPosition = MapPosition,
                MapDisplayName = MapDisplayName,
                Name = Name,
                PreMissionDescription = PreMissionDescription,
                Description = Description,
                PlayerSide = PlayerSide,
                EnemySide = EnemySide,
                Reward = Reward,
                Selected = Selected
            };
        }
    }
}