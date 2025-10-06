using System;
using Health.Runtime;
using Interfaces.Runtime;
using UnityEngine;

namespace Throwables.Runtime
{
    public class ThrowableColliderBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            _weaponStats = _throwableBehaviour.m_weaponStats;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_throwableBehaviour.m_owner == other.transform.parent.root.gameObject) return;
            if (_throwableBehaviour.m_owner == _throwableBehaviour.gameObject) return;
            Debug.Log($"Throwable Owner : {_throwableBehaviour.m_owner} / Other : {other.transform.root.gameObject}");
            if (other.TryGetComponent<HealthBehaviour>(out var healthBehaviour))
            {
                healthBehaviour.CmdTakeDamage(_weaponStats.m_damage);
                _throwableBehaviour.SetState(IThrowable.ThrowableState.HasHit);
                return;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _throwableBehaviour.SetState(IThrowable.ThrowableState.HasHit);
                return;
            }
        }
        
        
        [SerializeField] private ThrowableBehaviour _throwableBehaviour;
        private WeaponStats _weaponStats;

    }
}
