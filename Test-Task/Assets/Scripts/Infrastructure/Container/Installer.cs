using System;
using UnityEngine;

namespace Infrastructure.Container
{
    public class Installer : MonoBehaviour
    {
        public virtual void InstallBindings(Container container)
        {
            throw new NotImplementedException();
        }
    }
}