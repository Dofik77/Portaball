using ECS.Game.Components;
using ECS.Game.Components.TheDeeperComponent;
using Leopotam.Ecs;

namespace ECS.Game.Systems
{
    public class RotateSystem
    {
        private readonly EcsFilter<SetRotationComponent, LinkComponent> _portalRotation;
        public void Run()
        {
            foreach (var position in _portalRotation)
            {
                _portalRotation.Get2(position).View.Transform.position =
                    _portalRotation.Get1(position).position;
                
                _portalRotation.GetEntity(position).Del<SetRotationComponent>();
            }
        }
    }
}