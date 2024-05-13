using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/CharacterConfig", fileName = "CharacterConfig")]
    public sealed class CharacterConfig : ScriptableObject
    {
        [field: Space, SerializeField] public EngineConfig Engine {get; private set;}
        [field: Space, SerializeField] public CharacterGravityConfig Gravity {get; private set;}        
    }
}