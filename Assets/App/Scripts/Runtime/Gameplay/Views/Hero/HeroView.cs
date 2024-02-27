using BT.Runtime.Gameplay.Views.Camera;
using UnityEngine;

namespace BT.Runtime.Gameplay.Views.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroView : MonoBehaviour, ICharacterController, ICameraTarget
    {
        public CharacterController Controller => _cc ??= GetComponent<CharacterController>(); 
        public Transform TR => transform;

        private CharacterController _cc;

    }
}
