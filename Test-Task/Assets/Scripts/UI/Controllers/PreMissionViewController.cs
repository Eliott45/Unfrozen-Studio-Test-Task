using System;
using Missions.Data;
using UI.Views;

namespace UI.Controllers
{
    public class PreMissionViewController : IDisposable
    {
        public event Action OnStartMission;
        
        private readonly PreMissionView _view;

        private MissionInfo _missionInfo;
        
        public PreMissionViewController(PreMissionView view)
        {
            _view = view ? view : throw new NullReferenceException(nameof(PreMissionView));
        }

        public void Initialize(MissionInfo missionInfo)
        {
            _missionInfo = missionInfo ?? throw new NullReferenceException(nameof(MissionInfo));

            _view.SetName(_missionInfo.Name);
            _view.SetDescription(_missionInfo.Description);
            _view.SetPreviewSprite(_missionInfo.Preview);
            _view.OnStartButton += OnStartMission;
        }

        public void Dispose()
        {
            _view.OnStartButton -= OnStartMission;
        }

        public void ShowView()
        {
            _view.gameObject.SetActive(true);
        }

        public void HideView()
        {
            _view.gameObject.SetActive(false);
        }
    }
}