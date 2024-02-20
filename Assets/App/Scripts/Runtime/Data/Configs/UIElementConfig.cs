using BT.Runtime.UI.Scenes;
using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/UIElementConfig", fileName = "UIElementConfig")]
    public sealed class UIElementConfig : ScriptableObject
    {
        [field: Space, SerializeField] public LoadingScreen LoadingScreenPrefab {get; private set;}       
    
    }
}