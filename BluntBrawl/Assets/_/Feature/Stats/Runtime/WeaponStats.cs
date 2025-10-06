using UnityEngine;


[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Weapons/Weapon Stats")]
public class WeaponStats : ScriptableObject
{
    public int m_damage;
    public float m_velocityRequired;
    public int m_velocityDamageMultiplier;
    public float m_invincibilityDuration;
    public float m_force;
    public float m_verticalForce;
    public float m_horizontalForce;

    public bool m_isThrowable;
    public bool m_isRespawnable;
    
	public GameObject m_inWorldPrefab;
    public GameObject m_inHandPrefab;
}
