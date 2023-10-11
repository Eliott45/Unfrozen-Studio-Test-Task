using System;

namespace Missions.Data
{
    [Serializable]
    public struct MissionData
    {
        public MissionType Type;
        public MissionState State;
        public MissionInfo Info;
    }
}