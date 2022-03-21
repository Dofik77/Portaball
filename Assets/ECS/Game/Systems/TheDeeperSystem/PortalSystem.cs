using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.Impls;
using Game.Utils.MonoBehUtils;
using Leopotam.Ecs;
using ModestTree;
using PdUtils;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems
{
    public class PortalSystem : ReactiveSystem<EventAddComponent<PortalComponent>>
    {
        [Inject] private readonly GetPointFromScene _getPointFromScene;
        protected override EcsFilter <EventAddComponent<PortalComponent>> ReactiveFilter { get; }
        private EcsFilter<PortalComponent> _portals;
        private EcsFilter<SphereCharacterComponent, LinkComponent> _sphere;
        private EcsFilter<ActivePortalComponent> _activeComponent;
        
        protected override void Execute(EcsEntity entity)
        {
            PortalView portalView = entity.Get<LinkComponent>().View as PortalView;
            portalView.OnSphereTrigger += FindAnotherPortal;
        }
        
        void FindAnotherPortal(Uid id, PortalComponent.PortalColor enumColor)
        {
            foreach (var i in _portals)
            {
                var exitPortalEntity = _portals.GetEntity(i);
                var exitPortalColor = exitPortalEntity.Get<PortalComponent>().color;

                if (enumColor == exitPortalColor 
                    && id != exitPortalEntity.Get<UIdComponent>().Value)
                {
                    TeleportSphere(exitPortalEntity);
                } 
            }
        }

        private void TeleportSphere(EcsEntity exitPortalEntity)
        {
            foreach (var i in _sphere)
            {
                var sphereView = _sphere.Get2(i).View as SphereCharacterView;
                var exitPortalView = (PortalView) exitPortalEntity.Get<LinkComponent>().View;

                if (sphereView != null)
                {
                    var sphereVelocity = sphereView.rigidbody.velocity;

                    var sphereTransform = sphereView.rigidbody.transform;
                    var exitPortalTransform = exitPortalView.Transform;
                    var exitPortalPoint = exitPortalView._pointToLocate.transform;

                    sphereTransform.position =
                        exitPortalPoint.position;

                     sphereTransform.rotation = 
                         exitPortalTransform.rotation;
                    // sphere.engel - portal.system + portal2.system
                    // do letter

                     sphereView.rigidbody.velocity =
                         sphereVelocity;
                }
            }
        }
    }
}