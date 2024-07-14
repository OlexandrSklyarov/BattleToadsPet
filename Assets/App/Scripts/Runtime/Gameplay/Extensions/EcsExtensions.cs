using Leopotam.EcsLite;

namespace BT.Runtime.Gameplay.Extensions
{
    public static class EcsExtensions
    {
        public static ref T GetComponent<T>(this EcsWorld world, int entity) where T : struct
        {
            var pool = world.GetPool<T>();
            return ref pool.Get(entity);
        }
    }
}
