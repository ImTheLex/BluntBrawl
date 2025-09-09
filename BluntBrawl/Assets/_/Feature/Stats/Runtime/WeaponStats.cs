using UnityEngine;


[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Weapons/Weapon Stats")]
public class WeaponStats : ScriptableObject
{
    public int m_damage;
    public float m_velocityRequired;
    public int m_velocityDamageMultiplier;
    public float m_invincibilityDuration;
    public float m_force;

	public Color m_velocityMatchedColor;
	public Color m_onCooldownColor;
	public Color m_readyToUseColor;

    public GameObject m_inWorldPrefab;
    public GameObject m_inHandPrefab;
}
