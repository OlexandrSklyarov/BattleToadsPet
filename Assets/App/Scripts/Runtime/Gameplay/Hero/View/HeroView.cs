using BT.Runtime.Data.Configs;
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

        private EcsPackedEntity _ecsPackedEntity;
        private Rigidbody _rb;
        private Animator _animator;
        private csHomebrewIK _footIK;

        public void SetEntity(EcsPackedEntity ecsPackedEntity)
        {
            _ecsPackedEntity = ecsPackedEntity;
        }
    }
}
