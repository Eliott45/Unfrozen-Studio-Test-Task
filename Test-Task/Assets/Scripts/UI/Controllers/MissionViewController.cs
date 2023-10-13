using System;
using Missions.Data;
using UI.Views;
using UnityEngine;

namespace UI.Controllers
{
    public class MissionViewController
    {
        public event Action<string> OnPressSelectMission;
        
        private readonly MissionView _missionView;
        
        private string _missionId;
        private MissionInfo _missionInfo;
        
        public MissionViewController(MissionView view)
        {
            _missionView = view ? view : throw new NullReferenceException(nameof(MissionView));

        }
        
        public void LoadData(MissionInfo info, string missionId)
        {
            _missionInfo = info ?? throw new NullReferenceException(nameof(MissionData));
            _missionId = missionId;
            
            _missionView.SetMissionName(_missionInfo.MapDisplayName);
            _missionView.gameObject.transform.localPosition = _missionInfo.MapPosition;
            _missionView.OnSelectButtonClick += OnSelectMission;
        }

        public void Dispose()
        {
            _missionView.OnSelectButtonClick -= OnSelectMission;
        }

        public void ApplyState(MissionState state)
        {
            switch (state)
            {
                case MissionState.Active:
                    _missionView.gameObject.SetActive(true);
                    _missionView.DisplayFadePanel(false);
                    break;
                case MissionState.Locked:
                    _missionView.gameObject.SetActive(false);
                    break;
                case MissionState.TemporarilyLocked:
                case MissionState.Completed:
                    _missionView.gameObject.SetActive(true);
                    _missionView.DisplayFadePanel(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        
        private void OnSelectMission()
        {
            OnPressSelectMission?.Invoke(_missionId);
        }
    }
}