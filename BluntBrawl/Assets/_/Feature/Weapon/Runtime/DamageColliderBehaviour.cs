using Interfaces.Runtime;
using Mirror;
using Player.Runtime;
using Sounds.Runtime;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;

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
                    
                    //si owner => return
                    if (owner == other.gameObject.transform.root.gameObject) return;
                    //si cd => return
                    if (m_weaponBehaviour.m_hasHit) return;

                    _weaponSFX.WeaponHitSFX(netIdentity.connectionToClient);

                    InputDeviceCharacteristics hand = InputDeviceCharacteristics.Right;
                    NetworkIdentity identity = transform.root.gameObject.GetComponent<NetworkIdentity>();
                    _ownPlayerMovement.TargetRPCHapticToController(identity.connectionToClient, hand,0.75f,.5f);
                    
                    var amount = m_weaponBehaviour.m_damage * m_weaponBehaviour.m_velocityDamage;
                    //damageable.CmdTakeDamage(amount);
                    if (!damageable.m_isInvincible)
                    {
                        damageable.m_invincibilityDuration = m_weaponBehaviour.m_invincibilityDuration;
                    }
                    damageable.CmdTakeDamage(amount);
                    m_weaponBehaviour.m_hasHit = true;
                }

                if (other.TryGetComponent<IHealProvider>(out var healProvider))
                {
                    var owner = m_weaponBehaviour.m_owner;
                    var healable = owner.GetComponentInChildren<IHealable>();
                    healable.CmdHeal(healProvider.m_healAmount);
                    healProvider.DestroyProvider();
                }
                
                if (other.TryGetComponent<IBumpable>(out var bumpable))
                {
                    XROrigin xrOrigin = m_weaponBehaviour.m_owner.GetComponent<PlayerMovement>().m_XROrigin;
                    PlayerMovement playerMovement = other.transform.root.GetComponent<PlayerMovement>();
                    playerMovement.TargetAnimatorHit();
                    NetworkIdentity identity = other.transform.root.GetComponent<NetworkIdentity>();
                    bumpable.TargetPlayerBumpOnHit(identity.connectionToClient, xrOrigin.transform.position, m_weaponBehaviour.m_force);
                }
            }

        #endregion

        #region Main Method

        public void GetWeaponSFX(WeaponSFX weaponSFX) => _weaponSFX = weaponSFX;

        #endregion

        #region Privates and Protected


        [SerializeField] private WeaponBehaviour m_weaponBehaviour;
        private PlayerMovement _ownPlayerMovement => m_weaponBehaviour.m_owner.GetComponent<PlayerMovement>();

        private WeaponSFX _weaponSFX;
        

        #endregion

    }
}
