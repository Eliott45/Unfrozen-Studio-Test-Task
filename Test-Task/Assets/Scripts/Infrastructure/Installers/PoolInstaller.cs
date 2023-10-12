using Infrastructure.Container;
using Pool.Application;
using Pool.Controller;
using UnityEngine;

namespace Infrastructure.Installers
{
    public class PoolInstaller : Installer
    {
        [SerializeField] private PoolController _poolController;
        
        public override void InstallBindings(Container.Container container)
        {
            container.Bind(new PoolApplication(_poolController));
        }
    }  
}
