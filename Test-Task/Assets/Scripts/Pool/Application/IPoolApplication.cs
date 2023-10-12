using UnityEngine;

namespace Pool.Application
{
    public interface IPoolApplication
    {
        GameObject Create(GameObject prefab, Vector3 pos, Transform parent = null);
        
        T Create<T>(T prefab, Transform parent = null) where T : Component;
        
        void Return(GameObject createdObject);
    }
}