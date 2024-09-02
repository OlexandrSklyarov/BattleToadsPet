using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace BT.Runtime.Gameplay.Services.GameWorldData
{
    public sealed class SharedData
    {
        public IObjectResolver DIResolver;
        public EcsPackedEntity HeroEntity;
        public readonly Dictionary<Collider, EcsPackedEntity> EntityColliders = new();
    }
}