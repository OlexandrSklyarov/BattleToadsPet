using System.Linq;
using BT.Runtime.Data;
using BT.Runtime.Data.Configs;
using BT.Runtime.Gameplay.Combat.Components;
using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.Extensions;
using BT.Runtime.Gameplay.Hero.Components;
using BT.Runtime.Gameplay.Hero.View.Animations;
using Leopotam.EcsLite;
using UnityEngine;
using Util;

namespace BT.Runtime.Gameplay.Hero.Services.Attack
{
    public sealed class HeroAttackService : IAttackService
    {
        private readonly EcsPackedEntity _packedEntity;
        private readonly AttackConfig _attackConfig;
        private readonly EcsWorld _world;
        private readonly AnimationConfig _animConfig;
        private readonly AttackPoint[] _attackPoints;

        public HeroAttackService(EcsWorld world,
            EcsPackedEntity packedEntity,
            AttackPoint[] attackPoints,
            AttackConfig attackConfig,
            AnimationConfig animConfig)
        {
            _world = world;
            _packedEntity = packedEntity;
            _attackConfig = attackConfig;
            _animConfig = animConfig;
            _attackPoints = attackPoints;
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
                AttackType.HandAttack_1 => GameConstants.AnimatorPrm.ATTACK_1,
                AttackType.HandAttack_2 => GameConstants.AnimatorPrm.ATTACK_2,
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

        public void ApllyAttack(AttackType type, AttackPointType pointType, bool isPowerAttack = false)
        {
            var point = _attackPoints.FirstOrDefault(x => x.Type == pointType);

            if (point == null)
            {
                DebugUtil.PrintColor($"Not found attack point with type {pointType}", Color.red);
                return;
            }

            if (_packedEntity.Unpack(_world, out int entity))
            {
                var pool = _world.GetPool<AttackRequestComponent>();

                if (pool.Has(entity)) return;

                ref var heroView = ref _world.GetComponent<ViewModelTransformComponent>(entity);

                ref var attackRequest = ref pool.Add(entity);
                attackRequest.AttackDirection = heroView.ModelTransformRef.forward;
                attackRequest.Position = point.Point.position;
                attackRequest.Radius = _attackConfig.Radius;
                attackRequest.Damage = _attackConfig.Damages.First(x => x.Type == pointType).Power;

                //add power
                if (isPowerAttack)
                {
                    attackRequest.Damage *= _attackConfig.PowerDamageMultiplier;
                }

                DebugUtil.PrintColor($"ApllyAttack pow {attackRequest.Damage} - [{pointType}]", Color.cyan);
            }
        }

        public float GetCrossFadeTime() => _animConfig.AttackCrosfadeAnimime;
    }
}
