using System;
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
                damageable.TakeDamage();
            }
        }
        #endregion
    }
}
