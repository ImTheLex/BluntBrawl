using System.Collections;
using UnityEngine;

namespace Interfaces.Runtime
{
    public interface IDamageable
    {
        
        //public void CmdIncreaseVulnerability(int vulnerabilityAmount);
        
        public void CmdTakeDamage(int damageAmount);
        
        public void HandleDamageableDeath();

        public void IFrame();

    }
}
