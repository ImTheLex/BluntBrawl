using Mirror;
using UnityEngine;

namespace Player.Runtime
{
    public class SpawnPrefabCube : NetworkBehaviour
    {
        [SerializeField] private GameObject _prefab;
        
        
        [ClientRpc]
        public void RpcSpawn(Vector3 position)
        {
           GameObject obj = Instantiate(_prefab,position, Quaternion.identity);
        }

        [Command]
        public void CmdSpawn(Vector3 position)
        {
            RpcSpawn(position);
            NetworkServer.Spawn(gameObject);
        }
    }
}
