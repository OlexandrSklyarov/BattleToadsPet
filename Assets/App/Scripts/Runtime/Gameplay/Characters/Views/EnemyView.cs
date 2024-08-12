using UnityEngine;

namespace BT.Runtime.Gameplay.View
{
    public sealed class EnemyView : MonoBehaviour
    {
        [field: SerializeField] public Collider Collider { get; private set; }
    }
}
