
namespace Interfaces.Runtime
{
    public interface IDamageable
    {
        
        //public void CmdIncreaseVulnerability(int vulnerabilityAmount);
        public float m_invincibilityDuration { get; set; }
        public bool m_isInvincible { get; }
        public void CmdTakeDamage(int damageAmount);
        
        public void HandleDamageableDeath();

        public void IFrame();

    }
}
