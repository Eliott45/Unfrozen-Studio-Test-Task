using System;
using Pool.Controller;
using UnityEngine;

namespace Pool.Application
{
    public class PoolApplication : IPoolApplication
    {
        private readonly IPoolController _poolController;
        
        public PoolApplication(IPoolController poolController)
        {
            _poolController = poolController ?? throw new NullReferenceException(nameof(IPoolController));
        }
        
        public GameObject Create(GameObject prefab, Vector3 pos, Transform parent = null)
        {
            return _poolController.CreateFromPool(prefab, pos, Quaternion.identity, parent);
        }

        public T Create<T>(T prefab, Transform parent = null) where T : Component
        {
            return _poolController.CreateFromPool(prefab, parent);
        }

        public void Return(GameObject createdObject)
        {
            _poolController.ReturnToPool(createdObject);
        }
    }
}