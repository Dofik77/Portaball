using System.Diagnostics.CodeAnalysis;
using DataBase.Game;
using DG.Tweening;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Game.Components;
using ECS.Game.Components.Events;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.Impls;
using Game.SceneLoading;
using Leopotam.Ecs;
using Zenject;
#pragma warning disable 649
namespace ECS.Game.Systems.GameCycle
{
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class LevelEndSystem : ReactiveSystem<ChangeStageComponent>
    {
        // [Inject] private readonly ScreenVariables _screenVariables;
        [Inject] private readonly SignalBus _signalBus;

        [Inject] private ISceneLoadingManager _sceneLoadingManager;
        // [Inject] private readonly IVibrationService _vibrationService;
        private readonly EcsWorld _world;
        private readonly EcsFilter<CameraComponent, LinkComponent> _cameraF;
        // private readonly EcsFilter<BoxComponent, LinkComponent> _box;
        // private readonly EcsFilter<PutableComponent, LinkComponent> _putables;
        protected override EcsFilter<ChangeStageComponent> ReactiveFilter { get; }
        protected override bool DeleteEvent => false;
        private bool disable;

        protected override void Execute(EcsEntity entity)
        {
            if (disable)
                return;
            switch (entity.Get<ChangeStageComponent>().Value)
            {
                case EGameStage.Lose:
                    HandleLevelLose();
                    disable = true;
                    break;
                case EGameStage.Complete:
                    HandleLevelComplete();
                    disable = true;
                    break;
            }
        }

        [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
        private void HandleLevelComplete()
        {
            // foreach (var i in _box)
            // {
            //     var view = _box.Get2(i).Get<BoxView>();
            //     var filler = _box.Get1(i).Filler == EBoxFiller.Filler_1 ? view.GetFiller1() : view.GetFiller2();
            //     var fillerParticle = _box.Get1(i).Filler == EBoxFiller.Filler_1
            //         ? view.GetFiller1Particle()
            //         : view.GetFiller2Particle();
            //     fillerParticle.gameObject.SetActive(true);
            //     _vibrationService.Vibrate(420);
            //     filler.DOMoveY(view.GetFillerTopHeight().position.y, 1.8f).SetEase(Ease.Linear).SetDelay(0.3f)
            //         .OnComplete(() => moveBox());
            //
            //     void moveBox()
            //     {
            //         view.GetFront().GetComponent<MeshRenderer>().material =
            //             new Material(view.GetLeft().GetComponent<MeshRenderer>().material);
            //         var pos = _screenVariables.GetTransformPoint("BoxFinal").position;
            //         _world.CreateParticle("ConfettiBlast", view.GetFillerTopHeight().position + Vector3.up, 1.5f);
            //         view.GetLeft().DORotateQuaternion(view.GetLeftFinish().rotation, 0.3f).SetEase(Ease.Linear);
            //         view.GetRight().DORotateQuaternion(view.GetRightFinish().rotation, 0.3f).SetEase(Ease.Linear)
            //             .OnComplete(() =>
            //             {
            //                 foreach (var j in _putables)
            //                     _putables.Get2(j).View.Transform.SetParent(view.Transform);
            //             });
            //         view.Transform.DOMove(pos, 0.3f)
            //             .SetEase(Ease.Linear).SetDelay(0.3f)
            //             .OnComplete(() =>
            //             {
            //                 _world.CreateParticle("ConfettiShower", pos + Vector3.up * 10 + Vector3.back * 4, 15f);
            //                 _signalBus.OpenWindow<LevelCompleteWindow>();
            //             });
            //     }
            // }
        }

        private void HandleLevelLose()
        {
            foreach (var camera in _cameraF)
            {
                var cameraView = (CameraView) _cameraF.Get2(camera).View;
                //TODO goodcase
                cameraView.Transform.DOMoveY(0, 1.5f).SetEase(Ease.Linear).SetRelative(true).OnComplete(() =>
                {
                    _sceneLoadingManager.ReloadScene();
                });
            }
            
        }
    }
}
       