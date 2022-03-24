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
        public event Action<Uid, PortalComponent.PortalColor, Transform> OnSphereTrigger;
        public event Action<PortalView> OnPortalSpawn;
        
        [SerializeField] public PortalComponent.PortalColor color;
        [SerializeField] public GameObject _pointToLocate;
        [SerializeField] public BoxCollider _stopCollider;
        [SerializeField] public GameObject _portalEffect;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Sphere"))
            {
                OnSphereTrigger?.Invoke(Entity.Get<UIdComponent>().Value, 
                    Entity.Get<PortalComponent>().color, transform);
                _stopCollider.enabled = false;
                //srp - translate not view, transfer id View
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Sphere"))
            {
                _stopCollider.enabled = true;
            }
        }
    }
}