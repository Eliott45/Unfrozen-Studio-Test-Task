using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Container
{
    public class SceneContext : MonoBehaviour
    {
        [SerializeField] private List<Installer> _installers = new();

        private readonly Container _sceneContextContainer = new();
        
        private void Awake()
        {
            foreach (var installer in _installers)
            {
                installer.InstallBindings(_sceneContextContainer);
            }
        }
    }
}