using System;
using Ecs.Views.Linkable.Impl;
using UnityEngine;

namespace ECS.Views.Impls
{
    public class SpherePlayerView : LinkableView
    {
        [SerializeField] private Animator _animator;
        [SerializeField] public Rigidbody rigidbody;
    }
}