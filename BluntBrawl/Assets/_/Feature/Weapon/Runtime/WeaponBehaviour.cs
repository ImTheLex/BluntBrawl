using System;
using Item.Runtime;
using Sounds.Runtime;
using UnityEngine;

namespace Weapon.Runtime

{
    [RequireComponent(typeof(ItemBehaviour))]
    public class WeaponBehaviour : MonoBehaviour
    {
        #region Publics

            //public float m_speedRequired;
            public float m_velocity => _weaponVelocity;
            public float m_requiredVelocity => _weaponData.m_velocityRequired;

            [HideInInspector] public bool m_hasHit;
            public Color m_velocityMatchedColor => _weaponData.m_velocityMatchedColor;
            public Color m_onCooldownColor => _weaponData.m_onCooldownColor;
            public Color m_readyToUseColor => _weaponData.m_readyToUseColor;
            public int m_damage => _weaponData.m_damage;
            
            public float m_invincibilityDuration => _weaponData.m_invincibilityDuration;
            public int m_velocityDamage => _weaponData.m_velocityDamageMultiplier;
            public GameObject m_owner;
            public float m_force => _weaponData.m_force;
            
        #endregion
        
        
        #region Unity API

        private void Awake()
        {
            
            _itemGrabber = GetComponentInParent<ItemGrabber>();
            _debugTimer = m_invincibilityDuration;
            _weaponDamageCollider.gameObject.GetComponent<DamageColliderBehaviour>().GetWeaponSFX(_weaponSFX);
        }

        private void Start()
        {
            if(!_itemGrabber){_localPositionReference = gameObject.transform;}
            else {_localPositionReference = _itemGrabber.transform;}
            if(m_owner is null) m_owner =  transform.root.gameObject;

        }

        private void Update()
        {
            HandleDamageColliderOnVelocity();
            if (m_hasHit)
            {
                _debugTimer -= Time.deltaTime;
            }
            if (_debugTimer <= 0)
            {
                m_hasHit = false;
                _debugTimer = m_invincibilityDuration;
            }
        }
        
        #endregion
        
        
        #region Utils

        private void HandleDamageColliderOnVelocity()
        {
            
            var translation = _localPositionReference.transform.localPosition - _previousPos;
            var velocity = translation.magnitude / Time.deltaTime;
            _weaponVelocity = velocity;
            
            if (velocity > _weaponData.m_velocityRequired)
            
            {
                _weaponSFX.WeaponSlashSFX(100f);
                _weaponDamageCollider.enabled = true;
                _weaponDamageCollider.isTrigger = true;
                
            }
            else
            {
                _weaponDamageCollider.enabled = false;
                _weaponDamageCollider.isTrigger = false;
                
            }
            _previousPos = _localPositionReference.transform.localPosition;
        }

        private void HandleCooldownOnHit()
        {
            
            
        }
       
        #endregion
        
        
        #region Privates & Protected


            [SerializeField] private Transform _localPositionReference;
            [SerializeField] private Collider _weaponDamageCollider;
            [SerializeField] private Rigidbody _weaponRb;
            private float _weaponVelocity;
            private Vector3 _previousPos;
            [SerializeField] private ItemGrabber _itemGrabber;
            
            private float _debugTimer;
            private WeaponStats _weaponData => GetComponent<ItemBehaviour>().m_weaponData;

            private WeaponSFX _weaponSFX => GetComponent<WeaponSFX>();


        #endregion
    }
}
