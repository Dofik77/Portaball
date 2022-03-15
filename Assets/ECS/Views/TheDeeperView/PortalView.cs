using System;
using ECS.Game.Components.TheDeeperComponent;
using Ecs.Views.Linkable.Impl;
using UnityEngine;
using Object = System.Object;

namespace ECS.Views.Impls
{
    public class PortalView : LinkableView
    {
        [SerializeField] public PortalComponent.PortalColor color;
        public event Action<SphereCharacterView> OnSphereCollision;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out SphereCharacterView characterView))
            {
                OnSphereCollision?.Invoke(characterView);
            }
        }
    }
}