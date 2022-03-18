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
        
        private EcsEntity _portal;
        private PortalView _portalView;
        private PortalView _tempPortalView;

        private PortalComponent.PortalColor _wallColor;

        public void Run()
        {
            //replace in another method??? 
            foreach (var i in _eventInputDownComponent)
            {
                var inputPos = _eventInputDownComponent.Get1(i).Down;

                if (TryGetTouchPointInWorldSpace(out Vector3 locatePoint, inputPos)) 
                {
                    foreach (var j in _activePortal) 
                    {
                        //check existing portal(some_color) before locate new portal(some_color)
                        if (_wallColor == _activePortal.Get1(j).color)
                            _activePortal.GetEntity(j).Get<IsDestroyedComponent>();
                        Debug.Log("PortalTryDelete");
                    }

                   
                    
                    var newPortal = GetActualPortal(_wallColor);
                    var newPosition = new Vector3(locatePoint.x, locatePoint.y, locatePoint.z + 0.5f);
                   
                    _portalView = (PortalView) newPortal.Get<LinkComponent>().View;
                    _portalView.transform.position = newPosition;
                   
                }
                _eventInputDownComponent.GetEntity(i).Del<EventInputDownComponent>();
            }
            DragPortal();
        }
        
        private void DragPortal()
        {
            foreach (var i in _eventInputHoldAndDragComponent)
            {
                var inputPos = _eventInputHoldAndDragComponent.Get1(i).Drag;

                foreach (var inActionPortal in _inActionPortal)
                {
                    //logic 
                }
                    
                
                //drag and up - Del<MovingPortal>
                
                //after foreach EventInputUpComponent Del<InAction> - отдельный метод или форыч
            }
        }
        
        private bool TryGetTouchPointInWorldSpace(out Vector3 locatePoint, Vector2 inputPos)
        {
            var actualCamera = GetCameraFromFilter();
            var ray = actualCamera.ScreenPointToRay(inputPos);
            var hasHit = Physics.Raycast(ray, out var raycastHit,100f, LayerMask.GetMask("Default"));
           
            _wallColor = GetColorFromWall(raycastHit);
            locatePoint = raycastHit.point;
            
            return hasHit;
        }
        
        private PortalComponent.PortalColor GetColorFromWall(RaycastHit raycastHit)
        {
            if (raycastHit.transform.gameObject.TryGetComponent(out WallView wallView))
            {
                _wallColor = wallView.color;
            }
            return _wallColor;
        }
        
        private EcsEntity GetActualPortal(PortalComponent.PortalColor portalColor)
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


// catch drag, catch down
// down - locate, drag rotate
          
//позиция надавленного пальца 
//тык на экран -> ставится телепорт
//начал водить -> меняется rotaion
//тык в другое место -> новый телепорт и так далее