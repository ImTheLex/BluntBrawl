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

        private void Awake()
        {
	        GameObject obj = Instantiate(_startingWeapon.m_inHandPrefab, transform);
	        NetworkServer.Spawn(obj);
	        obj.transform.localPosition = Vector3.zero;
        }

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
            obj.transform.localPosition = Vector3.zero;
            
            
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

        [SerializeField] private WeaponStats _startingWeapon;

        #endregion
    }

}
