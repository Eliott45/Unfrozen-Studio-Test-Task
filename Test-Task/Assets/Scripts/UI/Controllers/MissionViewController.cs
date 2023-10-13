using System;
using Missions.Data;
using UI.Views;
using UnityEngine;

namespace UI.Controllers
{
    public class MissionViewController
    {
        public event Action<string> OnMissionSelect;

        private readonly string _missionId;
        private readonly MissionInfo _missionInfo;
        private readonly MissionView _missionView;
        
        public MissionViewController(MissionView view, MissionInfo info, string missionId)
        {
            _missionView = view ? view : throw new NullReferenceException(nameof(MissionView));
            
            _missionInfo = info ?? throw new NullReferenceException(nameof(MissionData));
            _missionId = missionId;
        }
        
        public void Initialize()
        {
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
            OnMissionSelect?.Invoke(_missionId);
        }
    }
}