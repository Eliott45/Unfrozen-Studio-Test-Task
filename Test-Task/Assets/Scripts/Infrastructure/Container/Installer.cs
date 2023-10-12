using UnityEngine;

namespace Infrastructure.Container
{
    public abstract class Installer : MonoBehaviour
    {
        public abstract void InstallBindings(Container container);
    }
}