using System;
using Mirror;
using UnityEngine;

namespace Weapon.Runtime
{
    public class WeaponColorFeedback : NetworkBehaviour
    {
        private WeaponBehaviour _weaponBehaviour => GetComponentInParent<WeaponBehaviour>();
        
        [SerializeField] private Renderer _lightRenderer;
        private MaterialPropertyBlock _materialPropertyBlock;

        private void Awake()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
        }
        
        private void Update()
        {
            
            if (_weaponBehaviour.m_velocity >= _weaponBehaviour.m_requiredVelocity)
            {
                //_lightRenderer.material.color = _weaponBehaviour.m_velocityMatchedColor;
                _materialPropertyBlock.SetColor("_BaseColor", _weaponBehaviour.m_velocityMatchedColor);
                _lightRenderer.SetPropertyBlock(_materialPropertyBlock);
                
            }
            else if (_weaponBehaviour.m_hasHit == true)
            {
               // _lightRenderer.material.color = _weaponBehaviour.m_onCooldownColor;
               _materialPropertyBlock.SetColor("_BaseColor", _weaponBehaviour.m_onCooldownColor);
               _lightRenderer.SetPropertyBlock(_materialPropertyBlock);
            }
            else
            {
                //_lightRenderer.material.color = _weaponBehaviour.m_readyToUseColor;
                _materialPropertyBlock.SetColor("_BaseColor", _weaponBehaviour.m_readyToUseColor);
                _lightRenderer.SetPropertyBlock(_materialPropertyBlock);
                
            }
        }


        [ContextMenu("Debug Cooldowns")]
        public void RpcDebugCooldownColor()
        {
            _materialPropertyBlock.SetColor("_BaseColor", _weaponBehaviour.m_onCooldownColor);
            _lightRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        [ContextMenu("Debug Cooldowns")]
        
        public void RpcDebugReadyColor()
        {
            _materialPropertyBlock.SetColor("_BaseColor", _weaponBehaviour.m_velocityMatchedColor);
            _lightRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        [ContextMenu("Debug Has Hit")]
        public void RpcDebugHasHit()
        {
            Debug.Log("Has Hit : " + _weaponBehaviour.m_hasHit);
            _weaponBehaviour.m_hasHit = true;
            Debug.Log("Has Hit : " + _weaponBehaviour.m_hasHit);
        }
    }
}
