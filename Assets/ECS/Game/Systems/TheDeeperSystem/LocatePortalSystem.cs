﻿using System.Collections.Generic;
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
        
        private EcsEntity _newPortal;
        private PortalComponent.PortalColor _wallColor;
        
        private Vector3 _deltaPos = Vector3.zero;   
        private Vector3 _prevPos = Vector3.zero;
        private Vector3 _downPos = Vector3.zero;

        private readonly LayerMask _defaultLayerMask = LayerMask.GetMask("Default");
        private readonly LayerMask _portalLayerMask = LayerMask.GetMask("Portal");

        public void Run()
        {
            LocatePortal();
            DragPortal();
        }
    
        private void LocatePortal()
        {
            foreach (var downInput in _eventInputDownComponent) 
            {
                _downPos = _eventInputDownComponent.Get1(downInput).Down;

                if (GetPointInWorldSpace(out Vector3 locatePoint, out RaycastHit raycastHit, _defaultLayerMask, _downPos))
                {
                    _wallColor = GetColorFromWall(raycastHit);
                    _newPortal = CreateActualPortal(_wallColor);
                    
                    foreach (var activePortal in _activePortal)
                    {
                        if (_activePortal.Get1(activePortal).color == _newPortal.Get<PortalComponent>().color)
                            _activePortal.GetEntity(activePortal).Get<IsDestroyedComponent>();
                    }
                    
                    var newPosition = new Vector3(locatePoint.x, locatePoint.y, locatePoint.z + 1.31f);
                    
                    _newPortal.Get<ActivePortalComponent>();
                    _newPortal.Get<InActionPortalComponent>();
                    _newPortal.Get<SetPositionComponent>().position = newPosition;
                }
                _eventInputDownComponent.GetEntity(downInput).Del<EventInputDownComponent>();
            }
        }
        
        private void DragPortal()
        {
            foreach (var inActionPortal in _inActionPortal)
            {
                foreach (var holdAndDrag in _eventInputHoldAndDragComponent)
                {
                    var dragPos = _eventInputHoldAndDragComponent.Get1(holdAndDrag).Drag;
                    
                    if (GetPointInWorldSpace(out Vector3 locatePoint, out RaycastHit raycastHit, _portalLayerMask, dragPos))
                    {
                        _deltaPos = (dragPos - (Vector2)_downPos);
                        _inActionPortal.GetEntity(inActionPortal).Get<SetRotationComponent>().Eugle =
                            _deltaPos;
                    }
                    _eventInputHoldAndDragComponent.GetEntity(holdAndDrag).Del<EventInputHoldAndDragComponent>();
                }

                foreach (var inputUp in _eventInputUpComponent)
                {
                    _inActionPortal.GetEntity(inActionPortal).Del<InActionPortalComponent>();
                    _eventInputUpComponent.GetEntity(inputUp).Del<EventInputUpComponent>();
                }
                
            }
        }
        
        private bool GetPointInWorldSpace(out Vector3 locatePoint, out RaycastHit raycastHit,
            LayerMask targetLayer, Vector3 inputPos)
        {
            var actualCamera = GetCameraFromFilter();
            var ray = actualCamera.ScreenPointToRay(inputPos);
            var hasHit = Physics.Raycast(ray, out raycastHit,100f,targetLayer);
            
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
                //exception - if stf don't have any color
            }
            return _wallColor;
        }
        
        private EcsEntity CreateActualPortal(PortalComponent.PortalColor portalColor)
        {
            var portal = _world.CreatePortal(portalColor);
            portal.Get<InActionPortalComponent>();
            
            return portal;
        }

        private Camera GetCameraFromFilter()
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