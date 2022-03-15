using System;
using DataBase.Game;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Utils.Extensions;
using Game.SceneLoading;
using Leopotam.Ecs;
using Runtime.Services.CommonPlayerData;
using Runtime.Services.CommonPlayerData.Data;
using Signals;
using SimpleUi.Abstracts;
using UniRx;
using UnityEngine;
using Utils.UiExtensions;
using Zenject;

namespace Game.Ui.InGameMenu
{
    public class InGameMenuViewController : UiController<InGameMenuView>, IInitializable
    {
        private readonly ISceneLoadingManager _sceneLoadingManager;
        private readonly EcsWorld _world;
        private readonly SignalBus _signalBus;
        private readonly ICommonPlayerDataService<CommonPlayerData> _commonPlayerData;

        public InGameMenuViewController(ISceneLoadingManager sceneLoadingManager,
            EcsWorld world, SignalBus signalBus, ICommonPlayerDataService<CommonPlayerData> commonPlayerData)
        {
            _sceneLoadingManager = sceneLoadingManager;
            _world = world;
            _signalBus = signalBus;
            _commonPlayerData = commonPlayerData;
        }
        
        public void Initialize()
        {
            View.Play.OnClickAsObservable().Subscribe(x => OnPlay()).AddTo(View.Play);
            View.Restart.OnClickAsObservable().Subscribe(x => OnRestart()).AddTo(View.Restart);
            View.FinishLevel.OnClickAsObservable().Subscribe(x => OnFinish()).AddTo(View.FinishLevel);
            _signalBus.GetStream<SignalUpdateImpact>().Subscribe(x => OnImpactUpdate(x.Value)).AddTo(View);
            // _signalBus.GetStream<SignalGameEnd>().Subscribe(x => InFinish(x.Result)).AddTo(View);
            View.coinCount.text = _commonPlayerData.GetData().coins.ToString();
        }

        private void OnPlay()
        {
            View.progress.SetFillAmount(_world.GetEntity<PlayerComponent>().Get<ImpactComponent>().Value.Remap01(1000));
            View.progress.gameObject.SetActive(true);
            View.Play.gameObject.SetActive(false);
            _world.SetStage(EGameStage.Play);
        }

        // private void InFinish(EGameResult result)
        // {
        //     View.progress.gameObject.SetActive(false);
        //     var impact = _world.GetEntity<PlayerComponent>().Get<ImpactComponent>().Value;
        //     switch (result)
        //     {
        //         case EGameResult.Fail:
        //             View.Restart.gameObject.SetActive(true);
        //             break;
        //         case EGameResult.Win:
        //             OnWin(impact, out var newCoinCount);
        //             View.finishLevel.FinishShow(
        //                     impact, 
        //                     impact.ComparePlayerStage() - 1,
        //                 () => {View.CoinAdded(Vector2.zero, newCoinCount); });
        //             break;
        //     }
        // }

        private void OnWin(int reward, out int newCoinCount)
        {
            var data = _commonPlayerData.GetData();
            newCoinCount = data.coins += reward;
            _commonPlayerData.Save(data);
        }
        
        private void OnRestart()
        {
            _sceneLoadingManager.ReloadScene();
        }

        private void OnImpactUpdate(int value) => View.progress.Repaint(value.Remap01(1000));

        private void OnFinish()
        {
            _sceneLoadingManager.LoadScene(EScene.Game);
        }
    }
}