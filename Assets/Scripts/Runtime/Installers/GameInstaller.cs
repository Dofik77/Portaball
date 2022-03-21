using ECS.Utils.Impls;
using Runtime.Game.Ui;
using Runtime.Initializers;
using Runtime.UI.QuitConcentPopUp;
using Services.PauseService.Impls;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindWindows();
            BindServices();
            Container.BindInterfacesAndSelfTo<GameInitializer>().AsSingle();
        }

        private void BindWindows()
        {
             Container.BindInterfacesAndSelfTo<ConsentWindow>().AsSingle();
             Container.BindInterfacesAndSelfTo<GameHudWindow>().AsSingle();
        }

        private void BindServices()
        {
            Container.BindInterfacesTo<SpawnService>().AsSingle();
            Container.BindInterfacesTo<PauseService>().AsSingle();
        }
    }
}