using DataBase.Game;
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
using Object = System.Object;

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
            //UIIdComp for all!!!
        }

        private void CreatePotal()
        {
            var portalOnScene = UnityEngine.Object.FindObjectsOfType<PortalView>(); 
            foreach (var view in portalOnScene)
            {
                var entity = _world.NewEntity();
                entity.Get<PortalComponent>().color = view.color;//из вьюшки
                entity.Get<EventAddComponent<PortalComponent>>();
                entity.Get<UIdComponent>().Value = UidGenerator.Next();
                entity.Get<LinkComponent>().View = view;
                view.Link(entity);
                
                //find all set by gamedis portal
            }
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
            
            //if have PrefabComponent, we havto Get EventAddComponent<PrefabComp>
            //maybe relacate in Extension and just find camera on scee and get some
            //component
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
            entity.Get<SphereCharacterComponent>();
            entity.Get<PrefabComponent>().Value = "Sphere";
            entity.Get<EventAddComponent<PrefabComponent>>();
            entity.Get<EventAddComponent<SphereCharacterComponent>>();
        }
    }
}