using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/MainConfig", fileName = "MainConfig")]
    public sealed class MainConfig : ScriptableObject
    {
        [field: SerializeField] public LevelDataBase LevelDataBase {get; private set;}        
        [field: Space, SerializeField] public UIElementConfig UI {get; private set;}        
        [field: Space, SerializeField] public HeroConfig Hero {get; private set;}        
    }
}