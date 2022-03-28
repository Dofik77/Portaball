using System;
using Game.SceneLoading;
using Runtime.Services.CommonPlayerData;
using Runtime.Services.CommonPlayerData.Data;
using SimpleUi.Abstracts;
using UniRx;
using Zenject;

namespace Game.Ui.SplashScreen.Impls
{
    public class SplashScreenViewController : UiController<SplashScreenView>, IInitializable
    {
        [Inject] private ICommonPlayerDataService<CommonPlayerData> _commonPlayerData;
        
        private readonly ISceneLoadingManager _sceneLoadingManager;
        private IDisposable _disposable = Disposable.Empty;

        public SplashScreenViewController(ISceneLoadingManager sceneLoadingManager)
        {
            _sceneLoadingManager = sceneLoadingManager;
        }
        
        public void Initialize()
        {
            // _sceneLoadingManager.LoadScene(EScene.Game0_1);
            _sceneLoadingManager.LoadScene(_commonPlayerData.GetData().Level);
            // _sceneLoadingManager.LoadScene(EScene.Game0__3);
            // _commonPlayerData.GetData().Level
        }
    }
}