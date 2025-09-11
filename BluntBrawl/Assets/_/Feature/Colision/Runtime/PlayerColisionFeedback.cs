using Animation.Runtime;
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
            if (!isLocalPlayer) return;
            if (!_isBumping) return;
            
            _bumpChrono -= Time.deltaTime;
            _playerRigidbody.AddForce(_bumpDirection * _forceWeapon, ForceMode.Impulse);
            
            if (_bumpChrono <= 0)
            {
                _isBumping = false;
                _playerMovement.m_isBumping = false;
                _faceOffset.ChangeFace("normal");
            }
        }

        #endregion
        
        #region Publics Methods
        
        
        public void PlayerBumpOnHit(Vector3 hitPosition, float force)
        {
           if (_isBumping) return;
           
            Vector3 direction = ( _playerRigidbody.position - hitPosition).normalized;
           
            float currentDamage = _playerHealth.m_maxHealth - _playerHealth.m_currentHealth;
           currentDamage = currentDamage <= 0f ? 1f : currentDamage;
           direction.y = 1 * _vecticalForcePerDamage * currentDamage;
           direction.x *= _horizontalForcePerDamage * force * currentDamage;
           direction.z *= _horizontalForcePerDamage * force * currentDamage;
           
           _bumpDirection = direction.normalized;
           _bumpChrono = _bumpTimer;
           _forceWeapon = force;
           _isBumping = true;
           _playerMovement.m_isBumping = true;
           _faceOffset.ChangeFace("hurt" + Random.Range(1,3));
           }
        
        [TargetRpc]
        public void TargetPlayerBumpOnHit(NetworkConnectionToClient target, Vector3 direction, float force)
        {
            PlayerBumpOnHit(direction, force);
        }

        // [ContextMenu("ChangeFace")]
        // public void ChangeFaceDebug()
        // {
        //     if (!_isFacingRight)
        //     {
        //         _faceOffset.ChangeFace("hurt"+ Random.Range(1, 3));
        //         _isFacingRight = true;
        //     }
        //     else
        //     {
        //         _faceOffset.ChangeFace("normal");
        //         _isFacingRight = false;
        //     }
        // }
        #endregion
        
        #region Utils


        
        
        #endregion


        #region Privates

        
        private HealthBehaviour _playerHealth => GetComponent<HealthBehaviour>();
        private PlayerMovement _playerMovement => GetComponentInParent<PlayerMovement>();
        private XROrigin _xROrigin => _playerMovement.m_XROrigin;
        private Rigidbody _playerRigidbody => _xROrigin.GetComponent<Rigidbody>();
        [SerializeField] private float _vecticalForcePerDamage;
        [SerializeField] private float _horizontalForcePerDamage;
        [SerializeField] private float _baseForce;
        [SerializeField] private float _bumpTimer;
        private float _bumpChrono;
        

        private Vector3 _bumpDirection = new Vector3(0f, 0f, 0f);
        private float _forceWeapon;
        private bool _isBumping;

        [SerializeField]private FaceOffset _faceOffset;
        
        private bool _isFacingRight = false;



        #endregion


    }
}
