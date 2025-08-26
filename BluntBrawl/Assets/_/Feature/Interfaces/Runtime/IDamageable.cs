using System.Collections;
using UnityEngine;

namespace Interfaces.Runtime
{
    public interface IDamageable
    {
        public void TakeDamage(int amount);
        public IEnumerator HandleInvincibilityFrame();
        
    }
}
