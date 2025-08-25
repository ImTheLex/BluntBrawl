using System;
using Interfaces.Runtime;
using Mirror;
using UnityEngine;

namespace Item.Runtime
{
    public class ItemGrabber : MonoBehaviour
    {
        #region Publics
	
	
        #endregion
	
        #region Unity API

        public void Update()
        {
	        var _currentItemInterface = GetComponentInChildren<IGrabbable>();
	        _currentItem = _currentItemInterface.m_grabTransform.gameObject;
        }

        public void OnTriggerEnter(Collider collider)
        {
            //if(!isLocalPlayer) return;
            if(collider.TryGetComponent<IGrabbable>(out var grabbable))
            {
				
                //if(grabbable.m_grabOwner != "none") return;
				
                _grabbableCurrentTarget = grabbable.m_grabTransform.gameObject;
                grabbable.DisplayGrabItemUI();
	
            }
        }
		
        public void OnTriggerExit(Collider collider)
        {
            //if(!isLocalPlayer) return;
            if(collider.TryGetComponent<IGrabbable>(out var grabbable))
            {
                //if(grabbable.m_grabOwner != "none") return;
							
                _grabbableCurrentTarget = null;
                grabbable.HideGrabItemUI();
            }
        }
		
        #endregion
	
        #region MainMethods
	
		[ContextMenu("Grab Item")]
        public void GrabItem()
        {
	
			
            if(_grabbableCurrentTarget == null) return;
            var nextGrabbableItemPosition = _currentItem.transform.position;
			
            if(_currentItem != null) UngrabItem();
            _grabbableCurrentTarget.transform.position = nextGrabbableItemPosition;
            _grabbableCurrentTarget.transform.SetParent(transform);
            _currentItem = _grabbableCurrentTarget;
			
			
			
			
        }
	
        #endregion
	
	
        #region Utils
	
        public void UngrabItem()
        {
			
            var currentItemRb = _currentItem.GetComponent<Rigidbody>();
            _currentItem.transform.SetParent(null);
            currentItemRb.isKinematic = false;
        }
	
        #endregion
	
	
        #region Privates
	
        private string _grabOwner;
        private GameObject _grabbableCurrentTarget;
        private GameObject _currentItem;
	
        #endregion
    }

}
