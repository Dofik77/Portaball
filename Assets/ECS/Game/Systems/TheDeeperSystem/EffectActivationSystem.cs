using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.TheDeeperComponent;
using Leopotam.Ecs;

namespace ECS.Game.Systems
{
    public class EffectActivationSystem : IEcsUpdateSystem
    {
        private readonly EcsFilter<PortalComponent, LinkComponent, EffectActivationComponent> _effectsFilter;
        private EcsFilter<PortalComponent> _portals;
        public void Run()
        {
            foreach (var effects in _effectsFilter)
            {
                foreach (var portal in _portals)
                {
                    
                }
            }
        }
    }
}