using System;
using Interfaces.Runtime;
using Mirror;
using UnityEngine;

namespace MisteryBox.Runtime
{
    public class MysteryBox : NetworkBehaviour,IMysteryBox
    {
        public int m_currentHealth => _currentHealth;
        private GameObject _mysteryBoxData;
        private MisteryBoxSystem _misteryBoxSystem;

        private void Awake()
        {
            _misteryBoxSystem = FindFirstObjectByType<MisteryBoxSystem>();
        }

        public void TakeDamage(int amount)
        {
            _currentHealth -= amount;
            if (_currentHealth <= 0)
            {
                CmdDropItem();
            }
        }

        [Server]
        public void SetData(GameObject data)
        {
            _mysteryBoxData = data;
        }
        
        [Command(requiresAuthority = false)]
        public void CmdTakeDamage()
        {
            TakeDamage(1);
        }

        [Server]
        public void DropItem()
        {
            var mystery = Instantiate(_mysteryBoxData,transform.position,transform.rotation);
            NetworkServer.Spawn(mystery);
            _misteryBoxSystem.AddToSpawnedWeapons(mystery);
            NetworkServer.Destroy(gameObject);
        }
    
        [ContextMenu("Drop Item Command"),Command(requiresAuthority = false)]
        public void CmdDropItem()
        {
            DropItem();
        }
    
        [SyncVar] private int _currentHealth = 1;
    
    }
}
