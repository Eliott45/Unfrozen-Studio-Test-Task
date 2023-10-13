using System;
using System.Collections.Generic;
using Heroes.Configs;
using Heroes.Data;
using Pool.Application;
using UI.Views;
using UnityEngine;

namespace UI.Controllers
{
    public class HeroGroupController : IDisposable
    {
        private readonly HeroConfig _heroConfig;
        private readonly HeroView _heroViewPrefab;
        private readonly IPoolApplication _poolApplication;
        private readonly Transform _heroGroupTransform;

        private readonly Dictionary<string, HeroViewController> _heroes = new();

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
                var heroViewController = InitHeroViewControllers(heroData);
                
                _heroes.Add(heroData.Id, heroViewController);
            }
        }
        
        public void Dispose()
        {
            foreach (var hero in _heroes)
            {
                hero.Value.Dispose();
            }
        }
        
        private HeroViewController InitHeroViewControllers(HeroData data)
        {
            var heroView = _poolApplication.Create(_heroViewPrefab, _heroGroupTransform);
            var heroViewController = new HeroViewController(heroView, data);
            
            heroViewController.OnHeroSelect += OnHeroSelect;
            
            heroViewController.Initialize();
            
            return heroViewController;
        }

        private void OnHeroSelect(string heroId)
        {
            UnselectAll();
            _heroes[heroId].Select();
        }

        private void UnselectAll()
        {
            foreach (var hero in _heroes)
            {
                hero.Value.Unselect();
            }
        }
    }
}

