using Health.Runtime;
using Interfaces.Runtime;
using Mirror;
using Player.Runtime;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace Colision.Runtime
{
    public class PlayerColisionFeedback : NetworkBehaviour, IBumpable
    {
        
        #region UnityAPI

        private void Update()
        {
            if (!_isBumping) return;
            
            _bumpChrono -= Time.deltaTime;
            _playerRigidbody.AddForce(_bumpDirection * _forceWeapon, ForceMode.Impulse);
            if (_bumpChrono <= 0)
            {
                _isBumping = false;
                _playerMovement.m_isBumping = false;
            }
        }

        #endregion
        
        #region Utils
        
        [Server]
        public void PlayerBumpOnHit(Vector3 hitPosition, float force)
        {
           if (_isBumping) return;
           
            Vector3 direction = ( _playerRigidbody.position - hitPosition).normalized;
           
            float currentDamage = _playerHealth.m_maxHealth - _playerHealth.m_currentHealth;
           currentDamage = currentDamage <= 0f ? 1f : currentDamage;
           direction.y = 1 * _vecticalForcePerDamage * currentDamage;
           direction.x *= _horizontalForcePerDamage * force * currentDamage;
           direction.z *= _horizontalForcePerDamage * force * currentDamage;
           
           _bumpDirection = direction;
           _bumpChrono = _bumpTimer;
           _forceWeapon = force;
           _isBumping = true;
           _playerMovement.m_isBumping = true;
        }
        
        
        #endregion


        #region Privates

        
        private HealthBehaviour _playerHealth;
        private PlayerMovement _playerMovement => GetComponentInParent<PlayerMovement>();
        private XROrigin _xROrigin => _playerMovement.m_XROrigin;
        private Rigidbody _playerRigidbody => _xROrigin.GetComponent<Rigidbody>();
        [SerializeField] private float _vecticalForcePerDamage;
        [SerializeField] private float _horizontalForcePerDamage;
        [SerializeField] private float _baseForce;
        [SerializeField] private float _bumpTimer;
        private float _bumpChrono;
        

        private Vector3 _bumpDirection;
        private float _forceWeapon;
        private bool _isBumping;
        


        #endregion

        
    }
}
