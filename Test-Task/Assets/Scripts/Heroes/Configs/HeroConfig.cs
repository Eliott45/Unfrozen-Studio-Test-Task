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
        
        public List<HeroData> GetHeroesCopy()
        {
            var copy = new List<HeroData>(_heroes.Count);
            
            foreach (var hero in _heroes)
            {
                var copiedHero = new HeroData
                {
                    Id = hero.Id,
                    HeroState = hero.HeroState,
                    Avatar = hero.Avatar,
                    AvatarBackground = hero.AvatarBackground,
                    Name = hero.Name,
                    Selected = hero.Selected,
                    Points = hero.Points
                };
                
                copy.Add(copiedHero);
            }
            
            return copy;
        }
        
        public void AddHero(HeroData hero)
        {
            hero.Id = Guid.NewGuid().ToString();
            _heroes.Add(hero);
        }
    }
}