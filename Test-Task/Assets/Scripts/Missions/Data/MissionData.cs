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
        public MissionInfo[] MissionOptions;
        public List<string> RequiredPreviousMissions;
        public List<string> RequiredPreviousOptions;
        public List<string> TemporarilyLockedMissions;

        public object Clone()
        {
            var clonedMissionInfos = new MissionInfo[MissionOptions.Length];
            
            for (var i = 0; i < MissionOptions.Length; i++)
            {
                clonedMissionInfos[i] = (MissionInfo)MissionOptions[i].Clone();
            }
            
            return new MissionData
            {
                Id = Id,
                Type = Type,
                State = State,
                MissionOptions = clonedMissionInfos,
                RequiredPreviousMissions = new List<string>(RequiredPreviousMissions),
                TemporarilyLockedMissions = new List<string>(TemporarilyLockedMissions)
            };
        }
    }
}