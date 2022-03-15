﻿using DataBase.Game;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.DataSave;
using ECS.Game.Components;
using ECS.Game.Components.Events;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Utils.Extensions;
using ECS.Utils.Impls;
using ECS.Views.Impls;
using Game.Utils.MonoBehUtils;
using Leopotam.Ecs;
using Runtime.DataBase.General.CommonParamsBase;
using Runtime.DataBase.General.GameCFG;
using Runtime.Services.CommonPlayerData;
using Runtime.Services.CommonPlayerData.Data;
using Runtime.Services.GameStateService;
using Services.Uid;
using Signals;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems
{
    public class GameInitializeSystem : IEcsInitSystem
    {
        [Inject] private readonly IGameStateService<GameState> _generalState;
        [Inject] private readonly GetPointFromScene _getPointFromScene;
        [Inject] private readonly ICommonPlayerDataService<CommonPlayerData> _playerData;
        
        private readonly EcsWorld _world;
        public void Init()
        {
            if (LoadGame()) return;
            CreateLevel();
            CreatePlayer();
            CreateCamera();
            CreatePotal();
            CreatePipes();
            CreateTimer();
        }

        private void CreatePotal()
        {
            var entity = _world.NewEntity();
            entity.Get<PortalComponent>();
            entity.Get<PrefabComponent>().Value = "Portal";
            entity.Get<EventAddComponent<PrefabComponent>>();
            entity.Get<EventAddComponent<PortalComponent>>();
            //for another portal create extensonSystemfor ingame entity
        }

        private void CreatePipes()
        {
            var entity = _world.NewEntity();
            entity.Get<PipeComponent>();
            entity.Get<EventAddComponent<PipeComponent>>();
        }

        private void CreateCamera()
        {
            var entity = _world.NewEntity();
            entity.Get<CameraComponent>();
            entity.Get<PrefabComponent>().Value = "MainCamera";
            entity.Get<EventAddComponent<PrefabComponent>>();
            entity.Get<EventAddComponent<CameraComponent>>();
        }

        private bool LoadGame()
        {
            _world.NewEntity().Get<GameStageComponent>().Value = EGameStage.Pause;
            var gState = _generalState.GetData();
            if (gState.SaveState.IsNullOrEmpty()) return false;
            foreach (var state in gState.SaveState)
            {
                var entity =_world.NewEntity();
                state.ReadState(entity);
            }
            return true;
        }

        private void CreateLevel()
        {
            var leveRoot = GameObject.Find("[LOCATE]");
            var playerData = _playerData.GetData();
            if (playerData.Level > 1)
                playerData.Level = 2;
            leveRoot.transform.GetChild(0).gameObject.SetActive(true);
            playerData.Level++;
            _playerData.Save(playerData);
        }
        

        private void CreateTimer()
        {
            var entity = _world.NewEntity();
            entity.Get<TimerComponent>();
            entity.Get<UIdComponent>().Value = UidGenerator.Next();
        }

        private void CreatePlayer()
        {
            var entity = _world.NewEntity();
            var point = _getPointFromScene.GetPoint("Player");
            entity.Get<PlayerComponent>();
            entity.Get<PrefabComponent>().Value = "Sphere";
            entity.Get<EventAddComponent<PrefabComponent>>();
            entity.Get<EventAddComponent<PlayerComponent>>();
        }
    }
}