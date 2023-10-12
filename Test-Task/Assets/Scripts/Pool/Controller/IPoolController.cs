using UnityEngine;

namespace Pool.Controller
{
    public interface IPoolController
    {
        T CreateFromPool<T>(T prefab, Transform parent) where T : Component;
        
        GameObject CreateFromPool(GameObject prefab, Vector3 pos, Quaternion rotation, Transform parent = null);
        
        void ReturnToPool(GameObject createdObject);
    }
}