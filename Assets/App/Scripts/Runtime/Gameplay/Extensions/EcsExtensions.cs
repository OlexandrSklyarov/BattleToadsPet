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

        public static ref T TryAddComponent<T>(this EcsWorld world, int entity) where T : struct
        {
            var pool = world.GetPool<T>();            
            if (!pool.Has(entity)) pool.Add(entity);
            return ref pool.Get(entity);
        }

        public static ref TAdd TryReplaceComponent<TRemove, TAdd>(this EcsWorld world, int entity) 
            where TRemove : struct
            where TAdd: struct
        {
            var poolRemove = world.GetPool<TRemove>();
            var poolAdd = world.GetPool<TAdd>();

            if (poolRemove.Has(entity)) poolRemove.Del(entity);
            if (!poolAdd.Has(entity)) poolAdd.Add(entity);

            return ref poolAdd.Get(entity);
        }
    }
}
