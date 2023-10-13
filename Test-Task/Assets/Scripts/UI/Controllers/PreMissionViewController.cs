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

        private MissionData _missionData;

        public PreMissionViewController(PreMissionView view)
        {
            _view = view ? view : throw new NullReferenceException(nameof(PreMissionView));

            _primaryOptionView = view.GetPrimaryOptionView();
            _secondaryOptionView = view.GetSecondaryOptionView();
        }

        public void Dispose()
        {
            
        }
        
        public void ShowView(MissionData data)
        {
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
            
            view.gameObject.SetActive(true);
        }
    }
}