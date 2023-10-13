using System;
using System.Collections.Generic;
using Heroes.Configs;
using Heroes.Data;
using Missions.Controllers;
using Missions.Data;
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
        private readonly MissionProgressController _missionProgressController;
        private readonly IPoolApplication _poolApplication;
        private readonly Transform _heroGroupTransform;

        private readonly Dictionary<string, HeroViewController> _heroControllers = new();
        private readonly Dictionary<string, HeroData> _heroData = new();

        private HeroData _selectedHero;

        public HeroGroupController(HeroConfig heroConfig, HeroView heroViewPrefab, MissionProgressController missionProgressController,
            IPoolApplication poolApplication, Transform heroGroupTransform)
        {
            _heroConfig = heroConfig ? heroConfig : throw new NullReferenceException(nameof(HeroConfig));
            _heroViewPrefab = heroViewPrefab ? heroViewPrefab : throw new NullReferenceException(nameof(HeroView));
            _missionProgressController = missionProgressController ?? throw new NullReferenceException(nameof(MissionProgressController));
            _poolApplication = poolApplication ?? throw new NullReferenceException(nameof(IPoolApplication));
            _heroGroupTransform = heroGroupTransform ? heroGroupTransform : throw new NullReferenceException(nameof(Transform));

            InitializeHeroes();
        }

        private void InitializeHeroes()
        {
            _missionProgressController.OnMissionComplete += OnMissionComplete;
            
            foreach (var heroData in _heroConfig.GetHeroesCopy())
            {
                var heroViewController = InitHeroViewControllers(heroData);
                
                _heroControllers.Add(heroData.Id, heroViewController);
                _heroData.Add(heroData.Id, heroData);
            }
        }
        
        public void Dispose()
        {
            _missionProgressController.OnMissionComplete -= OnMissionComplete;
            
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
            var heroViewController = new HeroViewController(heroView);
            
            heroViewController.OnPressSelectHero += OnPressSelectHero;
            
            heroViewController.LoadData(data);
            
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
        
        private void UpdateHeroPoints(MissionReward reward)
        {
            _selectedHero.Points += reward.HeroScore;
            _heroControllers[_selectedHero.Id].LoadData(_selectedHero);
        }

        private void UpdateOtherHeroesPoints(MissionReward reward)
        {
            foreach (var heroRewardScore in reward.OtherHeroScore)
            {
                var hero = _heroData[heroRewardScore.HeroId];
                hero.Points += heroRewardScore.Points;
                _heroControllers[hero.Id].LoadData(hero);
            }
        }

        private void UnlockHeroes(MissionReward reward)
        {
            foreach (var unlockedHeroId in reward.UnlockedHeroes)
            {
                var hero = _heroData[unlockedHeroId];
                hero.HeroState = HeroState.Available;
                _heroControllers[hero.Id].LoadData(hero);
            }
        }

        private void OnPressSelectHero(string heroId)
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

        private void OnMissionComplete(string id, MissionReward reward)
        {
            UpdateHeroPoints(reward);
            UpdateOtherHeroesPoints(reward);
            UnlockHeroes(reward);
            UnselectAll();
        }
    }
}

