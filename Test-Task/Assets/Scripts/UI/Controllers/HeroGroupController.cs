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

        private readonly Dictionary<HeroData, HeroView> _heroes = new();

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
            foreach (var heroData in _heroConfig.GetHeroesCopy())
            {
                var heroView = LoadHeroView(heroData);
                
                _heroes.Add(heroData, heroView);
            }
        }

        private HeroView LoadHeroView(HeroData data)
        {
            var heroView = _poolApplication.Create(_heroViewPrefab, _heroGroupTransform);
            
            heroView.SetAvatar(data.Avatar);
            heroView.SetAvatarBackground(data.AvatarBackground);
            heroView.SetHeroName(data.Name);
            heroView.SetPoints(data.Points.ToString());
            
            heroView.OnSelectButtonClick += () => OnSelectHero(data.Id);
            
            heroView.DisplaySelectMark(data.Selected);
            heroView.DisplayLockPanel(data.HeroState == HeroState.Locked);
            
            return heroView;
        }

        private void OnSelectHero(string heroId)
        {
            UnselectAll();
            
            foreach (var hero in _heroes)
            {
                if (heroId != hero.Key.Id)
                {
                    continue;
                }
                
                hero.Key.Selected = !hero.Key.Selected;
                hero.Value.DisplaySelectMark(hero.Key.Selected);
                
                break;
            }
        }

        private void UnselectAll()
        {
            foreach (var hero in _heroes)
            {
                hero.Key.Selected = false;
                hero.Value.DisplaySelectMark(false);
            }
        }
    }
}

