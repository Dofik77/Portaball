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
        private readonly EcsFilter<PortalComponent, LinkComponent, ActivePortalComponent> _activePortal; //сделать компонент "в движении" при движении
        private readonly EcsFilter<CameraComponent, LinkComponent, ActiveCameraComponent> _filterCamera;
        
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
                //в зависимости от кол-во стен n = кол-во n 
                //перенести в цикл ( или отдельный метод ) - портал должен создаваться после нажатия на экран,а не до
            }

            foreach (var i in _eventInputDownComponent)
            {
                // else our portal don't decected 
                //because portal not link ( i don't know why ) 
                
                foreach (var activePortal in _activePortal)
                {
                    var inputPos = _eventInputDownComponent.Get1(i).Down;
                    _portalView = (PortalView) _activePortal.Get2(activePortal).View;

                    if (TryGetTouchPointInWorldSpace(out Vector3 locatePoint, inputPos))
                    {
                        var newPosition = new Vector3(locatePoint.x, locatePoint.y, locatePoint.z + 0.5f);
                        _portalView.transform.position = newPosition;
                    }
                    _eventInputDownComponent.GetEntity(i).Del<EventInputDownComponent>();
                }
            }

            foreach (var i in _eventInputHoldAndDragComponent)
            {
                //учитывать есть ли компонент движаемый портал
                var inputPos = _eventInputHoldAndDragComponent.Get1(i).Drag;
                //drag and up - Del<MovingPortal>
            }
        }
        
        public bool TryGetTouchPointInWorldSpace(out Vector3 locatePoint, Vector2 inputPos)
        {
            var actualCamera = GetCameraFromFilter();
            //var point = actualCamera.ScreenToWorldPoint(inputPos);
            var ray = actualCamera.ScreenPointToRay(inputPos);
            var hasHit = Physics.Raycast(ray, out var raycastHit,100f, LayerMask.GetMask("Default"));
            //проблема - рейкаст сталкивался с сами телепортом и менял позицию телепорта относительно колладйа предедущего 
            //телепорта, поэтому нам надо искать рейкасты лишь у по Defualt
            locatePoint = raycastHit.point;
            Debug.DrawRay(actualCamera.transform.position, locatePoint, Color.red);
            
            return hasHit && IsHitInCurrentWall(raycastHit);
        }
        

        private bool IsHitInCurrentWall(RaycastHit raycastHit)
        {
            return true;
            //check current color of wall
            //можно ли ставить портал с помощью тегов
            // or by trining wall.get.view.color == portal.color
            // or tag == prefab.portal.color
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