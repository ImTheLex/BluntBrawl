using System;
using Interfaces.Runtime;
using UnityEngine;

namespace Weapon.Runtime
{
    public class DamageColliderBehaviour : MonoBehaviour
    {
        #region Publics

        
            public WeaponBehaviour m_weaponBehaviour;
            
            
        #endregion
        
        
        #region Unity API

            private void Awake()
            {
                _weaponStats = m_weaponBehaviour.m_weaponStats;
            }

            private void OnTriggerEnter(Collider other)
            {
                if (other.TryGetComponent<IDamageable>(out var damageable))
                {
                    var owner = m_weaponBehaviour.m_owner;

                    if (owner == other.gameObject) return;
                    
                    var amount = _weaponStats.m_damage *_weaponStats.m_velocityDamageMultiplier;
                    damageable.TakeDamage(amount);
                }
            }
        
        #endregion

        #region Privates

            private WeaponStats _weaponStats;

        #endregion
    }
}
