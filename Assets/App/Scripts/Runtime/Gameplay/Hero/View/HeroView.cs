using System;
using System.Diagnostics;
using BT.Runtime.Data.Configs;
using BT.Runtime.Gameplay.Hero.Components;
using BT.Runtime.Gameplay.Hero.View.Animation;
using BT.Runtime.Gameplay.Views.Camera;
using FischlWorks;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Views.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroView : MonoBehaviour, ICharacterController, ICameraTarget
    {
        [field: SerializeField] public CharacterConfig Config { get; private set; }        
        [field: SerializeField] public Transform Model { get; private set; }            
        [field: SerializeField] public BoxCollider FeetCollider { get; private set; }        
        public CharacterController CC => _cc ??= GetComponent<CharacterController>(); 
        public Animator Animator => _animator ??= GetComponentInChildren<Animator>(); 
        public csHomebrewIK FootIK => _footIK ??= GetComponentInChildren<csHomebrewIK>(); 
        public Transform TR => transform;
        public EcsPackedEntity MyEntity => _ecsPackedEntity;

        private EcsWorld _world;
        private EcsPackedEntity _ecsPackedEntity;
        private bool _isInit;
        private CharacterController _cc;
        private Animator _animator;
        private csHomebrewIK _footIK;

        public void Init(EcsWorld world, EcsPackedEntity ecsPackedEntity)
        {
            _world = world;
            _ecsPackedEntity = ecsPackedEntity;

            _isInit = true;
        }

        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmos() 
        {
            if (_ecsPackedEntity.Unpack(_world, out int entity))
            {
                var characterGroundPool = _world.GetPool<CharacterCheckGroundComponent>();
                var characterConfigPool = _world.GetPool<CharacterConfigComponent>();
                var characterMovementPool = _world.GetPool<MovementDataComponent>();
                ref var ground = ref characterGroundPool.Get(entity); 
                ref var config = ref characterConfigPool.Get(entity); 
                ref var movement = ref characterMovementPool.Get(entity); 

                //head
                var boxCastOrigin = new Vector3
                (
                    ground.FeetCollider.bounds.center.x,
                    ground.BodyBounds.max.y,
                    ground.FeetCollider.bounds.center.z
                );

                var boxCastSize = new Vector3
                (
                    ground.FeetCollider.bounds.size.x * config.ConfigRef.Gravity.HeadWidth,
                    config.ConfigRef.Gravity.HeadDetectionRayLength,
                    ground.FeetCollider.bounds.size.z * config.ConfigRef.Gravity.HeadWidth
                );

                Gizmos.color = (movement.IsBumpedHead) ? Color.red : Color.cyan;
                Gizmos.DrawCube(boxCastOrigin, boxCastSize);     

                //ground var boxCastOrigin = new Vector3
                boxCastOrigin = new Vector3
                (
                    ground.FeetCollider.bounds.center.x,
                    ground.FeetCollider.bounds.min.y,
                    ground.FeetCollider.bounds.center.z
                );

                boxCastSize = new Vector3
                (
                    ground.FeetCollider.bounds.size.x,
                    config.ConfigRef.Gravity.GroundDetectionRayLength,
                    ground.FeetCollider.bounds.size.z
                );   

                Gizmos.color = (movement.IsGround) ? Color.green : Color.yellow;
                Gizmos.DrawCube(boxCastOrigin, boxCastSize);           
            }
        }

        public void RegisterAnimBehaviour(IHeroAnimBehaviour heroAnimBehaviour)
        {
            if (!_isInit) return;
            
            heroAnimBehaviour.Init(_ecsPackedEntity, _world);
        }
    }
}
