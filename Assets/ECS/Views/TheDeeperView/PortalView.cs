using System;
using Ecs.Views.Linkable.Impl;
using UnityEngine;

namespace ECS.Views.Impls
{
    public class PortalView : LinkableView
    {
        public event Action<SphereCharacterView> OnSphereCollision;
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out SphereCharacterView characterView))
            {
                OnSphereCollision?.Invoke(characterView);
            } 
            // relacete in System
            //save velocity and change rotate 
        }
    }
}