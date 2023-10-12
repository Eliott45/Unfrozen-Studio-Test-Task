using System;
using System.Collections.Generic;

namespace Missions.Data
{
    [Serializable]
    public class MissionData
    {
        public string Id;
        public MissionType Type;
        public MissionState State;
        public MissionInfo PrimaryMissionDetails;
        public MissionInfo SecondaryMissionDetails;
        public List<string> RequiredPreviousMissions;
        public List<string> TemporarilyLockedMissions;
    }
}