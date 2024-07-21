using System;
using BT.Runtime.Data;
using BT.Runtime.Gameplay.Extensions;
using BT.Runtime.Gameplay.Hero.Components;
using BT.Runtime.Gameplay.Hero.View.Animations;
using Leopotam.EcsLite;
using UnityEngine;
using Util;

namespace BT.Runtime.Gameplay.Hero.Services
{
    public sealed class HeroAttackService : IAttackService
    {
        private readonly EcsPackedEntity _packedEntity;
        private readonly EcsWorld _world;

        public HeroAttackService(EcsPackedEntity packedEntity, EcsWorld world)
        {
            _packedEntity = packedEntity;
            _world = world;
        }

        public bool IsAttackExecuted()
        {
            if (_packedEntity.Unpack(_world, out int entity))
            {
                ref var attack = ref _world.GetComponent<CharacterAttackComponent>(entity);
                return attack.IsExecuted || attack.IsExecutedPower;
            }

            return false;
        }       

        public int GetAttackAnimID(AttackType type)
        {
            return type switch
            {
                AttackType.Attack_1 => GameConstants.AnimatorPrm.ATTACK_1,
                AttackType.Attack_2 => GameConstants.AnimatorPrm.ATTACK_2,
                _ => GameConstants.AnimatorPrm.ATTACK_3,
            };
        }

        public void ResetAttackExecuted()
        {
            if (_packedEntity.Unpack(_world, out int entity))
            {
                ref var attack = ref _world.GetComponent<CharacterAttackComponent>(entity);
                attack.IsExecuted = false; 
                attack.IsExecutedPower = false;                
            }
        }

        public void ApllyAttack(AttackType type)
        {
            var power = 100;
            DebugUtil.PrintColor($"ApllyAttack pow {power}", Color.cyan);

            if (_packedEntity.Unpack(_world, out int entity))
            {
                //ref var attack = ref _world.GetComponent<CharacterAttackComponent>(entity);
            }
        }
    }
}
