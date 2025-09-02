using System;
using Mirror;
using UnityEngine;

namespace NetworkConnection.Runtime
{
    public class CustomNetworkManager : NetworkBehaviour
    {
        #region Publics
        
            public static CustomNetworkManager instance { get; private set; }
        
        #endregion
        
        
        #region Unity API
            private void Awake()
            {
                if (instance == null)
                {
                    instance = this;
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }

            private void OnDestroy()
            {
                if (instance == this)
                {
                    instance = null;
                }
                
                
            }
            
        #endregion
        
        
        
        #region Main Methods
        
        [Command]
        public void CmdSpawnItem(GameObject prefab)
        {
            SpawnItem(prefab);
        }
        
        #endregion
        
        #region Utils

        [Server]
        private GameObject InstantiateItem(GameObject prefab, Transform position)
        {
            var obj = Instantiate(prefab, position);
            return obj;
        }

        
            
        
        
        [Server]
        private GameObject InstantiateItem(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var obj = Instantiate(prefab, position, rotation);
            return obj;
        }
        
        
        [Server]
        private GameObject InstantiateItem(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            var obj = Instantiate(prefab, position, rotation, parent);
            return obj;
        }
        
        
        [Server]
        private void SpawnItem(GameObject item)
        {
            NetworkServer.Spawn(item);
        }
        
        
        [Server]
        private void DestroyItem(GameObject item)
        {
            NetworkServer.Destroy(item);
        }
        
        
        #endregion

        
        #region Debug


        [Client]
        public GameObject DebugInstantiateItem(GameObject prefab, Transform position)
        {
            var obj = InstantiateItem(prefab, position);
            return obj;
        }
        
        [Client]
        public GameObject DebugInstantiateItem(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var obj = InstantiateItem(prefab, position, rotation);
            return obj;
        }
        
        [Client]
        public GameObject DebugInstantiateItem(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            var obj = InstantiateItem(prefab, position, rotation, parent);
            return obj;
        }
        
        [Client, ContextMenu("Spawn Item Debug")]
        public void DebugSpawnItem(GameObject item)
        {
            
            SpawnItem(item);
        }
        
        [Client,ContextMenu("Despawn Item Debug")]
        public void DebugDestroyItem(GameObject item)
        {
            
            DestroyItem(item);
        }

        #endregion


        [SerializeField] private GameObject _item;
    }
}
