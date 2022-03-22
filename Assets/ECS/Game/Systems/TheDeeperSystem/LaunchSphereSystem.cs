using ECS.Game.Systems.GameCycle;
using Leopotam.Ecs;

namespace ECS.Game.Systems
{
    public class LaunchSphereSystem
    {
        private readonly EcsFilter<EventInputDownComponent> _eventInputDownComponent; 
        
        private readonly EcsWorld _world;
    }
}