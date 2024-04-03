using Game.Runtime.Services.LoadingOperations.View;
using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/UIElementConfig", fileName = "UIElementConfig")]
    public sealed class UIElementConfig : ScriptableObject
    {
        [field: Space, SerializeField] public LoadingScreenView LoadingScreenPrefab {get; private set;}       
    
    }
}