using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Systems.GameCycle;
using Leopotam.Ecs;

namespace ECS.Game.Systems
{
    public class LocatePortalSystem : IEcsUpdateSystem
    {
        // filter for 
        // down, drag, activePortal 
        private readonly EcsFilter<> _gameStage;
        public void Run()
        {
            // catch drag, catch down
            
        }
    }
}