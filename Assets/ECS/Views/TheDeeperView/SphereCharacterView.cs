using System;
using Ecs.Views.Linkable.Impl;
using UnityEngine;

namespace ECS.Views.Impls
{
    public class SphereCharacterView : LinkableView
    {
        [SerializeField] private Animator _animator;
        [SerializeField] public Rigidbody rigidbody;
    }
}