using Heroes.Configs;
using Infrastructure.Container;
using Missions.Controllers;
using Pool.Application;
using UI.Controllers;
using UI.Views;
using UnityEngine;

namespace Infrastructure.Installers
{
    public class HeroInstallers : Installer
    {
        [SerializeField] private HeroConfig _heroConfig;
        [SerializeField] private HeroView _heroViewPrefab;
        [SerializeField] private Transform _heroGroupTransform;

        public override void InstallBindings(Container.Container container)
        {
            var poolApplication = container.Resolve<PoolApplication>();
            var missionProgressController = container.Resolve<MissionProgressController>();
            
            container.Bind(new HeroGroupController(_heroConfig, _heroViewPrefab, missionProgressController,
                poolApplication, _heroGroupTransform));
        }
    }
}