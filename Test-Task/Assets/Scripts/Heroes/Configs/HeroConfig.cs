using System;
using System.Collections.Generic;
using Heroes.Data;
using UnityEngine;

namespace Heroes.Configs
{
    [CreateAssetMenu(fileName = nameof(HeroConfig), menuName = "Heroes/" + nameof(HeroConfig))]
    public class HeroConfig : ScriptableObject
    {
        [SerializeField] private List<HeroData> _heroes;
        
        public List<HeroData> GetHeroes()
        {
            return _heroes;
        }
        
        public void AddHero(HeroData hero)
        {
            hero.Id = Guid.NewGuid().ToString();
            _heroes.Add(hero);
        }
    }
}