using System;
using Missions.Data;
using UI.Views;

namespace UI.Controllers
{
    public class PreMissionViewController : IDisposable
    {
        private readonly PreMissionView _view;
        private readonly PreOptionMissionView _primaryOptionView;
        private readonly PreOptionMissionView _secondaryOptionView;
        private readonly HeroGroupController _heroGroupController;

        private MissionData _missionData;

        public PreMissionViewController(PreMissionView view, HeroGroupController heroGroupController)
        {
            _view = view ? view : throw new NullReferenceException(nameof(PreMissionView));
            _heroGroupController = heroGroupController ?? throw new NullReferenceException(nameof(HeroGroupController));

            _primaryOptionView = view.GetPrimaryOptionView();
            _secondaryOptionView = view.GetSecondaryOptionView();
        }

        public void Dispose()
        {
            
        }
        
        public void ShowView(MissionData data)
        {
            _heroGroupController.OnHeroChange += OnHeroChange;
            
            _missionData = data ?? throw new NullReferenceException(nameof(MissionInfo));

            HideView();
            
            switch (data.Type)
            {
                case MissionType.Single:
                    LoadAndShowOptionView(_primaryOptionView, data.PrimaryMissionDetails);
                    break;
                case MissionType.Double:
                    LoadAndShowOptionView(_primaryOptionView, data.PrimaryMissionDetails);
                    LoadAndShowOptionView(_secondaryOptionView, data.SecondaryMissionDetails);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void HideView()
        {
            _primaryOptionView.gameObject.SetActive(false);
            _secondaryOptionView.gameObject.SetActive(false);
        }

        private void LoadAndShowOptionView(PreOptionMissionView view, MissionInfo info)
        {
            view.SetName(info.Name);
            view.SetDescription(info.Description);
            view.SetPreviewSprite(info.Preview);
            view.UpdateButtonInteractable(CanStartMission());
            
            view.gameObject.SetActive(true);
        }

        private bool CanStartMission()
        {
            return _heroGroupController.HasSelectedHero() && _missionData.State == MissionState.Active;
        }
        
        private void OnHeroChange()
        {
            _primaryOptionView.UpdateButtonInteractable(CanStartMission());
            _secondaryOptionView.UpdateButtonInteractable(CanStartMission());
        }
    }
}