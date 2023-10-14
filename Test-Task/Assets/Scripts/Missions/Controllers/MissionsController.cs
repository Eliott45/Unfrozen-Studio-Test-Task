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
            _preMissionViewController.ShowView(_missionData[missionId]);
        }

        private void OnPressStartMission(string optionId, MissionData mission)
        {
            _preMissionViewController.HideView();

            foreach (var missionOption in _missionData[mission.Id].MissionOptions)
            {
                if (missionOption.Id == optionId)
                {
                    _missionProgressController.StartMission(mission.Id, missionOption);
                    break;
                }
            }
        }

        private void OnMissionComplete(string id, MissionInfo info)
        {
            _missionData[id].State = MissionState.Completed;
            info.Completed = true;

            foreach (var missionViewController in _missionPoints[id])
            {
                missionViewController.ApplyState(_missionData[id].State);
            }
        }
    }
}