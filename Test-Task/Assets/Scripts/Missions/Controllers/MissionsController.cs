using System;
using System.Collections.Generic;
using Missions.Configs;
using Missions.Data;
using Pool.Application;
using UI.Controllers;
using UI.Views;
using UnityEngine;

namespace Missions.Controllers
{
    public class MissionsController : IDisposable
    {
        private readonly MissionsConfig _missionsConfig;
        private readonly MissionView _missionViewPrefab;
        private readonly PreMissionViewController _preMissionViewController;
        private readonly MissionProgressController _missionProgressController;
        private readonly IPoolApplication _poolApplication;
        private readonly Transform _mapTransform;

        private readonly Dictionary<string, MissionViewController[]> _missionPoints = new();
        private readonly Dictionary<string, MissionData> _missionData = new();

        public MissionsController(MissionsConfig missionsConfig,
            MissionView missionViewPrefab,
            PreMissionViewController preMissionViewController,
            MissionProgressController missionProgressController,
            IPoolApplication poolApplication,
            Transform mapTransform)
        {
            _missionsConfig =
                missionsConfig ? missionsConfig : throw new NullReferenceException(nameof(MissionsConfig));
            _missionViewPrefab = missionViewPrefab
                ? missionViewPrefab
                : throw new NullReferenceException(nameof(MissionView));
            _preMissionViewController = preMissionViewController ??
                                        throw new NullReferenceException(nameof(PreMissionViewController));
            _missionProgressController = missionProgressController ??
                                         throw new NullReferenceException(nameof(MissionProgressController));
            _poolApplication = poolApplication ?? throw new NullReferenceException(nameof(IPoolApplication));
            _mapTransform = mapTransform ? mapTransform : throw new NullReferenceException(nameof(Transform));

            InitializeMissions();
        }

        private void InitializeMissions()
        {
            _missionProgressController.OnMissionComplete += OnMissionComplete;
            _preMissionViewController.OnPressStartMission += OnPressStartMission;

            foreach (var missionData in _missionsConfig.GetMissionsCopy())
            {
                var missionViewControllers = InitMissionViewControllers(missionData);

                foreach (var controller in missionViewControllers)
                {
                    controller.ApplyState(missionData.State);
                }

                _missionPoints.Add(missionData.Id, missionViewControllers);
                _missionData.Add(missionData.Id, missionData);
            }
        }

        public void Dispose()
        {
            _missionProgressController.OnMissionComplete -= OnMissionComplete;
            _preMissionViewController.OnPressStartMission -= OnPressStartMission;

            foreach (var mission in _missionPoints)
            {
                foreach (var missionViewController in mission.Value)
                {
                    missionViewController.Dispose();
                }
            }
        }

        private MissionViewController[] InitMissionViewControllers(MissionData data)
        {
            MissionViewController[] controllers;
            switch (data.Type)
            {
                case MissionType.SingleOption:
                    controllers = new[]
                    {
                        InitMissionViewController(_poolApplication.Create(_missionViewPrefab, _mapTransform),
                            data.MissionOptions[0], data.Id)
                    };
                    break;
                case MissionType.MultipleOptions:
                    controllers = new MissionViewController[data.MissionOptions.Length];
                    for (var index = 0; index < data.MissionOptions.Length; index++)
                    {
                        var option = data.MissionOptions[index];
                        var missionView = _poolApplication.Create(_missionViewPrefab, _mapTransform);
                        
                        controllers[index] = InitMissionViewController(missionView, option, data.Id);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return controllers;
        }

        private MissionViewController InitMissionViewController(MissionView view, MissionInfo info, string id)
        {
            var missionViewController = new MissionViewController(view);

            missionViewController.OnPressSelectMission += OnPressSelectMission;

            missionViewController.LoadData(info, id);

            return missionViewController;
        }

        private void OnPressSelectMission(string missionId)
        {
            if(_missionData.TryGetValue(missionId, out var mission))
            {
                _preMissionViewController.ShowView(mission);
            }
            else
            {
                Debug.LogError($"Mission with id: {missionId} doesn't find!");
            }
        }

        private void OnPressStartMission(string optionId, MissionData mission)
        {
            _preMissionViewController.HideView();
            
            if(_missionData.TryGetValue(mission.Id, out var missionData))
            {
                foreach (var missionOption in missionData.MissionOptions)
                {
                    if (missionOption.Id == optionId)
                    {
                        _missionProgressController.StartMission(mission.Id, missionOption);
                        break;
                    }
                }
            }
            else
            {
                Debug.LogError($"Mission with id: {mission.Id} doesn't find!");
                return;
            }
        }

        private void OnMissionComplete(string id, MissionInfo info)
        {
            if(_missionData.TryGetValue(id, out var mission))
            {
                mission.State = MissionState.Completed;
                info.Completed = true;

                ApplyPointsState(mission.Id, mission.State);

                CheckAndUnlockMissions(id, info.Id);
            }
            else
            {
                Debug.LogError($"Mission with id: {id} doesn't find!");
            }
        }

        private void CheckAndUnlockMissions(string id, string optionId)
        {
            foreach (var mission in _missionData)
            {
                if (mission.Value.State != MissionState.Locked)
                {
                    continue;
                }
                
                if (mission.Value.RequiredPreviousMissions != null && mission.Value.RequiredPreviousMissions.Contains(id))
                {
                    mission.Value.State = MissionState.Active;
                    ApplyPointsState(mission.Key, mission.Value.State);
                    continue;
                }

                if (mission.Value.RequiredPreviousOptions != null && mission.Value.RequiredPreviousOptions.Contains(optionId))
                {
                    mission.Value.State = MissionState.Active;
                    ApplyPointsState(mission.Key, mission.Value.State);
                }
            }
        }

        private void ApplyPointsState(string missionId, MissionState state)
        {
            foreach (var missionViewController in _missionPoints[missionId])
            {
                missionViewController.ApplyState(state);
            }
        }
    }
}