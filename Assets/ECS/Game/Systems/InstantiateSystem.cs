using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components;
using ECS.Utils;
using ECS.Views;
using Leopotam.Ecs;
using UnityEngine;
using Zenject;

namespace ECS.Game.Systems
{
    public class InstantiateSystem : ReactiveSystem<EventAddComponent<PrefabComponent>>
    {
        [Inject] private readonly ISpawnService<EcsEntity, ILinkable> _spawnService; //inject and Nedo-Singletone
        protected override EcsFilter<EventAddComponent<PrefabComponent>> ReactiveFilter { get; } //рефлексия 
        protected override void Execute(EcsEntity entity)
        {
            var linkable = _spawnService.Spawn(entity);
            linkable?.Link(entity);
            entity.Get<LinkComponent>().View = linkable;
        }
    }
}