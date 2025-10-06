using System;
using Interfaces.Runtime;
using Mirror;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Item.Runtime
{
    public class ItemThrower : NetworkBehaviour
    {
        public float m_velocity;

        private void Start()
        {
            _lastPos =  transform.localPosition;
        }

        private void Update()
        {
            _velocity = (transform.localPosition - _lastPos) / Time.deltaTime;
            _lastPos = transform.localPosition;
        }
        
        #region Main Methods
        
            [Command(requiresAuthority = false)]
            public void SetThrowable(GameObject throwable)
            {
                _inHandThrowableObject = throwable;
            }
            
            [Command(requiresAuthority = false)]
            public void SetThrowableData(WeaponStats stats)
            {
                _inHandThrowableData = stats;
            }
            
            /*
            public void Throw()
            {
                Debug.Log("Throw");
                RpcQuitParent();
                
                var rb = _inHandThrowableObject.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;
                
                Vector3 throwDirection = _throwPosition.forward;
                Vector3 localVelocity = transform.TransformDirection(_velocity);

                rb.AddForce(localVelocity + throwDirection * _throwForce, ForceMode.Impulse);
                Debug.Log($"Debug ? Direction: {throwDirection} localVelocity: {_velocity}");
                


                //_inHandThrowableObject = null;
            }
*/
            
            [Command(requiresAuthority = false)]
            public void CmdThrow(Vector3 velocity)
            {
                if (_inHandThrowableObject == null) return;
                
                RpcQuitParent();
                var rb = _inHandThrowableObject.GetComponent<Rigidbody>();
                _inHandThrowableObject.GetComponent<IThrowable>().SetState(IThrowable.ThrowableState.Launched);

                //rb.AddForce(localVelocity + throwDirection * _throwForce, ForceMode.Impulse);
    
                RpcSyncThrow(rb.gameObject);
            }
            
            [ClientRpc]
            private void RpcSyncThrow(GameObject obj)
            {
                var rb = obj.GetComponent<Rigidbody>();
                rb.useGravity = true;
                Vector3 throwDirection = _throwPosition.forward;
                Vector3 localVelocity = transform.TransformDirection(_velocity);
                rb.AddForce(localVelocity + throwDirection * _throwForce, ForceMode.Impulse);

            }


            [ClientRpc]
            public void RpcQuitParent()
            {
                _inHandThrowableObject.transform.SetParent(null);
            }
            
            /*
            [Command(requiresAuthority = false)]
            public void CmdThrow()
            {
                Throw();
            }
            */

            public void Throw()
            {
                Debug.Log("Throwing");
                //_velocity = new Vector3(0,2,5);
                CmdThrow(_velocity);
            }
            
            [ContextMenu("Throw Item")]
            public void DebugThrow()
            {
                Throw();
            }
            
            private void OnThrowableChanged(GameObject oldThrowable, GameObject newThrowable)
            {
                if (oldThrowable != null)
                    Destroy(oldThrowable);

                if (newThrowable != null)
                {
                    newThrowable.transform.SetParent(_throwPosition.transform);
                    newThrowable.transform.localPosition = Vector3.zero;
                    newThrowable.transform.localRotation = Quaternion.identity;
                }
            }
            
            
        #endregion
        
        
        
        #region Privates

            [SerializeField] private Transform _throwPosition;
            [SerializeField] private float _throwForce = 2f;
            private Vector3 _lastPos;
            private Vector3 _velocity;
        
            [SyncVar(hook = nameof(OnThrowableChanged))]
            private GameObject _inHandThrowableObject;
            private WeaponStats _inHandThrowableData;
            
            
        #endregion
    }
}
