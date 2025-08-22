using System;
using UnityEngine;

namespace Weapon.Runtime
{
    public class WeaponBehaviour : MonoBehaviour
    {
        #region Publics

            public float m_speedRequired;
            public float m_velocity => _weaponVelocity;
            
        #endregion
        
        
        #region Unity API
        private void Update()
        {
            HandleDamageColliderOnVelocity();
        }
        
        #endregion
        
        
        #region Utils

        private void HandleDamageColliderOnVelocity()
        {
            
            var translation = transform.position - _previousPos;
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
            _previousPos = transform.position;
        }

       
        #endregion
        
        
        #region Privates & Protected

        
            [SerializeField] private Collider _weaponDamageCollider;
            [SerializeField] private Rigidbody _weaponRb;
            private float _weaponVelocity;
            private Vector3 _previousPos;


            #endregion
    }
}
