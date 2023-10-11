using System;
using UnityEngine;

namespace Missions.Data
{
    [Serializable]
    public struct MissionInfo
    {
        public string Id;
        public Vector2 MapPosition;
        public string MapDisplayName;
        public string Name;
        public string PreMissionDescription;
        public string Description;
        public string PlayerSide;
        public string EnemySide;
        public MissionReward Reward;
    }
}