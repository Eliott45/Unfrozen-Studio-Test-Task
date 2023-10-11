using System;
using UnityEngine;

namespace Heroes.Data
{
    [Serializable]
    public struct HeroData
    {
        public string Id;
        public Sprite Preview;
        public string Name;
        public int Points;
    }
}