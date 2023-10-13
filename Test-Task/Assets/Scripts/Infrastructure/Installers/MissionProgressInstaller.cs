using Infrastructure.Container;
using Missions.Controllers;

namespace Infrastructure.Installers
{
    public class MissionProgressInstaller : Installer
    {
        public override void InstallBindings(Container.Container container)
        {
            container.Bind(new MissionProgressController());
        }
    }
}