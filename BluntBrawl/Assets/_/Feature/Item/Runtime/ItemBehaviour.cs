using Interfaces.Runtime;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

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
            HideGrabItemUI();
        }

        #endregion
	
        #region MainMethods
	
        public void DisplayGrabItemUI()
        {
            if (_grabItemUI == null)
            {
                Assert.IsNull(_grabItemUI + " is null, so no UI display");
                return;
            }
            _grabItemUI.gameObject.SetActive(true);
            _canBeGrab = true;
        }
	
        public void HideGrabItemUI()
        {
            if (_grabItemUI == null)
            {
                Assert.IsNull(_grabItemUI + " is null, so no UI hide");
                return;
            }
            _grabItemUI.gameObject.SetActive(false);
            _canBeGrab = false;
        }
        #endregion
	
        #region Privates
	
        private bool _canBeGrab;
        private string _grabOwner;
        
        [SerializeField] private Canvas _grabItemUI;
        [SerializeField] private WeaponStats _weaponData;
        
		
        #endregion
    }
}
