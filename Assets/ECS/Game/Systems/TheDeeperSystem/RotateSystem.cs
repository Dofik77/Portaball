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
            foreach (var rotation in _portalRotation)
            {
                var portalView = (PortalView) _portalRotation.Get2(rotation).View;
                var angle = _portalRotation.Get1(rotation).deltaAngle;
                // portalView.transform.eulerAngles = new Vector3(0, 0,eugle);
                
                // portalView.transform.Rotate(portalView.transform.forward,
                //     Vector2.Dot(angle, Vector2.right) * -1, Space.World);

                var angelToRotate = new Vector3(0, 0, angle);
                portalView.transform.localEulerAngles += angelToRotate.normalized;
                
                Debug.Log(angle);

                //костыль - скрипт PortalSystem

                _portalRotation.GetEntity(rotation).Del<SetRotationComponent>();
            }
        }
    }
}