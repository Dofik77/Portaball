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
            // var data = _commonPlayerData.GetData();
            // View.Show(data.Level);
            // if (data.Level >= Enum.GetValues(typeof(EScene)).Cast<EScene>().Last())
            // {
            //     data.Level = loopedLevel;
            //     _analyticsService.SendRequest("last_level_complete");
            // }
            // else
            //     data.Level++;
            // var rand = Random.Range(300, 600);
            // data.Money += rand;
            // _commonPlayerData.Save(data);
            // View.Currency.text = (data.Money - rand).ToString();
            // View.RewardValue.text = new StringBuilder("+").Append(rand).ToString();
            // View.transform.DOMove(Vector3.zero, 0.7f).SetRelative(true).OnComplete(() => View.GetReward(data.Money));
            
            // cameraView.Transform.DOMoveY(0, 1.5f).SetEase(Ease.Linear).SetRelative(true).OnComplete(() =>
            // {
            //      _sceneLoadingManager.LoadScene(_commonPlayerData.GetData().Level);
            // });
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
       