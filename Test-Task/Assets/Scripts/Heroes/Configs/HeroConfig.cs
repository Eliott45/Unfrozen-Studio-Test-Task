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
                var copiedHero = (HeroData)hero.Clone();

                copy.Add(copiedHero);
            }
            
            return copy;
        }
        
        public void AddHero(HeroData hero)
        {
            hero.Id = Guid.NewGuid().ToString();
            _heroes.Add(hero);
        }
        
#if UNITY_EDITOR
        [ContextMenu("Generate IDs")]
		public void ConvertQuestsToJson()
		{
            foreach (var hero in _heroes)
            {
                hero.Id = Guid.NewGuid().ToString();
            }
		}
#endif
    }
}