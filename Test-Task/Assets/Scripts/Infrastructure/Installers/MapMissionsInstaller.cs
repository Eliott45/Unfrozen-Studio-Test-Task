using Infrastructure.Container;
using Missions.Configs;
using Missions.Controllers;
using Pool.Application;
using UI.Views;
using UnityEngine;

namespace Infrastructure.Installers
{
    public class MapMissionsInstaller : Installer
    {
        [SerializeField] private MissionsConfig _missionsConfig;
        [SerializeField] private MissionView _missionViewPrefab;
        [SerializeField] private Transform _mapTransform;
        
        public override void InstallBindings(Container.Container container)
        {
            container.Bind(new MapMissionsController(_missionsConfig, _missionViewPrefab, 
                container.Resolve<PoolApplication>(), _mapTransform));
        }
    }
}