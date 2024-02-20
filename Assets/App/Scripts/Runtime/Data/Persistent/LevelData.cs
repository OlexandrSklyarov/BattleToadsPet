using System;

namespace BT.Runtime.Data.Persistent
{
    [Serializable]
    public sealed class LevelData : IData
    {
        public int NextLevelIndex;
    }
}