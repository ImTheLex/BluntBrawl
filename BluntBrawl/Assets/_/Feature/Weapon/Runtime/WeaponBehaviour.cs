using Item.Runtime;
using UnityEngine;

namespace Weapon.Runtime

{
    [RequireComponent(typeof(ItemBehaviour))]
    public class WeaponBehaviour : MonoBehaviour
    {
        #region Publics

            public float m_speedRequired;
            public float m_velocity => _weaponVelocity;
            public int m_damage => _weaponData.m_damage;
            public int m_velocityDamage => _weaponData.m_velocityDamageMultiplier;
            public GameObject m_owner;
            
        #endregion
        
        
        #region Unity API

        private void Start()
        {
            ItemGrabber item = GetComponentInParent<ItemGrabber>();
            item.EquipStartingWeapon(gameObject, _weaponData);
        }

        private void Update()
        {
            HandleDamageColliderOnVelocity();
        }
        
        #endregion
        
        
        #region Utils

        private void HandleDamageColliderOnVelocity()
        {
            
            var translation = _localPositionReference.transform.localPosition - _previousPos;
            var velocity = translation.magnitude / Time.deltaTime;
            _weaponVelocity = velocity;
            //var velocity = Vector3.Magnitude(_weaponRb.linearVelocity);
            if (velocity > m_speedRequired)
            {
                Debug.Log("Can damage because Velocity is : " + velocity.ToString("F2") + " And required is : " + m_speedRequired);
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

       
        #endregion
        
        
        #region Privates & Protected


            [SerializeField] private Transform _localPositionReference;
            [SerializeField] private Collider _weaponDamageCollider;
            [SerializeField] private Rigidbody _weaponRb;
            private float _weaponVelocity;
            private Vector3 _previousPos;
        
            private WeaponStats _weaponData => GetComponent<WeaponStats>();


            #endregion
    }
}
