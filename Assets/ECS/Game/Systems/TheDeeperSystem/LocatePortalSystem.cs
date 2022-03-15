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
            
            //позиция надавленного пальца 
            //тык на экран -> ставится телепорт
            //начал водить -> меняется rotaion
            //тык в другое место -> новый телепорт и так далее
            foreach (var i in _eventInputHoldAndDragComponent)
            {
                var inpitPos = _eventInputHoldAndDragComponent.Get1(i).Down; // down - locate, drag rotate
                
                //raycast из этого места
                //и учитывать можно ли ставить портал с помощью тегов
                
                
                
                // portalView.transform.position = inpitPos;
            }


            // catch drag, catch down

        }

        
    }
}