using System.Collections.Generic;
using BT.Runtime.Gameplay.Combat.Services;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace BT.Runtime.Gameplay.Services.GameWorldData
{
    public sealed class SharedData
    {
        public IObjectResolver DIResolver;
        public EcsPackedEntity HeroEntity;
        public DetectTargetService DetectTargetService;
        public readonly Dictionary<Collider, EcsPackedEntity> EntityColliders = new();
    }
}