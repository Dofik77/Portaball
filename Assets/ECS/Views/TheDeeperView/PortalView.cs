using System;
using ECS.Game.Components;
using ECS.Game.Components.TheDeeperComponent;
using Ecs.Views.Linkable.Impl;
using Leopotam.Ecs;
using PdUtils;
using Services.Uid;
using UnityEngine;
using Object = System.Object;

namespace ECS.Views.Impls
{
    public class PortalView : LinkableView
    {
        [SerializeField] public PortalComponent.PortalColor color;
        [SerializeField] public GameObject _pointToLocate;
        public event Action<Uid, PortalComponent.PortalColor> OnSphereTrigger;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Sphere"))
            {
                OnSphereTrigger?.Invoke(Entity.Get<UIdComponent>().Value, 
                    Entity.Get<PortalComponent>().color);
                //srp - translate not view, transfer id View
            }
        }
    }
}