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
        private readonly IPoolApplication _poolApplication;
        private readonly Transform _mapTransform;

        private readonly Dictionary<string, MissionViewController[]> _missionPoints = new();
        private readonly Dictionary<string, MissionData> _missionData = new();
        
        public MissionsController(MissionsConfig missionsConfig, 
            MissionView missionViewPrefab,
            PreMissionViewController preMissionViewController,
            IPoolApplication poolApplication,
            Transform mapTransform)
        {
            _missionsConfig = missionsConfig ? missionsConfig : throw new NullReferenceException(nameof(MissionsConfig));
            _missionViewPrefab = missionViewPrefab ? missionViewPrefab : throw new NullReferenceException(nameof(MissionView));
            _preMissionViewController = preMissionViewController ?? throw new NullReferenceException(nameof(PreMissionViewController));
            _poolApplication = poolApplication ?? throw new NullReferenceException(nameof(IPoolApplication));
            _mapTransform = mapTransform ? mapTransform : throw new NullReferenceException(nameof(Transform));

            InitializeMissions();
        }

        private void InitializeMissions()
        {
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
            var missionViewController = new MissionViewController(view, info, id);

            missionViewController.OnMissionSelect += OnMissionSelect;

            missionViewController.Initialize();

            return missionViewController;
        }

        private void OnMissionSelect(string missionId)
        {
            _preMissionViewController.ShowView(_missionData[missionId]);
        }
    }
}
