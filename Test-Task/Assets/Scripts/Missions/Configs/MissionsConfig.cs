using System.Collections.Generic;
using Missions.Data;
using UnityEngine;

namespace Missions.Configs
{
    [CreateAssetMenu(fileName = nameof(MissionsConfig), menuName = "Missions/" + nameof(MissionsConfig), order = 0)]
    public class MissionsConfig : ScriptableObject
    {
        [SerializeField] private List<MissionData> _missions;
    }
}