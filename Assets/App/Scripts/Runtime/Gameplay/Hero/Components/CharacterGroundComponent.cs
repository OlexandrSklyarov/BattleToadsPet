using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Components
{
    public struct CharacterGroundComponent
    {
        public BoxCollider FeetCollider;
        public CapsuleCollider BodyCollider;
        public bool IsGrounded;
    }
}