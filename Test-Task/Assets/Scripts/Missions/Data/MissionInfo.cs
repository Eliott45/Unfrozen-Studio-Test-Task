using System;

namespace Missions.Data
{
    [Serializable]
    public struct MissionInfo
    {
        public string Name;
        public string PreMissionDescription;
        public string Description;
        public string PlayerSide;
        public string EnemySide;
    }
}