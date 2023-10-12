using Infrastructure.Container;
using Pool.Application;
using Pool.Controller;
using UnityEngine;

namespace Pool.Infrastructure
{
    public class PoolInstaller : Installer
    {
        [SerializeField] private PoolController _poolController;
        
        public override void InstallBindings(Container container)
        {
            container.Bind(new PoolApplication(_poolController));
        }
    }  
}
