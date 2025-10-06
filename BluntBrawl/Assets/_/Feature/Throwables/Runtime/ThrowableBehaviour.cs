using System;
using Interfaces.Runtime;
using Mirror;
using UnityEngine;

namespace Throwables.Runtime
{
    public class ThrowableBehaviour : NetworkBehaviour,IThrowable
    {

        
        public IThrowable.ThrowableState m_throwableState = IThrowable.ThrowableState.None;
        [HideInInspector] public WeaponStats m_weaponStats => _weaponStats;
        [HideInInspector] public GameObject m_owner;

        private void OnEnable()
        {
            _owner = transform.root.gameObject;
        }

        [Client]
        private void LateUpdate()
        {
            Debug.Log("Parent : " + transform.root.gameObject);
            if (_owner == gameObject)
            {
                _owner = transform.root.gameObject;
                m_owner = _owner;
            }
        }

        private void Update()
        {
            switch (m_throwableState)
            {
                case IThrowable.ThrowableState.None:
                    break;
                case IThrowable.ThrowableState.Launched:
                    ActivateCollider();
                    break;
                case IThrowable.ThrowableState.HasHit:
                    SpawnInWorld();
                    break;
                case IThrowable.ThrowableState.HasDespawn:
                    break;
            }
        }

        public void SetState(IThrowable.ThrowableState state)
        {
            m_throwableState = state;
        }
        
        
        [Server]
        private void SpawnInWorld()
        {
            if (m_weaponStats.m_isRespawnable == false) return;
            GameObject obj = Instantiate(_weaponStats.m_inWorldPrefab, transform.position, transform.rotation);
            NetworkServer.Spawn(obj);
            m_throwableState = IThrowable.ThrowableState.HasDespawn;
            NetworkServer.Destroy(gameObject);
        }

        [Server]
        private void ActivateCollider()
        {
            _damageCollider.enabled = true;
            _damageCollider.isTrigger = true;   
        }
        
        [SerializeField] private WeaponStats _weaponStats;
        [SerializeField] private Collider _damageCollider;
        private GameObject _owner;
    }
}
