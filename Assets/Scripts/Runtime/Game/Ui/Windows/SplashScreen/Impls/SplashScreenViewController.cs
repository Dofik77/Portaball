using System;
using Game.SceneLoading;
using SimpleUi.Abstracts;
using UniRx;
using Zenject;

namespace Game.Ui.SplashScreen.Impls
{
    public class SplashScreenViewController : UiController<SplashScreenView>, IInitializable
    {
        private readonly ISceneLoadingManager _sceneLoadingManager;
        private IDisposable _disposable = Disposable.Empty;

        public SplashScreenViewController(ISceneLoadingManager sceneLoadingManager)
        {
            _sceneLoadingManager = sceneLoadingManager;
        }
        
        public void Initialize()
        {
            _sceneLoadingManager.LoadScene(EScene.Game0_1);
        }
    }
}