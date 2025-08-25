using UnityEngine;


[CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Weapons/Weapon Stats")]
public class WeaponStats : ScriptableObject
{
    public int m_damage;
    public int m_velocityDamageMultiplier;
}
