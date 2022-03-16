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
        private readonly EcsFilter<ActivePortalComponent> _activePortal;
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
                _portalView = (PortalView) _portal.Get<LinkComponent>().View;
                _countOfPortal++;
            }

            foreach (var i in _eventInputHoldAndDragComponent)
            {
                var inputPos = _eventInputHoldAndDragComponent.Get1(i).Down; 
                
                if (TryGetTouchPointInWorldSpace(out Vector3 locatePoint, inputPos))
                {
                    Debug.Log("GetTouchPointInWorldSpace");
                    _portalView.transform.position = locatePoint;
                }
            }
        }
        
        public bool TryGetTouchPointInWorldSpace(out Vector3 locatePoint, Vector2 inputPos)
        {
            var actualCamera = GetCameraFromFilter();   
            var ray = actualCamera.ScreenPointToRay(inputPos);
            var hasHit = Physics.Raycast(ray, out var raycastHit);
            locatePoint = raycastHit.point;
            
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