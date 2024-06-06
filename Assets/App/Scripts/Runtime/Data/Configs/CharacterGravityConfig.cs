using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/Character/CharacterGravityConfig", fileName = "CharacterGravityConfig")]
    public sealed class CharacterGravityConfig : ScriptableObject
    {
        [field: SerializeField, Min(0.01f)] public float MinVerticalVelocity {get; private set;} = 0.05f;
        [field: SerializeField, Min(0.01f)] public float FallMultiplier {get; private set;} = 2f;
        [field: SerializeField, Min(0.01f)] public float MaxFallVelocity {get; private set;} = 20f;
        [field: Space, SerializeField] public Vector3 CheckGroundOffset {get; private set;} = new Vector3(0f, 0.2f, 0f);
        [field: SerializeField, Min(0.01f)] public float CheckGroundSphereRadius {get; private set;} = 0.205f;
        [field: SerializeField] public LayerMask GroundLayer {get; private set;}
    }
}
