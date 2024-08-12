using System;
using BT.Runtime.Gameplay.Map.View;
using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/LevelDataBase", fileName = "LevelDataBase")]
    public sealed class LevelDataBase : ScriptableObject
    {
        [field: SerializeField] public Level[] Levels {get; private set;}

        [Serializable]
        public sealed class Level
        {
            [field: SerializeField] public string LevelName {get; private set;} = "TestLevel";
            [field: SerializeField] public string Scene {get; private set;} = "LevelTest";            
            [field: SerializeField] public string SceneEnvironment {get; private set;} = "Environment";            
            [field: SerializeField] public MapViewMediator MapPrefab {get; private set;}           
        }
    }
}