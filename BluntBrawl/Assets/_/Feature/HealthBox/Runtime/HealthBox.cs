using System;
using Interfaces.Runtime;
using Mirror;
using UnityEngine;

namespace HealthBox.Runtime
{
    public class HealthBox : NetworkBehaviour, IHealProvider
    {

        public int m_healAmount => _healValue;
        private HealthBoxSystem _healthSystem;

        public void Awake()
        {
            _healthSystem = FindFirstObjectByType<HealthBoxSystem>();
        }
        

        [ContextMenu("Decrease Health"), Server]
        public void DestroyProvider()
        {
            _healthSystem.DecreaseCurrentHealthBoxes(gameObject);
            //NetworkServer.Destroy(gameObject);
        }

        [Command]
        public void CmdDestroyProvider()
        {
            DestroyProvider();
        }
        [SerializeField] private int _healValue;

        [Server]
        public void ServerDestroyProvider()
        {
            DestroyProvider();
        }

    }
}
