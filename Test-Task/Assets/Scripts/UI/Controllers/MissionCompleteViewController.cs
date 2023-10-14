using System;
using Missions.Controllers;
using Missions.Data;
using UI.Views;

namespace UI.Controllers
{
    public class MissionCompleteViewController : IDisposable
    {
        private readonly MissionCompleteView _missionCompleteView;
        private readonly MissionProgressController _missionProgressController;

        private string _currentMissionId;
        private MissionInfo _currentMissionInfo;
        
        public MissionCompleteViewController(MissionCompleteView view, MissionProgressController progressController)
        {
            _missionCompleteView = view ? view : throw new NullReferenceException(nameof(MissionCompleteView));
            _missionProgressController = progressController ?? throw new NullReferenceException(nameof(MissionProgressController));

            _missionProgressController.OnStartMission += OnMissionStart;
        }

        public void Dispose()
        {
            _missionProgressController.OnStartMission -= OnMissionStart;
            _missionCompleteView.OnCompleteButtonClick -= OnPressMissionComplete;
        }

        private void OnMissionStart(string id, MissionInfo missionInfo)
        {
            _currentMissionId = id;
            _currentMissionInfo = missionInfo;
            
            _missionCompleteView.gameObject.SetActive(true);
            
            _missionCompleteView.SetName(missionInfo.Name);
            _missionCompleteView.SetDescription(missionInfo.Description);
            _missionCompleteView.SetPlayerSide(missionInfo.PlayerSide);
            _missionCompleteView.SetEnemySide(missionInfo.EnemySide);
            _missionCompleteView.SetMissionImage(missionInfo.Preview);
            
            _missionCompleteView.OnCompleteButtonClick += OnPressMissionComplete;
        }

        private void OnPressMissionComplete()
        {
            _missionCompleteView.OnCompleteButtonClick -= OnPressMissionComplete;
            
            _missionCompleteView.gameObject.SetActive(false);
            
            _missionProgressController.CompleteMission(_currentMissionId, _currentMissionInfo);
            
            _currentMissionId = string.Empty;
            _currentMissionInfo = null;
        }
    }
}