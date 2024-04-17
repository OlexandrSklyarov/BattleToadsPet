using BT.Runtime.Data.Configs;
using BT.Runtime.Gameplay.Views.Camera;
using UnityEngine;

namespace BT.Runtime.Gameplay.Views.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroView : MonoBehaviour, ICharacterController, ICameraTarget
    {
        [field: SerializeField] public CharacterConfig Config { get; private set; }        
        [field: SerializeField] public Transform Model { get; private set; }        
        public CharacterController Controller => _cc ??= GetComponent<CharacterController>(); 
        public Transform TR => transform;

        private CharacterController _cc;
    }
}
