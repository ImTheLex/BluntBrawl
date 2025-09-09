using Interfaces.Runtime;
using Mirror;
using Player.Runtime;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace Weapon.Runtime
{
    public class DamageColliderBehaviour : NetworkBehaviour
    {
        
        
        #region Unity API

            
            private void OnTriggerEnter(Collider other)
            {
                if (other.TryGetComponent<IDamageable>(out var damageable))
                {
                    var owner = m_weaponBehaviour.m_owner;

                    if (owner == other.gameObject.transform.root.gameObject) return;

                    
                    var amount = m_weaponBehaviour.m_damage * m_weaponBehaviour.m_velocityDamage;
                    //damageable.CmdTakeDamage(amount);
                    if (!damageable.m_isInvincible)
                    {
                        damageable.m_invincibilityDuration = m_weaponBehaviour.m_invincibilityDuration;
                    }
                    damageable.CmdTakeDamage(amount);
                    
                }

                if (other.TryGetComponent<IBumpable>(out var bumpable))
                {
                    XROrigin xrOrigin = m_weaponBehaviour.m_owner.GetComponent<PlayerMovement>().m_XROrigin;
                    NetworkIdentity identity = other.transform.root.GetComponent<NetworkIdentity>();
                    bumpable.TargetPlayerBumpOnHit(identity.connectionToClient, xrOrigin.transform.position, m_weaponBehaviour.m_force);
                    
                }
            }
        
        #endregion
        
        #region Privates and Protected
        
        
        [SerializeField] private WeaponBehaviour m_weaponBehaviour;
        
        
        #endregion
       
    }
}
