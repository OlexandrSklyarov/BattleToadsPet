using BT.Runtime.Data.Configs;
using UnityEngine;
using UnityEngine.AI;

namespace BT.Runtime.Gameplay.Enemy.View
{
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class EnemyView : MonoBehaviour
    {
        [field: SerializeField] public Collider Collider { get; private set; }
        [field: SerializeField] public Transform ViewBody { get; private set; }
        [field: SerializeField] public WarrirorConfig Config { get; private set; }
        public Animator Animator => _animator ??= GetComponentInChildren<Animator>();
        public NavMeshAgent NavMeshAgent => _navMeshAgent ??= GetComponent<NavMeshAgent>();

        private Animator _animator;
        private NavMeshAgent _navMeshAgent;

        public async void ReclaimAsync()
        {
            await Awaitable.WaitForSecondsAsync(3f);
            Destroy(this.gameObject);
        }
    }
}
