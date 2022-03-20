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
                var eugle = _portalRotation.Get1(position).Eugle;
                portalView.transform.Rotate(new Vector3(eugle.x,0,0).normalized);
                //костыль - скрипт PortalSystem

                _portalRotation.GetEntity(position).Del<SetRotationComponent>();
            }
        }
    }
}