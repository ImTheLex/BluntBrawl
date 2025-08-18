using InputSystem.BluntBrawl;
using Mirror;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Runtime
{
    public class PlayerMovement : NetworkBehaviour, BluntBrawlInputActions.IPlayerActions
    {
        
        #region Unity API

        private void Awake()
        {
            _playerInputActions = new BluntBrawlInputActions();
            _playerInputActions.Player.SetCallbacks(this);
            _playerHead = _playerOrigin.GetComponent<XROrigin>().Camera.transform;
        }

        private void OnEnable() => _playerInputActions.Enable();
        
        private void OnDisable() => _playerInputActions.Disable();

        private void Update()
        {
            if (isLocalPlayer)
            {
                Move();
                //MoveLook();
            }
        }
        

        #endregion


        #region Input action

        

        
            public void OnMove(InputAction.CallbackContext context)
            {
                _playerInputMovement = context.ReadValue<Vector2>();
            }
    
            public void OnLook(InputAction.CallbackContext context)
            {
                _playerInputView = context.ReadValue<Vector2>();
            }
    
            public void OnAttack(InputAction.CallbackContext context)
            {
                
            }
    
            public void OnInteract(InputAction.CallbackContext context)
            {
                
            }
    
            public void OnCrouch(InputAction.CallbackContext context)
            {
                
            }
    
            public void OnJump(InputAction.CallbackContext context)
            {
                
            }
    
            public void OnPrevious(InputAction.CallbackContext context)
            {
                
            }
    
            public void OnNext(InputAction.CallbackContext context)
            {
               
            }
    
            public void OnSprint(InputAction.CallbackContext context)
            {
               
            }
        
        #endregion
        

        #region Utils
      
        
        private void Move()
        {
            Vector3 inputDirection = _playerHead.forward * _playerInputMovement.y + _playerHead.right * _playerInputMovement.x;
            inputDirection.y = 0;
            _playerOrigin.position += inputDirection * (Time.deltaTime * _moveSpeed);
        }

        private void MoveLook()
        {
            float rotateDirection = _playerInputView.y * Time.deltaTime * _rotateSpeed;
            _playerOrigin.Rotate(0, rotateDirection, 0);
        }

        #endregion
        
        
        #region Private and Protected

        
        private BluntBrawlInputActions _playerInputActions;
        
        [SerializeField, Tooltip("Meter per second")] private float _moveSpeed;

        [SerializeField, Tooltip("Degre per second")] private float _rotateSpeed;
        [SerializeField, Tooltip("XROrigin of this player")] private Transform _playerOrigin;
        private Transform _playerHead;
        
        private Vector2 _playerInputMovement;
        private Vector2 _playerInputView;

        #endregion

        
    }
}
