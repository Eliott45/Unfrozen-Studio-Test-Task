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
        
        public List<MissionData> GetMissionsCopy()
        {
            var copy = new List<MissionData>(_missions.Count);

            foreach (var mission in _missions)
            {
                var clonedMission = (MissionData)mission.Clone();
                copy.Add(clonedMission);
            }

            return copy;
        }
        
        public void AddMission(MissionData mission)
        {
            mission.Id = Guid.NewGuid().ToString();
            _missions.Add(mission);
        }
        
#if UNITY_EDITOR
        [ContextMenu("Generate IDs")]
		public void ConvertQuestsToJson()
		{
            foreach (var mission in _missions)
            {
                if (mission.Id == string.Empty)
                {
                    mission.Id = Guid.NewGuid().ToString();
                }
                
                foreach (var option in mission.MissionOptions)
                {
                    if (option.Id== string.Empty)
                    {
                        option.Id = Guid.NewGuid().ToString();
                    }
                }
            }
		}
#endif
    }
}