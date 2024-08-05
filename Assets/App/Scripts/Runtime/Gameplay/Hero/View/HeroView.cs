using BT.Runtime.Data.Configs;
using BT.Runtime.Gameplay.Combat.View;
using BT.Runtime.Gameplay.Hero.Services.Attack;
using BT.Runtime.Gameplay.Hero.View.Animations;
using BT.Runtime.Gameplay.Views.Camera;
using FischlWorks;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Views.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroView : MonoBehaviour, ICharacterController, ICameraTarget, IDamagableEntity
    {
        [field: SerializeField] public CharacterConfig Config { get; private set; }        
        [field: SerializeField] public Transform Model { get; private set; }            
        [field: SerializeField] public BoxCollider FeetCollider { get; private set; }        
        [field: Space, SerializeField] public AttackPoint[] AttackPoints { get; private set; }        
        public CharacterController CC => _cc ??= GetComponent<CharacterController>(); 
        public Animator Animator => _animator ??= GetComponentInChildren<Animator>(); 
        public csHomebrewIK FootIK => _footIK ??= GetComponentInChildren<csHomebrewIK>(); 
        public Transform TR => transform;
        public EcsPackedEntity MyEntity => _ecsPackedEntity;
        public EcsPackedEntity DamageEntity => _ecsPackedEntity;
        
        private EcsWorld _world;
        private EcsPackedEntity _ecsPackedEntity;
        private HeroAttackService _attackService;
        private CharacterController _cc;
        private Animator _animator;
        private csHomebrewIK _footIK;
        private bool _isInit;

        public void Init(EcsWorld world, EcsPackedEntity ecsPackedEntity)
        {
            _world = world;
            _ecsPackedEntity = ecsPackedEntity;

            _attackService = new HeroAttackService(_world, _ecsPackedEntity, 
                AttackPoints, Config.Attack, Config.Animation);

            _isInit = true;
        }

        // [Conditional("UNITY_EDITOR")]
        // private void OnDrawGizmos() 
        // {
        //     if (_ecsPackedEntity.Unpack(_world, out int entity))
        //     {
        //         var characterGroundPool = _world.GetPool<CharacterCheckGroundComponent>();
        //         var characterConfigPool = _world.GetPool<CharacterConfigComponent>();
        //         var characterMovementPool = _world.GetPool<MovementDataComponent>();
        //         ref var ground = ref characterGroundPool.Get(entity); 
        //         ref var config = ref characterConfigPool.Get(entity); 
        //         ref var movement = ref characterMovementPool.Get(entity); 
                
        //         var boxCastOrigin = new Vector3
        //         (
        //             ground.FeetCollider.bounds.center.x,
        //             ground.FeetCollider.bounds.min.y,
        //             ground.FeetCollider.bounds.center.z
        //         );

        //         var boxCastSize = new Vector3
        //         (
        //             ground.FeetCollider.bounds.size.x,
        //             config.ConfigRef.Gravity.GroundDetectionRayLength,
        //             ground.FeetCollider.bounds.size.z
        //         );   

        //         Gizmos.color = (movement.IsGround) ? Color.green : Color.yellow;
        //         Gizmos.DrawCube(boxCastOrigin, boxCastSize);           
        //     }
        // }

        public void RegisterAnimBehaviour(IHeroAnimBehaviour heroAnimBehaviour)
        {
            if (!_isInit) return;
            
            heroAnimBehaviour.Init(_attackService);
        }        
    }
}
