using System;
using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.Impls;
using Game.Utils.MonoBehUtils;
using Leopotam.Ecs;
using ModestTree;
using PdUtils;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

namespace ECS.Game.Systems
{
    public class PortalSystem : ReactiveSystem<EventAddComponent<PortalComponent>>
    {
        [Inject] private readonly GetPointFromScene _getPointFromScene;
        protected override EcsFilter <EventAddComponent<PortalComponent>> ReactiveFilter { get; }
        private EcsFilter<PortalComponent> _portals;
        private EcsFilter<SpherePlayerComponent, LinkComponent> _sphere;
        private EcsFilter<ActivePortalComponent> _activeComponent;

        private PortalView _portalView;
        
        protected override void Execute(EcsEntity entity)
        {
            _portalView = entity.Get<LinkComponent>().View as PortalView;
            _portalView.OnSphereTrigger += FindAnotherPortal;
        }
        
        
        void FindAnotherPortal(Uid id, PortalComponent.PortalColor enumColor, Transform enterPortalTransform)
        {
            foreach (var i in _portals)
            {
                var exitPortalEntity = _portals.GetEntity(i);
                var exitPortalColor = exitPortalEntity.Get<PortalComponent>().color; 

                if (enumColor == exitPortalColor 
                    && id != exitPortalEntity.Get<UIdComponent>().Value)
                {
                    TeleportSphere(exitPortalEntity, enterPortalTransform);
                } 
            }
        }

        private void TeleportSphere(EcsEntity exitPortalEntity, Transform enterPortalTransform)
        {
            foreach (var i in _sphere)
            {
                var sphereView = _sphere.Get2(i).View as SpherePlayerView;
                var exitPortalView = (PortalView) exitPortalEntity.Get<LinkComponent>().View;

                if (sphereView != null)
                {
                    var sphereVelocity = sphereView.rigidbody.velocity;

                    var enterPortalAngle = -enterPortalTransform.localEulerAngles +
                                           exitPortalView.transform.localEulerAngles + new Vector3(0,0,180);
                    //разворот относительно нового портала ( инверсия поворота ) 
                    
                    // x1=x*cos(angle) - y*sin(angle); 
                    // y1=y*cos(angle) + x*sin(angle);
                    // разворот вектора - жесть 
                    
                    var newSphereVelocity = new Vector3();
                    
                    newSphereVelocity.x = sphereVelocity.x * Mathf.Cos(enterPortalAngle.z * Mathf.Deg2Rad) -
                                          sphereVelocity.y * Mathf.Sin(enterPortalAngle.z * Mathf.Deg2Rad);
                    
                    newSphereVelocity.y = sphereVelocity.y * Mathf.Cos(enterPortalAngle.z * Mathf.Deg2Rad) +
                                          sphereVelocity.x * Mathf.Sin(enterPortalAngle.z * Mathf.Deg2Rad);

                    var sphereTransform = sphereView.rigidbody;
                    var exitPortalTransform = exitPortalView.transform;
                    var exitPortalPoint = exitPortalView._pointToLocate.transform;
                    
                    sphereTransform.position =
                        exitPortalPoint.position;

                     sphereTransform.rotation = 
                         exitPortalTransform.rotation;
                     
                     sphereView.rigidbody.velocity =
                         newSphereVelocity;
                }
            }
        }
    }
}