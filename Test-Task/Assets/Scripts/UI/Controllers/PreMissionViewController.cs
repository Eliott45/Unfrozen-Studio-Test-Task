using System;
using System.Collections.Generic;
using Missions.Data;
using Pool.Application;
using UI.Views;

namespace UI.Controllers
{
    public class PreMissionViewController : IDisposable
    {
        public event Action<string, MissionData> OnPressStartMission;
        
        private readonly PreMissionView _view;
        private readonly PreOptionMissionView _preOptionMissionViewPrefab;
        private readonly HeroGroupController _heroGroupController;
        private readonly IPoolApplication _poolApplication;
        private readonly List<PreOptionMissionView> _optionViews = new(2);
        
        private MissionData _missionData;

        public PreMissionViewController(PreMissionView view, PreOptionMissionView preOptionMissionViewPrefab, 
            HeroGroupController heroGroupController, IPoolApplication poolApplication)
        {
            _view = view ? view : throw new NullReferenceException(nameof(PreMissionView));
            _preOptionMissionViewPrefab = preOptionMissionViewPrefab ? preOptionMissionViewPrefab : throw new NullReferenceException(nameof(PreOptionMissionView));
            
            _heroGroupController = heroGroupController ?? throw new NullReferenceException(nameof(HeroGroupController));
            _poolApplication = poolApplication ?? throw new NullReferenceException(nameof(IPoolApplication));
        }

        public void Dispose()
        {
            _heroGroupController.OnHeroChange -= OnHeroChange;
        }
        
        public void ShowView(MissionData data)
        {
            _missionData = data ?? throw new NullReferenceException(nameof(MissionInfo));

            HideView();
            
            switch (data.Type)
            {
                case MissionType.SingleOption:
                    LoadAndShowOptionView(data.MissionOptions[0]);
                    
                    break;
                case MissionType.MultipleOptions:
                    foreach (var missionOption in data.MissionOptions)
                    {
                        LoadAndShowOptionView(missionOption);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _heroGroupController.OnHeroChange += OnHeroChange;
        }
        
        public void HideView()
        {
            foreach (var optionView in _optionViews)
            {
                optionView.OnStartButton -= OnOptionPressStart;
                
                _poolApplication.Return(optionView.gameObject);
            }
            
            _optionViews.Clear();
            
            _heroGroupController.OnHeroChange -= OnHeroChange;
        }

        private void LoadAndShowOptionView(MissionInfo info)
        {
            var view = _poolApplication.Create(_preOptionMissionViewPrefab, _view.gameObject.transform);
            
            _optionViews.Add(view);
            
            view.SetId(info.Id);
            view.SetName(info.Name);
            view.SetDescription(info.PreMissionDescription);
            view.SetPreviewSprite(info.Preview);
            view.UpdateButtonInteractable(CanStartMission());

            view.OnStartButton += OnOptionPressStart;
            
            view.gameObject.SetActive(true);
        }

        private void OnOptionPressStart(string id)
        {
            OnPressStartMission?.Invoke(id, _missionData);
        }

        private bool CanStartMission()
        {
            return _heroGroupController.HasSelectedHero() && _missionData.State == MissionState.Active;
        }
        
        private void OnHeroChange()
        {
            foreach (var option in _optionViews)
            {
                option.UpdateButtonInteractable(CanStartMission());
            }
        }
    }
}