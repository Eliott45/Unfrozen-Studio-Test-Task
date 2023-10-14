using System;
using Missions.Data;

namespace Missions.Controllers
{
    public class MissionProgressController
    {
        public event Action<string, MissionInfo> OnStartMission; 
        public event Action<string, MissionInfo> OnMissionComplete;
        
        public void StartMission(string id, MissionInfo mission)
        {
            OnStartMission?.Invoke(id, mission);
        }

        public void CompleteMission(string id, MissionInfo mission)
        {
            OnMissionComplete?.Invoke(id, mission);
        }
    }
}