using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Views.Impls;
using Leopotam.Ecs;

namespace ECS.Game.Systems.TheDeeperSystem
{
    public class ChangeModelPlayerSystem : IEcsUpdateSystem
    {
        private EcsFilter<SpherePlayerComponent, LinkComponent> _player;
        public void Run()
        {
            foreach (var player in _player)
            {
                switch (1)
                {
                    
                }
            }
        }
        
        
    }
}