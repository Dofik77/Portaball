using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components;
using ECS.Game.Components.TheDeeperComponent;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems
{
    public class PipeSystem : ReactiveSystem<EventAddComponent<PipeComponent>>
    {
        protected override EcsFilter<EventAddComponent<PipeComponent>> ReactiveFilter { get; }
        protected override void Execute(EcsEntity entity)
        {
            //some pipe logic
        }
    }
}