using Interfaces.Runtime;
using Mirror;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace Item.Runtime
{
    public class ItemBehaviour : NetworkBehaviour,IGrabbable
    {
        #region Publics
	
        public string m_grabOwner
        {
            get => _grabOwner;
            set => _grabOwner = value;
        }

        public Transform m_grabTransform => transform;

        public WeaponStats m_weaponData
        {
            get
            {
                return _weaponData;
            }
        }

        #endregion
	
        #region Unity API
        

        private void OnDisable()
        {
            //HideGrabItemUI();
        }

        #endregion
	
        #region MainMethods
        

        [Command(requiresAuthority = false)]
        public void CmdDisplayUI(GameObject targetToLookAt, NetworkIdentity identity)
        {
            Debug.Log("In DisplayUI");
            DisplayUI(targetToLookAt, identity);
        }

        [Server]
        public void DisplayUI(GameObject targetToLookAt,  NetworkIdentity identity)
        {
            TargetDisplayGrabItemUI(identity.connectionToClient,targetToLookAt);
        }
        
        
        [TargetRpc]
        public void TargetDisplayGrabItemUI(NetworkConnectionToClient target, GameObject targetToLookAt)
        {
            _grabItemUI.gameObject.SetActive(true);
            RotateUI(targetToLookAt);
            _canBeGrab = true;
        }

        
        public void RotateUI(GameObject targetToLookAt)
        {
            Debug.Log("In RotateUI : " + targetToLookAt);
            _grabItemUI.transform.LookAt(targetToLookAt.GetComponentInChildren<XROrigin>().gameObject.transform);
        }
        
        
        
        [Command(requiresAuthority = false)]
        public void CmdHideUI(GameObject grabber, NetworkIdentity identity)
        {
            HideUI(grabber, identity);
        }

        [Server]
        public void HideUI(GameObject targetToLookAt, NetworkIdentity identity)
        {
            TargetHideUI(identity.connectionToClient, targetToLookAt);
        }

        [TargetRpc]
        public void TargetHideUI(NetworkConnectionToClient target, GameObject targetToLookAt)
        {
            _grabItemUI.gameObject.SetActive(false);
            _canBeGrab = false;
        }
        

       
        #endregion
	
        #region Privates
	
        private bool _canBeGrab;
        private string _grabOwner;
        
        private GameObject _objToLookAt;
        [SerializeField] private Canvas _grabItemUI;
        [SerializeField] private WeaponStats _weaponData;
        
		
        #endregion
    }
}
