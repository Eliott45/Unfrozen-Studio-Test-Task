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
        [SerializeField] private PreOptionMissionView _preOptionMissionViewPrefab;
        [SerializeField] private MissionCompleteView _missionCompleteView;
        [SerializeField] private Transform _mapTransform;
        
        public override void InstallBindings(Container.Container container)
        {
            var heroGroupController = container.Resolve<HeroGroupController>();
            var missionProgressController = container.Resolve<MissionProgressController>();
            var poolApplication = container.Resolve<PoolApplication>();
            var preMissionViewController = new PreMissionViewController(_preMissionView, _preOptionMissionViewPrefab,
                heroGroupController, poolApplication);

            container.Bind(new MissionCompleteViewController(_missionCompleteView, missionProgressController));
            container.Bind(new MissionsController(_missionsConfig, _missionViewPrefab, preMissionViewController, 
                missionProgressController, poolApplication, _mapTransform));
        }
    }
}