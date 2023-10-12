using System;
using System.Collections.Generic;
using Heroes.Configs;
using UI.Views;

namespace UI.Controllers
{
    public class HeroGroupController
    {
        private readonly HeroConfig _heroConfig;
        private readonly HeroView _heroViewPrefab;

        private Dictionary<string, HeroView> _heroViews = new();

        public HeroGroupController(HeroConfig heroConfig, HeroView heroViewPrefab)
        {
            _heroConfig = heroConfig ? heroConfig : throw new NullReferenceException(nameof(HeroConfig));
            _heroViewPrefab = heroViewPrefab ? heroViewPrefab : throw new NullReferenceException(nameof(HeroView));
        }

        public void InitializeHeroes()
        {
            foreach (var heroData in _heroConfig.GetHeroes())
            {
                // TODO create heroes 
            }
        }
    }
}

