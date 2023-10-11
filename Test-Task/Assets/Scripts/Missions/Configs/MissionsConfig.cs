using System;
using System.Collections.Generic;
using Missions.Data;
using UnityEngine;

namespace Missions.Configs
{
    [CreateAssetMenu(fileName = nameof(MissionsConfig), menuName = "Missions/" + nameof(MissionsConfig))]
    public class MissionsConfig : ScriptableObject
    {
        [SerializeField] private List<MissionData> _missions;

        public List<MissionData> GetMissions()
        {
            return _missions;
        }
        
        public void AddMission(MissionData mission)
        {
            mission.PrimaryMissionDetails.Id = Guid.NewGuid().ToString();
            if (mission.Type == MissionType.Double)
            {
                mission.SecondaryMissionDetails.Id = Guid.NewGuid().ToString();
            } 
            _missions.Add(mission);
        }
    }
}