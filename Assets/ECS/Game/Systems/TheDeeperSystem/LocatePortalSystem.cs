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

        private EcsEntity newPortal;
        private PortalView _portalView;
        private PortalView _tempPortalView;

        private int counter = 0;

        private PortalComponent.PortalColor _wallColor;

    public void Run()
    {
        foreach (var downInput in _eventInputDownComponent) // перенести в отдельный метод 
        {
            var inputPos = _eventInputDownComponent.Get1(downInput).Down;

            if (TryGetTouchPointInWorldSpace(out Vector3 locatePoint, inputPos))
            {
                newPortal = CreateActualPortal(_wallColor);
                
                foreach (var activePortal in _activePortal)
                {
                    if (_activePortal.Get1(activePortal).color == newPortal.Get<PortalComponent>().color)
                        _activePortal.GetEntity(activePortal).Get<IsDestroyedComponent>(); // вынести в отдельный метод? 
                }
                
                var newPosition = new Vector3(locatePoint.x, locatePoint.y, locatePoint.z + 0.5f);
                
                newPortal.Get<ActivePortalComponent>();
                newPortal.Get<InActionPortalComponent>();
                newPortal.Get<SetPositionComponent>().position = newPosition;
            }
            _eventInputDownComponent.GetEntity(downInput).Del<EventInputDownComponent>();
        }
        DragPortal();
    }
        
        private void DragPortal()
        {
            foreach (var actionPortal in _inActionPortal)
            {
                foreach (var i in _eventInputHoldAndDragComponent)
                {
                    var inputPos = _eventInputHoldAndDragComponent.Get1(i).Drag;
                    
                    //нужен точно такой же Raycast - возможно нужно вынести LayerMask в отдельные 2 переменные 
                    //и передавать методы с рейкастом тот LayerMask, который нужен для получения объекта
                    //после получения объекта - меняем его Rotation через RotationComp 
                    //и живем счастливо
                }
            }
            
            //drag and up - Del<MovingPortal>
                
            //after foreach EventInputUpComponent Del<InAction> - отдельный метод или форыч
            
            // var inputPos = _eventInputHoldAndDragComponent.Get1(i).Drag;
            // _eventInputHoldAndDragComponent.GetEntity(i).Del<EventInputHoldAndDragComponent>();
            //
            // foreach (var downInput in _eventInputDownComponent)
            // {
            //     _eventInputDownComponent.GetEntity(downInput).Del<EventInputDownComponent>();
            // }
            //     
            // foreach (var inputUp in _eventInputUpComponent)
            // {
            //     _eventInputUpComponent.GetEntity(inputUp).Del<EventInputUpComponent>();
            // }
            // Debug.Log("HoldAndRag");
                
            //по сути это должен быть клоном придедущего алгоритма - с учетом того что у нас есть Drag и обработка Up + Мы обрабатываем лишь InActionPortal
        }
        
        private bool TryGetTouchPointInWorldSpace(out Vector3 locatePoint, Vector2 inputPos)
        {
            var actualCamera = GetCameraFromFilter();
            var ray = actualCamera.ScreenPointToRay(inputPos);
            var hasHit = Physics.Raycast(ray, out var raycastHit,100f, LayerMask.GetMask("Default"));
           
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