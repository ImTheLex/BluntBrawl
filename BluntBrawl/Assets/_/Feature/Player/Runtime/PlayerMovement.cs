using InputSystem.BluntBrawl;
using Mirror;
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
        }

        private void OnEnable() => _playerInputActions.Enable();
        
        private void OnDisable() => _playerInputActions.Disable();

        private void Update()
        {
            if (isLocalPlayer) MoveMediator();
        }
        

        #endregion
        
        #region Input Actions
            public void OnMove(InputAction.CallbackContext context)
            {
                _playerInputMovement = context.ReadValue<Vector2>();
            }
    
            public void OnLook(InputAction.CallbackContext context)
            {
                
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
      
        
        private void MoveMediator()
        {
            Vector3 cameraDirection = new Vector3();
            cameraDirection.x = _playerInputMovement.x;
            cameraDirection.z = _playerInputMovement.y;
            _playerOrigin.position += cameraDirection * (Time.deltaTime * _moveSpeed);
        }

        #endregion
        
        #region Private and Protected

        
        private BluntBrawlInputActions _playerInputActions;
        
        [SerializeField] private float _moveSpeed;
        private Vector2 _playerInputMovement;
        [SerializeField] private Transform _playerOrigin;



        #endregion
    }
}
