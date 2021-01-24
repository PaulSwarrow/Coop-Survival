using Libs.GameFramework.Systems;
using Mirror;
using UnityEngine;

namespace DefaultNamespace
{
    public class NetObjectSpawnSystem : ObjectSpawnSystem
    {
        public override T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var item = base.Spawn(prefab, position, rotation, parent);
            NetworkServer.Spawn(item.gameObject);
            return item;
        }

        public override void Destroy<T>(T item)
        {
            NetworkServer.UnSpawn(item.gameObject);
            base.Destroy(item);
        }
    }
}