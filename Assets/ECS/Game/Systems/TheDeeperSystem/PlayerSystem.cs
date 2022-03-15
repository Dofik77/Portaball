﻿using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using Game.Utils.MonoBehUtils;
using Leopotam.Ecs;
using Zenject;

namespace ECS.Game.Systems
{
    public class PlayerSystem : ReactiveSystem<EventAddComponent<PlayerComponent>>
    {
        protected override EcsFilter<EventAddComponent<PlayerComponent>> ReactiveFilter { get; }
        [Inject] private readonly GetPointFromScene _getPointFromScene;
        protected override void Execute(EcsEntity entity)
        {
            var point = _getPointFromScene.GetPoint("Player");
            entity.Get<LinkComponent>().View.Transform.position = point.position;
        }
    }
}