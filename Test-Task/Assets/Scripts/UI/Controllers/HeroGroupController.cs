using System;
using System.Collections.Generic;
using Heroes.Configs;
using Heroes.Data;
using Pool.Application;
using UI.Views;
using UnityEngine;

namespace UI.Controllers
{
    public class HeroGroupController
    {
        private readonly HeroConfig _heroConfig;
        private readonly HeroView _heroViewPrefab;
        private readonly IPoolApplication _poolApplication;
        private readonly Transform _heroGroupTransform;

        private Dictionary<string, HeroView> _heroViews = new();

        public HeroGroupController(HeroConfig heroConfig, HeroView heroViewPrefab, 
            IPoolApplication poolApplication, Transform heroGroupTransform)
        {
            _heroConfig = heroConfig ? heroConfig : throw new NullReferenceException(nameof(HeroConfig));
            _heroViewPrefab = heroViewPrefab ? heroViewPrefab : throw new NullReferenceException(nameof(HeroView));
            _poolApplication = poolApplication ?? throw new NullReferenceException(nameof(IPoolApplication));
            _heroGroupTransform = heroGroupTransform ? heroGroupTransform : throw new NullReferenceException(nameof(Transform));

            InitializeHeroes();
        }

        private void InitializeHeroes()
        {
            foreach (var heroData in _heroConfig.GetHeroes())
            {
                var heroView = LoadHeroView(heroData);
                
                _heroViews.Add(heroData.Id, heroView);
            }
        }

        private HeroView LoadHeroView(HeroData data)
        {
            var heroView = _poolApplication.Create(_heroViewPrefab, _heroGroupTransform);
            
            heroView.SetAvatar(data.Avatar);
            heroView.SetAvatarBackground(data.AvatarBackground);
            heroView.SetHeroName(data.Name);
            heroView.SetPoints(data.Points.ToString());
            
            heroView.DisplaySelectMark(false);
            heroView.DisplayLockPanel(false);
            
            return heroView;
        }
    }
}

