using Interfaces.Runtime;
using Mirror;
using UnityEngine;

namespace Item.Runtime
{
    public class ItemGrabber : NetworkBehaviour
    {
        #region Publics
	
	
        #endregion
	
        #region Unity API

        
        public void OnTriggerEnter(Collider collider)
        {
            
            if(collider.TryGetComponent<IGrabbable>(out var grabbable))
            {
                _grabbableWeaponData = grabbable.m_weaponData;
                _grabbableObject = collider.gameObject;
                grabbable.DisplayGrabItemUI();
            }
        }
		
        public void OnTriggerExit(Collider collider)
        {
            
            if(collider.TryGetComponent<IGrabbable>(out var grabbable))
            {
	            _grabbableWeaponData = null;
                _grabbableObject = null;
                grabbable.HideGrabItemUI();
            }
        }
		
        #endregion
	
        #region MainMethods

        public void EquipStartingWeapon(GameObject weapon, WeaponStats weaponStats)
        {
	        _inHandWeapon = weapon;
	        _inHandWeaponData = weaponStats;
        } 
	
		[ContextMenu("Grab Item")]
		
		public void GrabItem()
        {
	
			
            if(_grabbableWeaponData == null || _grabbableObject == null) return;
            if(_inHandWeaponData != null) UngrabItem();
            
            GameObject obj = Instantiate(_grabbableWeaponData.m_inHandPrefab, transform);
            NetworkServer.Spawn(obj);
            
            
            Destroy(_grabbableObject);
        }
	
        #endregion
	
	
        #region Utils
	
        public void UngrabItem()
        {
	        GameObject obj = Instantiate(_inHandWeaponData.m_inWorldPrefab, _grabbableObject.transform.position, Quaternion.identity);
	        NetworkServer.Spawn(obj);
	        Destroy(_inHandWeapon);
        }
	
        #endregion
	
	
        #region Privates
	
        private string _grabOwner;
        
        private WeaponStats _inHandWeaponData;
        private GameObject _inHandWeapon;
        private WeaponStats _grabbableWeaponData;
        private GameObject _grabbableObject;
        

        #endregion
    }

}
