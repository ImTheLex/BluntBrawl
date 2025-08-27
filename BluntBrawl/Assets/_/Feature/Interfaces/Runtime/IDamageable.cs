using System.Collections;
using UnityEngine;

namespace Interfaces.Runtime
{
    public interface IDamageable
    {
        public void TakeDamage(int amount);
        public void CmdTakeDamage(int amount);
        
        public void CmdIncreaseVulnerability(int vulnerabilityAmount);
        
        public void HandleDamageableDeath();
        public IEnumerator HandleInvincibilityFrame();
        
    }
}
