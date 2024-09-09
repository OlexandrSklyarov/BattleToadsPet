using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Combat.Services
{
    public sealed class DetectTargetService
    {
        private readonly Collider[] _colliderResult;

        public DetectTargetService()
        {
            _colliderResult = new Collider[32];
        }

        public bool FindTargetsInRadius<T>(Vector3 position, float radius, out T target)  where T : MonoBehaviour
        {
            var findCount = Physics.OverlapSphereNonAlloc (position, radius, _colliderResult);
            
            for (int i = 0; i < findCount; i++)
            {
                var col = _colliderResult[i];

                if (col == null) continue;                

                if (col.TryGetComponent(out target)) return true;                              
            }
            
            target = default;
            return false;
        }

        public IEnumerable<Collider> FindCollidersInRadius(Vector3 position, float radius)
        {
            var findCount = Physics.OverlapSphereNonAlloc (position, radius, _colliderResult);
            
            for (int i = 0; i < findCount; i++)
            {
                var col = _colliderResult[i];

                if (col == null) continue;                

                yield return col;                           
            }
        }
    }
}
