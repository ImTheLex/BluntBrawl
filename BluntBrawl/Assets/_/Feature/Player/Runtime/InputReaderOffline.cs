using InputSystem.BluntBrawl;
using UINavigation.Runtime;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Runtime
{
    public class InputReaderOffline : MonoBehaviour, BluntBrawlInputActions.IPlayerActions,BluntBrawlInputActions.IBBXRILeftActions,BluntBrawlInputActions.IBBXRIRightActions

    {
        #region Unity API
        
            private void Awake()
            {
                _playerInputActions = new BluntBrawlInputActions();
                _playerInputActions.Player.SetCallbacks(this);
                _playerInputActions.BBXRILeft.SetCallbacks(this);
                _playerInputActions.BBXRIRight.SetCallbacks(this);
                _XROrigin = _playerOrigin.GetComponent<XROrigin>();

            }

            private void OnEnable() => _playerInputActions.Enable();
        
            private void OnDisable() => _playerInputActions.Disable();

            private void Update()
            {
                TrackingPositionController();
                TrackingRotationController();
            }

            #endregion
        public void OnMove(InputAction.CallbackContext context)
        {
            
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Vector2 direction = context.ReadValue<Vector2>();
                //MoveHead(direction);
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            
        }

        public void OnDebugPosition(InputAction.CallbackContext context)
        {
            
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            
        }

        public void OnInteractA(InputAction.CallbackContext context)
        {
            if(context.performed) _rightControllerInteractA.Interact();
        }
        

        #region Utils

            private void MoveHead(Vector2 direction)
            {
                Vector3 xrRotation = _XROrigin.transform.rotation.eulerAngles;
                
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
            
        #endregion

        
        
        #region Privates
        
            private BluntBrawlInputActions _playerInputActions;
            
            
            [SerializeField] private XROrigin _XROrigin;
            [SerializeField, Tooltip("XROrigin of this player")] private Transform _playerOrigin;
            
            
            [Header("Settings for Tracked Controller")] 
            [SerializeField] private Transform _leftController;
            [SerializeField] private Transform _rightController;
            
            private Vector3 _leftControllerInputPosition;
            private Vector3 _rightControllerInputPosition;
            
            private Quaternion _leftControllerInputRotation;
            private Quaternion _rightControllerInputRotation;
            
            [SerializeField] private RightControllerInteract _rightControllerInteractA; 
            
            
            [SerializeField, Tooltip("Angle in degree that will be add when right stick tap")] private float _rotateAngle;

        #endregion
        
    }
}
