using System;
using Interfaces.Runtime;
using Mirror;
using MisteryBox.Runtime;
using Sounds.Runtime;
using UnityEngine;

namespace Item.Runtime
{
    public class ItemGrabber : NetworkBehaviour
    {
        #region Publics


        public WeaponStats m_inHandWeaponData => _inHandWeaponData;
	
        #endregion
	
        #region Unity API

        private void Awake()
        {
	        _misteryBoxSystem = FindFirstObjectByType<MisteryBoxSystem>();
        }

        public override void OnStartServer()
        {
	        base.OnStartServer();
	        GameObject obj = Instantiate(_startingWeapon.m_inHandPrefab, transform);
	        NetworkServer.Spawn(obj, connectionToClient); 

	        _inHandWeapon = obj;
	        _inHandWeaponData = _startingWeapon;

	        obj.transform.localPosition = Vector3.zero;
        }

        [Command]
        public void CmdResetWeapon()
        {
	        Debug.Log("Asked for a weapon reset");
	        ResetWeapon();
        }

        [Command(requiresAuthority = false)]
        public void CmdDropWeapon()
        {
	        DropWeapon();
        }

        public void DropWeapon()
        {
	        NetworkServer.Destroy(_inHandWeapon);
	        _inHandWeapon = null;
        }
        
        [Server]
        public void ResetWeapon()
        {
	        GameObject obj = Instantiate(_startingWeapon.m_inHandPrefab, transform);
	        NetworkServer.Spawn(obj, connectionToClient); 

	        _inHandWeapon = obj;
	        _inHandWeaponData = _startingWeapon;
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
	
        
        
		[Command,ContextMenu("Grab Item")]
		public void GrabItem()
		{

            if(_grabbableWeaponData == null || _grabbableObject == null) return;
            if (_inHandWeaponData != null)
            {
	            UngrabItem();
            }
            
            GameObject obj = Instantiate(_grabbableWeaponData.m_inHandPrefab, transform);
            NetworkServer.Spawn(obj,connectionToClient);
            _misteryBoxSystem.RemoveFromSpawnedWeapons(_grabbableObject);
            _inHandWeapon = obj;
            _inHandWeaponData = _grabbableWeaponData;
            NetworkServer.Destroy(_grabbableObject);

		}
	
        #endregion
	
	
        #region Utils
	
       
        public void UngrabItem()
        {
	        GameObject obj = Instantiate(_inHandWeaponData.m_inWorldPrefab, _grabbableObject.transform.position, Quaternion.identity);
	        NetworkServer.Spawn(obj);
            _misteryBoxSystem.AddToSpawnedWeapons(obj);
	        
            _inHandWeapon.GetComponent<WeaponSFX>().WeaponDropSFX(netIdentity.connectionToClient);
	        NetworkServer.Destroy(_inHandWeapon);
	        _inHandWeapon = null;
	        _inHandWeaponData = null;	
        }
	
        #endregion
	
        #region Hooks

        private void OnWeaponChanged(GameObject oldWeapon, GameObject newWeapon)
        {
	        if (oldWeapon != null)
		        Destroy(oldWeapon);

	        if (newWeapon != null)
	        {
		        newWeapon.transform.SetParent(transform);
		        newWeapon.transform.localPosition = Vector3.zero;
		        newWeapon.transform.localRotation = Quaternion.identity;
	        }
        }
        
        #endregion
	
        #region Privates
	
	        private string _grabOwner;
	        
		    private WeaponStats _inHandWeaponData;
		    [SyncVar(hook = nameof(OnWeaponChanged))]
			private GameObject _inHandWeapon;
	        private WeaponStats _grabbableWeaponData;
	        private GameObject _grabbableObject;
	        private MisteryBoxSystem _misteryBoxSystem;

	        [SerializeField] private WeaponStats _startingWeapon;
        

        #endregion
    }

}
