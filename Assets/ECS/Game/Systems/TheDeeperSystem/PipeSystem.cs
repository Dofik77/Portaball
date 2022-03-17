using ECS.Core.Utils.ReactiveSystem;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.Impls;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems
{
    public class PipeSystem : ReactiveSystem<EventAddComponent<PipeComponent>>
    {
        protected override EcsFilter<EventAddComponent<PipeComponent>> ReactiveFilter { get; }
        private EcsFilter<SphereCharacterComponent, LinkComponent> _sphere;
        protected override void Execute(EcsEntity entity)
        {
            PipeView pipeView = entity.Get<LinkComponent>().View as PipeView;
            pipeView.OnSphereTrigger += DeactiveSphere;
        }

        private void DeactiveSphere(PipeView pipeView)
        {
            if (pipeView.PipeState == PipeComponent.PipeState.Exit)
            {
                foreach (var i in _sphere)
                {
                    var sphereView = _sphere.Get2(i).View as SphereCharacterView;
                    sphereView.gameObject.SetActive(false);
                    //прописать логику вызова VIEW при попадании в трубу 
                }
            }
        }
    }
}