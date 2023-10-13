using System;
using System.Collections.Generic;

namespace Missions.Data
{
    [Serializable]
    public class MissionData : ICloneable
    {
        public string Id;
        public MissionType Type;
        public MissionState State;
        public MissionInfo PrimaryMissionDetails;
        public MissionInfo SecondaryMissionDetails;
        public List<string> RequiredPreviousMissions;
        public List<string> TemporarilyLockedMissions;

        public object Clone()
        {
            return new MissionData
            {
                Id = Id,
                Type = Type,
                State = State,
                PrimaryMissionDetails = (MissionInfo)PrimaryMissionDetails.Clone(),
                SecondaryMissionDetails = (MissionInfo)SecondaryMissionDetails.Clone(),
                RequiredPreviousMissions = new List<string>(RequiredPreviousMissions),
                TemporarilyLockedMissions = new List<string>(TemporarilyLockedMissions)
            };
        }
    }
}