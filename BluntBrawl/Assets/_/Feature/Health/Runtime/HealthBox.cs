using Interfaces.Runtime;
using Mirror;
using UnityEngine;

namespace Health.Runtime
{
    public class HealthBox : NetworkBehaviour, IHealProvider
    {

        public int m_healAmount => _healValue;
       

        public void DestroyProvider()
        {
            NetworkServer.Destroy(gameObject);
        }

        [SerializeField] private int _healValue;

        [Server]
        public void ServerDestroyProvider()
        {
            DestroyProvider();
        }

    }
}
