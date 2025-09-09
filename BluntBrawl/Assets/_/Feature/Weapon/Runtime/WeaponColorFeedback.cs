using System;
using UnityEngine;

namespace Weapon.Runtime
{
    public class WeaponColorFeedback : MonoBehaviour
    {
        private WeaponBehaviour _weaponBehaviour => GetComponentInParent<WeaponBehaviour>();
        
        [SerializeField] private Renderer _lightRenderer;
        private void Update()
        {
            
            if (_weaponBehaviour.m_velocity >= _weaponBehaviour.m_requiredVelocity)
            {
                _lightRenderer.material.color = _weaponBehaviour.m_velocityMatchedColor;
            }
            else if (_weaponBehaviour.m_hasHit == true)
            {
                _lightRenderer.material.color = _weaponBehaviour.m_onCooldownColor;
            }
            else
            {
                _lightRenderer.material.color = _weaponBehaviour.m_readyToUseColor;
            }
        }
    }
}
