using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.Impls;
using Leopotam.Ecs;

namespace ECS.Game.Systems
{
    public class EffectActivationSystem : IEcsUpdateSystem
    {
        private readonly EcsFilter<PortalComponent, LinkComponent, EffectActivationComponent, UIdComponent> _effectsFilter;
        private EcsFilter<PortalComponent, LinkComponent, UIdComponent> _portals;
        public void Run()
        {
            foreach (var effects in _effectsFilter)
            {
                var portalWithEffectsEntity = _effectsFilter.GetEntity(effects);
                var portalWithEffectsView = (PortalView) portalWithEffectsEntity.Get<LinkComponent>().View;
                var portalColorWithEffects = portalWithEffectsView.color;
                var id = portalWithEffectsEntity.Get<UIdComponent>().Value;

                foreach (var portal in _portals)
                {
                    var portalEntity = _portals.GetEntity(portal);
                    var portalView = (PortalView) portalEntity.Get<LinkComponent>().View;
                    var portalColor = portalView.color;

                    if (portalColorWithEffects == portalColor && id != portalEntity.Get<UIdComponent>().Value)
                    {
                        portalWithEffectsView._portalEffect.SetActive(true);
                        portalView._portalEffect.SetActive(true);
                    }
                }
            }
        }
    }
}