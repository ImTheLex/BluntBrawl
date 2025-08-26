using InputSystem.BluntBrawl;
using Interfaces.Runtime;
using Item.Runtime;
using Mirror;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Runtime
{
    public class PlayerMovement : NetworkBehaviour, BluntBrawlInputActions.IPlayerActions, BluntBrawlInputActions.IBBXRILeftActions, BluntBrawlInputActions.IBBXRIRightActions,BluntBrawlInputActions.IBBXRIRightInteractionActions
    {
        
        #region Publics
            //public BluntBrawlInputActions m_playerInputActions => _playerInputActions;
            
        #endregion
        
        
        #region Unity API

        private void Awake()
        {
            _playerInputActions = new BluntBrawlInputActions();
            _playerInputActions.Player.SetCallbacks(this);
            _playerInputActions.BBXRILeft.SetCallbacks(this);
            _playerInputActions.BBXRIRight.SetCallbacks(this);
            _playerInputActions.BBXRIRightInteraction.SetCallbacks(this);

            _XROrigin = _playerOrigin.GetComponent<XROrigin>();
            _playerHead = _XROrigin.Camera.transform;
            _playerRigidbody = _XROrigin.GetComponent<Rigidbody>();
            _playerRigidbody.maxLinearVelocity = 20f;

            _itemGrabber = _rightController.GetComponent<ItemGrabber>();

        }

        private void OnEnable() => _playerInputActions.Enable();
        
        private void OnDisable() => _playerInputActions.Disable();

        private void Update()
        {
            if (isLocalPlayer)
            {
                Move();
                TrackingPositionController();
                TrackingRotationController();
            }
        }

        
        #endregion


        #region Input action

        

            //main player
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
                if (isLocalPlayer) _isSprinting = context.performed;
            }
            
            //Left
            public void OnPositionLeft(InputAction.CallbackContext context)
            {
                _leftControllerInputPosition = context.ReadValue<Vector3>();
            }

            public void OnRotationLeft(InputAction.CallbackContext context)
            {
                _leftControllerInputRotation = context.ReadValue<Quaternion>();
            }
            
            //Right
            public void OnPositionRight(InputAction.CallbackContext context)
            {
                _rightControllerInputPosition = context.ReadValue<Vector3>();
            }

            public void OnRotationRight(InputAction.CallbackContext context)
            {
                _rightControllerInputRotation = context.ReadValue<Quaternion>();
            }
            
            //Right interaction
            public void OnSelect(InputAction.CallbackContext context)
            {
                if (!isLocalPlayer) return;
                if (context.performed)
                {
                    _itemGrabber.GrabItem();
                }
                
            }

            public void OnSelectValue(InputAction.CallbackContext context)
            {
                
            }

            public void OnActivate(InputAction.CallbackContext context)
            {
                
            }

            public void OnActivateValue(InputAction.CallbackContext context)
            {
                
            }

            public void OnUIPress(InputAction.CallbackContext context)
            {
                
            }

            public void OnUIPressValue(InputAction.CallbackContext context)
            {
                
            }

            public void OnUIScroll(InputAction.CallbackContext context)
            {
                
            }

            public void OnTranslateManipulation(InputAction.CallbackContext context)
            {
                
            }

            public void OnRotateManipulation(InputAction.CallbackContext context)
            {
                
            }

            public void OnDirectionalManipulation(InputAction.CallbackContext context)
            {
                
            }

            public void OnScaleToggle(InputAction.CallbackContext context)
            {
                
            }

            public void OnScaleOverTime(InputAction.CallbackContext context)
            {
                
            }
            
            #endregion
            
        
    
        #region Utils
    
        
        private void Move()
        {
            Vector3 inputDirection = _playerHead.forward * _playerInputMovement.y + _playerHead.right * _playerInputMovement.x;
            inputDirection.y = 0;
            
            if (_isSprinting) _playerRigidbody.linearVelocity = inputDirection * (_moveSpeed * (_sprintMultiplier > 1f ? _sprintMultiplier:1f));
            else _playerRigidbody.linearVelocity = inputDirection * _moveSpeed;
            if (_playerInputMovement.magnitude <= 0f) _playerRigidbody.linearVelocity = Physics.gravity * _playerRigidbody.mass;
            
            
        }
       
        
        private void TrackingPositionController()
        {
            _leftController.localPosition = _leftControllerInputPosition;
            _rightController.localPosition = _rightControllerInputPosition;
        }
        
        private void TrackingRotationController()
        {
            _leftController.rotation = _leftControllerInputRotation;
            _rightController.rotation = _rightControllerInputRotation;
        }

        #endregion
        
        
        #region Private and Protected

        
        private BluntBrawlInputActions _playerInputActions;
        
        [Header("Settings for Movement")]
        [SerializeField, Tooltip("XROrigin of this player")] private Transform _playerOrigin;
        [SerializeField, Tooltip("Meter per second")] private float _moveSpeed;
        private bool _isSprinting;
        [SerializeField] private float _sprintMultiplier;

        [Header("Settings for Tracked Controller")] 
        [SerializeField] private Transform _leftController;
        [SerializeField] private Transform _rightController;
        
        private Transform _playerHead;
        private XROrigin _XROrigin;
        private Rigidbody _playerRigidbody;
        
        
        private Vector2 _playerInputMovement;

        private Vector3 _leftControllerInputPosition;
        private Vector3 _rightControllerInputPosition;
        
        private Quaternion _leftControllerInputRotation;
        private Quaternion _rightControllerInputRotation;
        
        [SerializeField] private ItemGrabber _itemGrabber;


        #endregion

        
    }
}
