using System;
using InputSystem.BluntBrawl;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Runtime
{
    public class PlayerMovement : MonoBehaviour, BluntBrawlInputActions.IPlayerActions
    {
        
        #region Unity API

        private void Awake()
        {
            _playerInputActions = new BluntBrawlInputActions();
            _playerInputActions.Player.SetCallbacks(this);
            // _characterController.GetComponent<CharacterController>();
            //_cameraRig = FindFirstObjectByType<OVRCameraRig>();
        }

        private void OnEnable() => _playerInputActions.Enable();
        
        private void OnDisable() => _playerInputActions.Disable();

        private void Update()
        {
            //MoveCameraRIG();
            //MovePlayer();
            //MoveDebugPlayer();
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
      
        
        private void MoveDebugPlayer()
        {
            Vector3 cameraDirection = new Vector3();
            cameraDirection.x = _playerInputMovement.x;
            cameraDirection.z = _playerInputMovement.y;
            // _characterController.Move(cameraDirection * (Time.deltaTime * _moveSpeed));
        }

        #endregion
        
        #region Private and Protected

        
        private BluntBrawlInputActions _playerInputActions;
        
        [SerializeField] private float _moveSpeed;
        private Vector2 _playerInputMovement;

        // private Oculus.Interaction.Locomotion.CharacterController _characterController;

        #endregion
    }
}
