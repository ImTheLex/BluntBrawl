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
        
            private void OnTriggerEnter(Collider other)
            {
                if (other.TryGetComponent<IDamageable>(out var damageable))
                {
                    var owner = m_weaponBehaviour.m_owner;

                    if (owner == other.gameObject) return;
                    
                    var amount = m_weaponBehaviour.m_weaponStats.m_damage;
                    damageable.TakeDamage(amount);
                }
            }
        
        #endregion
    }
}
