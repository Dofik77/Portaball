using System.Collections.Generic;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Game.Systems.GameCycle;
using ECS.Utils.Extensions;
using ECS.Views.Impls;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems
{
    public class LocatePortalSystem : IEcsUpdateSystem
    {
        private readonly EcsFilter<EventInputHoldAndDragComponent> _eventInputHoldAndDragComponent; // Press
        private readonly EcsFilter<EventInputDownComponent> _eventInputDownComponent; // Down
        private readonly EcsFilter<EventInputUpComponent> _eventInputUpComponent; // Up
        
        private readonly EcsFilter<CameraComponent, LinkComponent, ActiveCameraComponent> _filterCamera;
        
        private readonly EcsFilter<PortalComponent, LinkComponent, ActivePortalComponent> _activePortal;
        private readonly EcsFilter<InActionPortalComponent, LinkComponent> _inActionPortal;

        private readonly EcsWorld _world;
        
        private EcsEntity newPortal;
        private PortalComponent.PortalColor _wallColor;

        private LayerMask _defaultLayerMask = LayerMask.GetMask("Default");
        private LayerMask _portalLayerMask = LayerMask.GetMask("Portal");

    public void Run()
    {
        foreach (var downInput in _eventInputDownComponent) 
        {
            var inputPos = _eventInputDownComponent.Get1(downInput).Down;

            if (TryGetTouchPointInWorldSpace(out Vector3 locatePoint, _defaultLayerMask, inputPos))
            {
                newPortal = CreateActualPortal(_wallColor);
                
                foreach (var activePortal in _activePortal)
                {
                    if (_activePortal.Get1(activePortal).color == newPortal.Get<PortalComponent>().color)
                        _activePortal.GetEntity(activePortal).Get<IsDestroyedComponent>();
                }
                
                var newPosition = new Vector3(locatePoint.x, locatePoint.y, locatePoint.z + 0.5f);
                
                newPortal.Get<ActivePortalComponent>();
                newPortal.Get<InActionPortalComponent>();
                newPortal.Get<SetPositionComponent>().position = newPosition;
            }
            _eventInputDownComponent.GetEntity(downInput).Del<EventInputDownComponent>();
        }
        
        // DragPortal();
    }
        
        private void DragPortal()
        {
            foreach (var actionPortal in _inActionPortal)
            {
                foreach (var holdAndDrag in _eventInputHoldAndDragComponent)
                {
                    var inputPos = _eventInputHoldAndDragComponent.Get1(holdAndDrag).Drag;
                    
                    if (TryGetTouchPointInWorldSpace(out Vector3 locatePoint, _portalLayerMask, inputPos))
                    {
                        var newRotation = Quaternion.Euler(locatePoint);
                        _inActionPortal.GetEntity(actionPortal).Get<SetRotationComponent>().Eugle = newRotation;
                    }
                    _eventInputHoldAndDragComponent.GetEntity(holdAndDrag).Del<EventInputHoldAndDragComponent>();
                    
                    foreach (var inputUp in _eventInputUpComponent)
                    {
                        _inActionPortal.GetEntity(actionPortal).Del<InActionPortalComponent>();
                        _eventInputUpComponent.GetEntity(inputUp).Del<EventInputUpComponent>();
                    }
                }
            }
        }
        
        private bool TryGetTouchPointInWorldSpace(out Vector3 locatePoint, LayerMask layerMask, Vector2 inputPos)
        {
            var actualCamera = GetCameraFromFilter();
            var ray = actualCamera.ScreenPointToRay(inputPos);
            var hasHit = Physics.Raycast(ray, out var raycastHit,100f,layerMask);
           
            _wallColor = GetColorFromWall(raycastHit); //вынести в отдельную переменную 
            locatePoint = raycastHit.point;
            
            return hasHit;
        }
        
        private PortalComponent.PortalColor GetColorFromWall(RaycastHit raycastHit)
        {
            if (raycastHit.transform.gameObject.TryGetComponent(out WallView wallView))
            {
                _wallColor = wallView.color;
            }
            else
            {
                //uncolerd - if wall hane not opporunity set portal or check over border 
            }
            return _wallColor;
        }
        
        private EcsEntity CreateActualPortal(PortalComponent.PortalColor portalColor)
        {
            var portal = _world.CreatePortal(portalColor);
            portal.Get<InActionPortalComponent>();
            
            return portal;
        }

        public Camera GetCameraFromFilter()
        {
            Camera actualCamera = null;
            
            foreach (var camera in _filterCamera)
            {
                var cameraView = (CameraView) _filterCamera.Get2(camera).View;
                actualCamera = cameraView._camera;
            }

            return actualCamera != null ? actualCamera : null;
        }
    }
}