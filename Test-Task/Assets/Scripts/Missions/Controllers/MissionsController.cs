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
            _missionsConfig = missionsConfig ? missionsConfig : throw new NullReferenceException(nameof(MissionsConfig));
            _missionViewPrefab = missionViewPrefab ? missionViewPrefab : throw new NullReferenceException(nameof(MissionView));
            _preMissionViewController = preMissionViewController ?? throw new NullReferenceException(nameof(PreMissionViewController));
            _missionProgressController = missionProgressController ?? throw new NullReferenceException(nameof(MissionProgressController));
            _poolApplication = poolApplication ?? throw new NullReferenceException(nameof(IPoolApplication));
            _mapTransform = mapTransform ? mapTransform : throw new NullReferenceException(nameof(Transform));

            InitializeMissions();
        }

        private void InitializeMissions()
        {
            _missionProgressController.OnMissionComplete += OnMissionComplete;
            _preMissionViewController.OnStartMission += OnPressStartMission;
            
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
            _preMissionViewController.OnStartMission -= OnPressStartMission;
            
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
            var controllers = data.Type switch
            {
                MissionType.Single => new[]
                {
                    InitMissionViewController(_poolApplication.Create(_missionViewPrefab, _mapTransform),
                        data.PrimaryMissionDetails, data.Id)
                },
                MissionType.Double => new[]
                {
                    InitMissionViewController(_poolApplication.Create(_missionViewPrefab, _mapTransform),
                        data.PrimaryMissionDetails, data.Id),
                    InitMissionViewController(_poolApplication.Create(_missionViewPrefab, _mapTransform),
                        data.SecondaryMissionDetails, data.Id)
                },
                _ => throw new ArgumentOutOfRangeException()
            };

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
            _preMissionViewController.ShowView(_missionData[missionId]);
        }

        private void OnPressStartMission(string id, MissionInfo mission)
        {
            _preMissionViewController.HideView();
            _missionProgressController.StartMission(id, mission);
        }
        
        private void OnMissionComplete(string id, MissionInfo info)
        {
            _missionData[id].State = MissionState.Completed;
            info.Completed = true;

            foreach (var missionViewController in _missionPoints[id])
            {
                missionViewController.ApplyState(_missionData[id].State);
            }

            CheckAndUnlockMissions();
        }
        
        private void CheckAndUnlockMissions()
        {
            foreach (var missionData in _missionData.Values)
            {
                if (missionData.State != MissionState.Locked)
                {
                    continue;
                }
                
                var allPreviousMissionsCompleted = CheckRequiredMissions(missionData);

                if (!allPreviousMissionsCompleted)
                {
                    continue;
                }
                
                missionData.State = MissionState.Active;

                if (!_missionPoints.TryGetValue(missionData.Id, out var missionViewControllers))
                {
                    continue;
                }
                
                foreach (var missionViewController in missionViewControllers)
                {
                    missionViewController.ApplyState(missionData.State);
                }
            }
        }

        private bool CheckRequiredMissions(MissionData mission)
        {
            foreach (var requiredMissionId in mission.RequiredPreviousMissions)
            {
                if (!IsMissionCompleted(requiredMissionId))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsMissionCompleted(string missionId)
        {
            if (_missionData.TryGetValue(missionId, out var missionData))
            {
                return missionData.State == MissionState.Completed ||
                       IsPrimaryMissionCompleted(missionData.PrimaryMissionDetails, missionId) ||
                       IsSecondaryMissionCompleted(missionData.SecondaryMissionDetails, missionId);
            }
            return false;
        }

        private bool IsPrimaryMissionCompleted(MissionInfo missionInfo, string missionId)
        {
            return missionInfo.OptionId == missionId && missionInfo.Completed;
        }

        private bool IsSecondaryMissionCompleted(MissionInfo missionInfo, string missionId)
        {
            return missionInfo.OptionId == missionId && missionInfo.Completed;
        }
    }
}
