﻿using System;
using DataBase.Game;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components;
using ECS.Game.Components.Flags;
using ECS.Game.Components.TheDeeperComponent;
using ECS.Views.Impls;
using Leopotam.Ecs;
using Services.Uid;
using UnityEngine;
using UnityEngine.UIElements;

namespace ECS.Utils.Extensions
{
    public static class GameExtensions
    {
        public static EcsEntity CreateCamera(this EcsWorld world) //maybe relocate from gameINITSystem
        {
            var entity = world.NewEntity();
            entity.Get<UIdComponent>().Value = UidGenerator.Next();
            entity.Get<PositionComponent>();
            entity.Get<RotationComponent>().Value = Quaternion.Euler(new Vector3(47,0,0));
            entity.Get<CameraComponent>();
            entity.Get<PrefabComponent>().Value = "MainCamera";
            entity.Get<EventAddComponent<PrefabComponent>>();
            return entity;
        }

        public static EcsEntity CreatePortal(this EcsWorld world, PortalComponent.PortalColor portalColor)
        {
            var entity = world.NewEntity();
            entity.Get<PortalComponent>().color = portalColor;
            entity.Get<EventAddComponent<PortalComponent>>();
            
            entity.Get<PrefabComponent>().Value = Enum.GetName(typeof(PortalComponent.PortalColor), portalColor);
            entity.Get<EventAddComponent<PrefabComponent>>();
            
            entity.Get<SetPositionComponent>();
            entity.Get<UIdComponent>().Value = UidGenerator.Next();
            
            return entity;
        }
    }
}