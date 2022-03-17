using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
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
        private readonly EcsFilter<PortalComponent, LinkComponent, ActivePortalComponent> _activePortal;
        private readonly EcsFilter<CameraComponent, LinkComponent> _filterCamera;
        
        private readonly EcsWorld _world;
        private EcsEntity _portal;
        private PortalView _portalView;
        private int _countOfPortal = 0;
        
        public void Run()
        {
            if (_countOfPortal < 1)
            {
                _portal = _world.CreatePortal();
                _portal.Get<ActivePortalComponent>();
                _countOfPortal++;
            }

            foreach (var i in _eventInputHoldAndDragComponent)
            {
                // else our portal don't decected 
                //because portal not link ( i don't know why ) 
                foreach (var activePortal in _activePortal)
                {
                    var inputPos = _eventInputHoldAndDragComponent.Get1(i).Down;
                    Debug.Log(inputPos.x + "+" + inputPos.y);
                    _portalView = (PortalView) _activePortal.Get2(activePortal).View;
                    
                    if (TryGetTouchPointInWorldSpace(out Vector3 locatePoint, inputPos))
                    {
                        var position = _portalView.transform.position;
                        position = new Vector3(locatePoint.x, locatePoint.y);
                        _portalView.transform.position = position;
                    }
                }
               
            }
        }
        
        public bool TryGetTouchPointInWorldSpace(out Vector3 locatePoint, Vector2 inputPos)
        {
            var actualCamera = GetCameraFromFilter();   
            var ray = actualCamera.ScreenPointToRay(inputPos);
            var hasHit = Physics.Raycast(ray, out var raycastHit);
            locatePoint = raycastHit.point;
            Debug.DrawRay(actualCamera.transform.position, locatePoint, Color.red);
            
            return hasHit && IsHitInCurrentWall(raycastHit);
        }

        private bool IsHitInCurrentWall(RaycastHit raycastHit)
        {
            return true;
            //check current color of wall
            //можно ли ставить портал с помощью тегов
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
        
        // catch drag, catch down
        // down - locate, drag rotate
          
        //позиция надавленного пальца 
        //тык на экран -> ставится телепорт
        //начал водить -> меняется rotaion
        //тык в другое место -> новый телепорт и так далее
    }
}