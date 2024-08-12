using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BT.Runtime.Gameplay.Map.View
{
    public sealed class MapViewMediator : MonoBehaviour
    {
        public Vector3[] EnemySpawnPoints => _spawnPoints.ToArray();
        
        [SerializeField] private Transform _enemySpawnPointHolder;

        private List<Vector3> _spawnPoints = new();

        public void Init()
        {
            foreach(Transform tr in _enemySpawnPointHolder)
            {
                _spawnPoints.Add(tr.position);
            }
        }
    }
}
