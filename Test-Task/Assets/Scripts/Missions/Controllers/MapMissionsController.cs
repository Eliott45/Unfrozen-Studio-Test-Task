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
    public class MapMissionsController : IDisposable
    {
        private readonly MissionsConfig _missionsConfig;
        private readonly MissionView _missionViewPrefab;
        private readonly IPoolApplication _poolApplication;
        private readonly Transform _mapTransform;
        
        private readonly Dictionary<string, MissionViewController[]> _missions = new();
        
        public MapMissionsController(MissionsConfig missionsConfig, MissionView missionViewPrefab, IPoolApplication poolApplication,
            Transform mapTransform)
        {
            _missionsConfig = missionsConfig ? missionsConfig : throw new NullReferenceException(nameof(MissionsConfig));
            _missionViewPrefab = missionViewPrefab ? missionViewPrefab : throw new NullReferenceException(nameof(MissionView));
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
                
                _missions.Add(missionData.Id, missionViewControllers);
            }
        }
        
        public void Dispose()
        {
            foreach (var mission in _missions)
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

        private void OnMissionSelect(string obj)
        {
            // todo add logic 
        }
    }
}
