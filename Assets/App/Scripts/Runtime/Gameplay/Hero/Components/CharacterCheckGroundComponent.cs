using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Components
{
    public struct CharacterCheckGroundComponent
    {
        public BoxCollider FeetCollider;
        public CapsuleCollider BodyCollider;
        public Collider[] GroundResult;
        public Collider[] HeadBumpResult;
    }
}