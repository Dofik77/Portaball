using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Game.Systems.Move
{
    public class MoveRotateToTargetSystem : IEcsUpdateSystem
    {
        private readonly EcsFilter<PositionComponent, TargetPositionComponent> _position;
        private readonly EcsFilter<RotationComponent, TargetRotationComponent> _rotation;
        public void Run()
        {
            foreach (var i in _position)
            {
                var speed = _position.Get2(i).Speed;
                var target = _position.Get2(i).Value;
                ref var pos = ref _position.Get1(i).Value;
                pos = Vector3.MoveTowards(pos, target, Time.deltaTime * speed);
                if(Vector3.Distance(pos, target) < 0.01f)
                    _position.GetEntity(i).Del<TargetPositionComponent>();
            }
            
            foreach (var i in _rotation)
            {
                var speed = _rotation.Get2(i).Speed;
                var target = _rotation.Get2(i).Value;
                ref var rot = ref _rotation.Get1(i).Value;
                rot = Quaternion.RotateTowards(rot, target, Time.deltaTime * speed);
                if(Vector3.Distance(target.eulerAngles, rot.eulerAngles) < 0.01f)
                    _rotation.GetEntity(i).Del<TargetRotationComponent>();
            }
        }
    }
}