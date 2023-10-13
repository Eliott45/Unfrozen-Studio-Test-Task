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
        public event Action OnHeroChange;
        
        private readonly HeroConfig _heroConfig;
        private readonly HeroView _heroViewPrefab;
        private readonly IPoolApplication _poolApplication;
        private readonly Transform _heroGroupTransform;

        private readonly Dictionary<string, HeroViewController> _heroControllers = new();
        private readonly Dictionary<string, HeroData> _heroData = new();

        private HeroData _selectedHero;

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
                
                _heroControllers.Add(heroData.Id, heroViewController);
                _heroData.Add(heroData.Id, heroData);
            }
        }
        
        public void Dispose()
        {
            foreach (var hero in _heroControllers)
            {
                hero.Value.Dispose();
            }
        }

        public bool HasSelectedHero()
        {
            return _selectedHero != null;
        }
        
        private HeroViewController InitHeroViewControllers(HeroData data)
        {
            var heroView = _poolApplication.Create(_heroViewPrefab, _heroGroupTransform);
            var heroViewController = new HeroViewController(heroView, data);
            
            heroViewController.OnHeroSelect += OnHeroSelect;
            
            heroViewController.Initialize();
            
            return heroViewController;
        }

        private void UnselectAll()
        {
            _selectedHero = null;
            foreach (var hero in _heroControllers)
            {
                hero.Value.Unselect();
            }
            OnHeroChange?.Invoke();
        }

        private void OnHeroSelect(string heroId)
        {
            if (heroId == _selectedHero?.Id)
            {
                UnselectAll();
                return;
            }
            
            UnselectAll();
            
            _heroControllers[heroId].Select();
            _selectedHero = _heroData[heroId];
            
            OnHeroChange?.Invoke();
        }
    }
}

