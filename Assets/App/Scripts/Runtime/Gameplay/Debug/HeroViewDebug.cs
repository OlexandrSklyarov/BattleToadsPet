using BT.Runtime.Data.Configs;
using UnityEngine;

namespace BT
{
    public class HeroViewDebug : MonoBehaviour
    {
        [SerializeField] private CharacterConfig _config;

        private void OnDrawGizmos() 
        {
            if (!_config.IsShowDebug) return;

            //draw check ground
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.TransformPoint(_config.Gravity.CheckGroundOffset), _config.Gravity.CheckGroundSphereRadius);
        }
    }
}
