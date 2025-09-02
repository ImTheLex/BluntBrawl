using InputSystem.BluntBrawl;
using Item.Runtime;
using Mirror;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Runtime
{
    public class PlayerMovement : NetworkBehaviour, BluntBrawlInputActions.IPlayerActions,
        BluntBrawlInputActions.IBBXRILeftActions, BluntBrawlInputActions.IBBXRIRightActions,
        BluntBrawlInputActions.IBBXRIRightInteractionActions
    {


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

        private void Start()
        {
            _XROrigin.MoveCameraToWorldLocation(_XROrigin.transform.position);
        }

        private void OnDisable() => _playerInputActions.Disable();

        private void Update()
        {
            if (isLocalPlayer)
            {
                if (_doubleTapChrono >= 0f) _doubleTapChrono -= Time.deltaTime;
                if (_isDashing)
                {
                    _dashChrono += Time.deltaTime;
                    Dash();
                    if (_dashChrono > _dashDuration)
                    {
                        _dashChrono = 0;
                        _isDashing = false;
                    }
                }
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
            if (!isLocalPlayer) return;
            _playerInputMovement = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (!isLocalPlayer) return;
            if (context.started)
            {
                Vector3 direction = _playerHead.transform.forward + _playerRigidbody.position;
                direction.y += 1f;
                if (isServer)
                {
                    _spawnPrefabCube.RpcSpawn(direction);
                }
                else
                {
                    _spawnPrefabCube.CmdSpawn(direction);
                }
            }
        }

        public void OnDebugPosition(InputAction.CallbackContext context)
        {
            if (!isLocalPlayer) return;
            if (context.started) _playerRigidbody.position = new Vector3(0, 5, 0);
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (!isLocalPlayer) return;
            if (!_isDashing && context.started)
            {
                _isDashing = true;
                _dashDirection = _playerInputMovement.normalized;
            }
        }
        
        
        /*Dash method on double tap joystick
         
         public void OnDash(InputAction.CallbackContext context)
        {
            if (_doubleTapChrono <= 0f && context.started)
            {
                _doubleTapChrono = _dashTimeWindow;
                _previousDirection = context.ReadValue<Vector2>().normalized;
            }else if (!_isDashing && context.started)
            {
                float dotProduct = Vector2.Dot(_previousDirection, context.ReadValue<Vector2>().normalized);
                if (dotProduct >= 0.7f)
                {
                    _isDashing = true;
                    _dashDirection = context.ReadValue<Vector2>().normalized;
                }
                else
                {
                    _doubleTapChrono = _dashTimeWindow;
                    _previousDirection = context.ReadValue<Vector2>().normalized;
                }
            }
        }*/

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

        #endregion



        #region Utils

        
        private void Move()
        {
            Vector3 inputDirection = _playerHead.forward * _playerInputMovement.y +
                                     _playerHead.right * _playerInputMovement.x;
            inputDirection.y = 0;

            // if (_isSprinting)
            //     _playerRigidbody.linearVelocity =
            //         inputDirection * (_moveSpeed * (_sprintMultiplier > 1f ? _sprintMultiplier : 1f));
            //else
            _playerRigidbody.linearVelocity = inputDirection * _moveSpeed;
            
            if (_playerInputMovement.magnitude <= 0f)
                _playerRigidbody.linearVelocity = Physics.gravity * _playerRigidbody.mass;
        }

        private void Dash()
        {
            Vector3 dashDirection = _playerHead.forward * _dashDirection.y + _playerHead.right * _dashDirection.x;
            dashDirection.y = 0;
            _playerRigidbody.AddForce(dashDirection * _dashForce, ForceMode.Impulse);
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
        //[SerializeField] private float _sprintMultiplier;
        
        //dash
        private float _doubleTapChrono;
        private Vector2 _previousDirection;
        
        private float _dashChrono;
        private bool _isDashing;
        private Vector2 _dashDirection;
        
        [SerializeField, Tooltip("Speed of the dash")] private float _dashDuration = .25f;
        [SerializeField, Tooltip("Time to trigger the double tap (in seconds)")] private float _dashTimeWindow;
        [SerializeField, Tooltip("Distance per 1 seconds")] private float _dashForce;

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
        
        [SerializeField] private SpawnPrefabCube _spawnPrefabCube;


        #endregion

        
    }
}
