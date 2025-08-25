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

        public void Awake()
        {

	        var test = _currentItemInterface.GetComponent<IGrabbable>();
	        _currentItem = test.m_grabTransform.gameObject;
	        _currentLocalItem = test.m_localPrefab;
	        _currentWorldItem = test.m_worldPrefab;
        }

        public void OnTriggerEnter(Collider collider)
        {
            //if(!isLocalPlayer) return;
            if(collider.TryGetComponent<IGrabbable>(out var grabbable))
            {
				
                //if(grabbable.m_grabOwner != null) return;
				
                _grabbableCurrentTarget = grabbable.m_grabTransform.gameObject;
                _currentLocalItem = grabbable.m_localPrefab;
                grabbable.DisplayGrabItemUI();
	
            }
        }
		
        public void OnTriggerExit(Collider collider)
        {
            //if(!isLocalPlayer) return;
            if(collider.TryGetComponent<IGrabbable>(out var grabbable))
            {
                //if(grabbable.m_grabOwner != null) return;
                _currentLocalItem = _currentItem;
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
            if(_currentItem != null) UngrabItem();
            
            Instantiate(_currentLocalItem, transform.position, transform.rotation,transform);
            
            var spawnPos = _grabbableCurrentTarget.transform.position;
            
	        var obj = Instantiate(_currentWorldItem);
            obj.transform.position = new Vector3(spawnPos.x, spawnPos.y+2, spawnPos.z);
            
            var test = _currentLocalItem.GetComponent<IGrabbable>();
            _currentWorldItem = test.m_worldPrefab;
            Destroy(_grabbableCurrentTarget);
            

        }
	
        #endregion
	
	
        #region Utils
	
        public void UngrabItem()
        {
			Instantiate(_currentWorldItem);
			//_currentItem.SetActive(false);
			Destroy(_currentItem);

			/*
            var currentItemRb = _currentItem.GetComponent<Rigidbody>();
            _currentItem.transform.SetParent(null);
            currentItemRb.isKinematic = false;
            */
        }
	
        #endregion
	
	
        #region Privates
	
        private string _grabOwner;
        
        [SerializeField] private GameObject _currentItemInterface;
        
        private GameObject _currentLocalItem;
        private GameObject _currentWorldItem;

        private GameObject _grabbableCurrentTarget;
        private GameObject _currentItem;
	
        #endregion
    }

}
