using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.Impls;
using Game.Utils.MonoBehUtils;
using Leopotam.Ecs;
using ModestTree;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems
{
    public class PortalSystem : ReactiveSystem<EventAddComponent<PortalComponent>>
    {
        protected override EcsFilter<EventAddComponent<PortalComponent>> ReactiveFilter { get; }
        //другой портал найдем по фильтру ActiveComponent[] 
        [Inject] private readonly GetPointFromScene _getPointFromScene;
        protected override void Execute(EcsEntity entity)
        {
            var firstPoint = _getPointFromScene.GetPoint("FirstPortal");
            entity.Get<LinkComponent>().View.Transform.position = firstPoint.position;

            PortalView portalView = entity.Get<LinkComponent>().View as PortalView;
            PortalComponent portalComponent = entity.Get<PortalComponent>(); //how transfer color
            portalView.OnSphereCollision += TeleportSphere;
            

            void TeleportSphere(SphereCharacterView characterView)
            {
                //находить портал с таким же цветом 
                var portal = _getPointFromScene.GetPoint("SecondPortal");
                var sphereVelocity = characterView.rigidbody.velocity;
                
                var transform = characterView.rigidbody.transform;
                transform.position =
                    portal.position;
                transform.rotation = 
                    portal.rotation;
                
                characterView.rigidbody.velocity = 
                    new Vector3(sphereVelocity.x, sphereVelocity.y, sphereVelocity.z);
            }
        }
    }
}