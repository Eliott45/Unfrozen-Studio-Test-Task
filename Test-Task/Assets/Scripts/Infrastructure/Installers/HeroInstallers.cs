using Heroes.Configs;
using Infrastructure.Container;
using UI.Controllers;
using UI.Views;
using UnityEngine;

namespace Infrastructure.Installers
{
    public class HeroInstallers : Installer
    {
        [SerializeField] private HeroConfig _heroConfig;
        [SerializeField] private HeroView _heroViewPrefab;

        public override void InstallBindings(Container.Container container)
        {
            container.Bind(_heroConfig);
            container.Bind(_heroViewPrefab);
            container.Bind(new HeroGroupController(_heroConfig, _heroViewPrefab));
        }
    }
}