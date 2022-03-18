using ECS.Game.Systems;
using ECS.Game.Systems.GameCycle;
using ECS.Game.Systems.GameDay;
using ECS.Game.Systems.Linked;
using ECS.Game.Systems.Move;
using Game.Utils.MonoBehUtils;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace ECS.Installers
{
    public class EcsInstaller : MonoInstaller
    {
        [SerializeField] private GetPointFromScene _getPointFromScene;
        [SerializeField] private ScenePath _pathRoot;
        public override void InstallBindings()
        {
            Container.Bind<GetPointFromScene>().FromInstance(_getPointFromScene).AsSingle();
            Container.Bind<ScenePath>().FromInstance(_pathRoot).AsSingle();
            Container.BindInterfacesAndSelfTo<EcsWorld>().AsSingle().NonLazy();
            BindSystems();
            Container.BindInterfacesTo<EcsMainBootstrap>().AsSingle();
        }

        private void BindSystems()
        {
            //tech system
            Container.BindInterfacesAndSelfTo<IsAvailableSetViewSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameInitializeSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<InstantiateSystem>().AsSingle();
            //Container.BindInterfacesAndSelfTo<PositionRotationTranslateSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameTimerSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PositionSystem>().AsSingle();
            //game process system
            Container.BindInterfacesAndSelfTo<CameraLocateSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PipeSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PortalSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<LocatePortalSystem>().AsSingle();
            // Container.BindInterfacesAndSelfTo<PlayerRootMoveSystem>().AsSingle();
            // Container.BindInterfacesAndSelfTo<MoveRotateToTargetSystem>().AsSingle();
            // Container.BindInterfacesAndSelfTo<RemapOnAddSystem>().AsSingle();
            // Container.BindInterfacesAndSelfTo<PlayerModelMoveSideSystem>().AsSingle();
            // Container.BindInterfacesAndSelfTo<AddImpactToPlayerSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputSystem>().AsSingle();
            
            //tech system
            Container.BindInterfacesAndSelfTo<GamePauseSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveGameSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<StartGameSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<EndGameSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStageSystem>().AsSingle();        //always must been last
            Container.BindInterfacesAndSelfTo<CleanUpSystem>().AsSingle();          //must been latest than last!
        }       
    }
}