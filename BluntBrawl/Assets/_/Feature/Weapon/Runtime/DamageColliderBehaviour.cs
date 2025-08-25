using Interfaces.Runtime;
using UnityEngine;

namespace Weapon.Runtime
{
    public class DamageColliderBehaviour : MonoBehaviour
    {
        
        
        #region Unity API

            
            private void OnTriggerEnter(Collider other)
            {
                if (other.TryGetComponent<IDamageable>(out var damageable))
                {
                    var owner = m_weaponBehaviour.m_owner;

                    if (owner == other.gameObject) return;

                    var amount = m_weaponBehaviour.m_damage * m_weaponBehaviour.m_velocityDamage;
                    damageable.TakeDamage(amount);
                }
            }
        
        #endregion
        
        #region Privates and Protected
        
        
        [SerializeField] private WeaponBehaviour m_weaponBehaviour;
        
        
        #endregion
       
    }
}
