using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.Impls;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems
{
    public class RotateSystem : IEcsUpdateSystem
    {
        private readonly EcsFilter<SetRotationComponent, LinkComponent> _portalRotation;

        private Camera _camera;
        
        public void Run()
        {
            foreach (var position in _portalRotation)
            {
                var portalView = (PortalView) _portalRotation.Get2(position).View;
                portalView.transform.Rotate(portalView.transform.forward);

                _portalRotation.GetEntity(position).Del<SetRotationComponent>();
            }
        }
    }
}