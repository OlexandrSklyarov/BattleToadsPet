using BT.Runtime.Data.Configs;
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
        public CharacterController Controller => _cc ??= GetComponent<CharacterController>(); 
        public Animator Animator => _animator ??= GetComponentInChildren<Animator>(); 
        public csHomebrewIK FootIK => _footIK ??= GetComponentInChildren<csHomebrewIK>(); 
        public Transform TR => transform;
        public EcsPackedEntity MyEntity => _ecsPackedEntity;

        private EcsPackedEntity _ecsPackedEntity;
        private CharacterController _cc;
        private Animator _animator;
        private csHomebrewIK _footIK;

        public void SetEntity(EcsPackedEntity ecsPackedEntity)
        {
            _ecsPackedEntity = ecsPackedEntity;
        }
    }
}
