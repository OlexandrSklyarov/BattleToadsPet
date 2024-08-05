using System;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Services.Attack
{
    [Serializable]
    public class AttackPoint
    {
        public AttackPointType Type;
        public Transform Point;
    }
}
