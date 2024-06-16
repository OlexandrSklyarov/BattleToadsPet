using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Components
{
    public struct CharacterCheckGroundComponent
    {
        public BoxCollider FeetCollider;
        public Bounds BodyBounds;
        public Collider[] GroundResult;
        public Collider[] HeadBumpResult;

    }
}