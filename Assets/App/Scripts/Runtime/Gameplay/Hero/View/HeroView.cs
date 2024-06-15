using BT.Runtime.Data.Configs;
using BT.Runtime.Gameplay.Hero.Components;
using BT.Runtime.Gameplay.Views.Camera;
using FischlWorks;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Views.Hero
{
    [RequireComponent(typeof(Rigidbody))]
    public class HeroView : MonoBehaviour, ICharacterController, ICameraTarget
    {
        [field: SerializeField] public CharacterConfig Config { get; private set; }        
        [field: SerializeField] public Transform Model { get; private set; }        
        [field: SerializeField] public CapsuleCollider BodyCollider { get; private set; }        
        [field: SerializeField] public BoxCollider FeetCollider { get; private set; }        
        public Rigidbody RB => _rb ??= GetComponent<Rigidbody>(); 
        public Animator Animator => _animator ??= GetComponentInChildren<Animator>(); 
        public csHomebrewIK FootIK => _footIK ??= GetComponentInChildren<csHomebrewIK>(); 
        public Transform TR => transform;
        public EcsPackedEntity MyEntity => _ecsPackedEntity;

        private EcsWorld _world;
        private EcsPackedEntity _ecsPackedEntity;
        private Rigidbody _rb;
        private Animator _animator;
        private csHomebrewIK _footIK;

        public void SetEntity(EcsWorld world, EcsPackedEntity ecsPackedEntity)
        {
            _world = world;
            _ecsPackedEntity = ecsPackedEntity;
        }

        private void OnDrawGizmos() 
        {
            if (_ecsPackedEntity.Unpack(_world, out int entity))
            {
                var characterGroundPool = _world.GetPool<CharacterCheckGroundComponent>();
                var characterConfigPool = _world.GetPool<CharacterConfigComponent>();
                ref var ground = ref characterGroundPool.Get(entity); 
                ref var config = ref characterConfigPool.Get(entity); 

                //head
                var boxCastOrigin = new Vector3
                (
                    ground.FeetCollider.bounds.center.x,
                    ground.BodyCollider.bounds.max.y,
                    ground.FeetCollider.bounds.center.z
                );

                var boxCastSize = new Vector3
                (
                    ground.FeetCollider.bounds.size.x * config.ConfigRef.Gravity.HeadWidth,
                    config.ConfigRef.Gravity.HeadDetectionRayLength,
                    ground.FeetCollider.bounds.size.z * config.ConfigRef.Gravity.HeadWidth
                );

                Gizmos.color = Color.cyan;
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

                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(boxCastOrigin, boxCastSize);           
            }
        }
    }
}
