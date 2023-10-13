using Infrastructure.Container;
using Missions.Configs;
using Missions.Controllers;
using Pool.Application;
using UI.Controllers;
using UI.Views;
using UnityEngine;

namespace Infrastructure.Installers
{
    public class MissionsInstaller : Installer
    {
        [SerializeField] private MissionsConfig _missionsConfig;
        [SerializeField] private MissionView _missionViewPrefab;
        [SerializeField] private PreMissionView _preMissionView;
        [SerializeField] private Transform _mapTransform;
        
        public override void InstallBindings(Container.Container container)
        {
            var preMissionViewController = new PreMissionViewController(_preMissionView, 
                container.Resolve<HeroGroupController>());

            container.Bind(new MissionsController(_missionsConfig, _missionViewPrefab, preMissionViewController
               , container.Resolve<PoolApplication>(), _mapTransform));
        }
    }
}