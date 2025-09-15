using System.Collections.Generic;
using InputSystem.BluntBrawl;
using Item.Runtime;
using Mirror;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using InputDevice = UnityEngine.XR.InputDevice;

namespace Player.Runtime
{
    public class PlayerMovement : NetworkBehaviour, BluntBrawlInputActions.IPlayerActions,
        BluntBrawlInputActions.IBBXRILeftActions, BluntBrawlInputActions.IBBXRIRightActions,
        BluntBrawlInputActions.IBBXRIRightInteractionActions
    {

        #region Publics
        
        [HideInInspector] public bool m_isBumping = false;

        [HideInInspector] public XROrigin m_XROrigin => _XROrigin;
        
        
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
            if (!isLocalPlayer) return;
            
            TrackingPositionController();
            TrackingRotationController();
            
            if (DetectKillZ()) _playerRigidbody.position = new Vector3(0, 5, 0);
            if (m_isBumping) return;
            
            
            if (_doubleTapChrono >= 0f) _doubleTapChrono -= Time.deltaTime;
            if (_dashCooldownChrono >= 0f) _dashCooldownChrono -= Time.deltaTime;
            
            if (_isDashing)
            {
                _dashChrono += Time.deltaTime;
                AnimatorMoving(_networkIdentity.connectionToClient, false);
                Dash();
                if (_dashChrono > _dashDuration)
                {
                    _dashChrono = 0;
                    _dashCooldownChrono = _dashCooldown;
                    _isDashing = false;
                }

                return;
            }
            Move();
        }

        

        #endregion

        


        #region Input action



        //main player
        public void OnMove(InputAction.CallbackContext context)
        {
            if (!isLocalPlayer) return;
            _playerInputMovement = context.ReadValue<Vector2>();
            if (_playerInputMovement != Vector2.zero)
            {
                AnimatorMoving(_networkIdentity.connectionToClient, true);
                AnimatorHorizontal(_networkIdentity.connectionToClient, _playerInputMovement.x);
                AnimatorVertical(_networkIdentity.connectionToClient, _playerInputMovement.y);
            }
            else AnimatorMoving(_networkIdentity.connectionToClient, false);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (!isLocalPlayer) return;
            if (context.started)
            {
                Vector2 direction = context.ReadValue<Vector2>();
                MoveHead(direction);
            }
            
        }
        

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (!isLocalPlayer) return;
            if (context.started)
            {
                // Vector3 direction = _playerHead.transform.forward + _playerRigidbody.position;
                // direction.y += 1f;
                // _spawnPrefabCube.CmdSpawn(direction);
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
                if (_dashCooldownChrono >= 0f) return;
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


        #region Publics Methods
        
        
        
        public void SendHapticToController(InputDeviceCharacteristics hand, float amplitude, float duration)
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(hand, devices);
            foreach (var device in devices)
            {
                if (device.TryGetHapticCapabilities(out HapticCapabilities capabilities) && capabilities.supportsImpulse)
                {
                    device.SendHapticImpulse(0, amplitude, duration);
                }
            }
        }

        [TargetRpc]
        public void TargetRPCHapticToController(NetworkConnectionToClient target, InputDeviceCharacteristics hand,
            float amplitude, float duration)
        {
            SendHapticToController(hand, amplitude, duration);
        }

        #endregion



        #region Utils

        
        [TargetRpc]
        private void AnimatorMoving(NetworkConnectionToClient target, bool move)
        {
            _animator.SetBool("IsMoving", move);
        }

        [TargetRpc]
        private void AnimatorHorizontal(NetworkConnectionToClient target, float horizontal)
        {
            _animator.SetFloat("horizontal", horizontal);
        }

        [TargetRpc]
        private void AnimatorVertical(NetworkConnectionToClient target, float vertical)
        {
            _animator.SetFloat("vertical", vertical);
        }
        
        private void Move()
        {
            if (!IsGrounded())
            {
                _playerRigidbody.linearVelocity += Physics.gravity * _playerRigidbody.mass;
                AnimatorMoving(_networkIdentity.connectionToClient, false);
                return;
            }
            
            
            Vector3 inputDirection = _playerHead.forward * _playerInputMovement.y +
                                     _playerHead.right * _playerInputMovement.x;
            inputDirection.y = 0;
            
            _playerRigidbody.linearVelocity = inputDirection * _moveSpeed;
        }

        private bool IsGrounded()
        {
            return Physics.OverlapSphere(_XROrigin.transform.position + new Vector3(0f,0.3f,0f),0.3f, _groundLayer).Length > 0;
        }
        
        private bool DetectKillZ()
        {
            return _playerRigidbody.position.y <= -15f;
        }
        
        private void MoveHead(Vector2 direction)
        {
            Vector3 xrRotation =_XROrigin.transform.rotation.eulerAngles;
            Debug.Log(direction.x);
            if (direction.x >= 0.5f)
            {
                _XROrigin.transform.rotation = Quaternion.Euler(xrRotation.x,xrRotation.y + _rotateAngle,xrRotation.z);
                Debug.Log(xrRotation.x);
            }
            else if (direction.x <= -0.5f)
            {
                _XROrigin.transform.rotation = Quaternion.Euler(xrRotation.x,xrRotation.y - _rotateAngle,xrRotation.z);
                Debug.Log(xrRotation.x);
            }
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
            _leftController.localRotation = _leftControllerInputRotation;
            _rightController.localRotation = _rightControllerInputRotation;
        }

        #endregion
        
        
        #region Private and Protected

        
        private BluntBrawlInputActions _playerInputActions;
        
        [Header("Settings for Movement")]
        [SerializeField, Tooltip("XROrigin of this player")] private Transform _playerOrigin;
        [SerializeField, Tooltip("Meter per second")] private float _moveSpeed;
        [SerializeField] private LayerMask _groundLayer;
        private bool _isSprinting;
        //[SerializeField] private float _sprintMultiplier;
        
        [Header("Settings for rotation")]
        [SerializeField, Tooltip("Angle in degree that will be add when right stick tap")] private float _rotateAngle;
        
        
        //dash
        private float _doubleTapChrono;
        private Vector2 _previousDirection;
        
        private float _dashChrono;
        private bool _isDashing;
        private Vector2 _dashDirection;
        
        [Header("Settings for Dash")]
        [SerializeField, Tooltip("Speed of the dash")] private float _dashDuration = .25f;
        [SerializeField, Tooltip("Time to trigger the double tap (in seconds)")] private float _dashTimeWindow;
        [SerializeField, Tooltip("Distance per 1 seconds")] private float _dashForce;

        [SerializeField, Tooltip("Time in seconds after a new Dash can be done")] private float _dashCooldown;

        private float _dashCooldownChrono;
        
        
        
        [Header("Settings for Tracked Controller")] 
        [SerializeField] private Transform _leftController;
        [SerializeField] private Transform _rightController;
        
        private Transform _playerHead;
        private XROrigin _XROrigin;
        private Rigidbody _playerRigidbody;
        
        
        private Vector2 _playerInputMovement;
        private Vector2 _playerInputRotation;

        private Vector3 _leftControllerInputPosition;
        private Vector3 _rightControllerInputPosition;
        
        private Quaternion _leftControllerInputRotation;
        private Quaternion _rightControllerInputRotation;
        
        [SerializeField] private ItemGrabber _itemGrabber;
        [SerializeField] private Animator _animator;
        private NetworkIdentity _networkIdentity=> GetComponent<NetworkIdentity>();

        #endregion

        
    }
}
