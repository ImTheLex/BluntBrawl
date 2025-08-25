using System;
using Interfaces.Runtime;
using Mirror;
using TMPro;
using UnityEngine;

namespace Item.Runtime
{
    public class ItemBehaviour : NetworkBehaviour,IGrabbable
    {
        #region Publics
	
        public string m_grabOwner => _grabOwner;
        public GameObject m_worldPrefab
        {
            get { return _worldPrefab; }
        }

        public GameObject m_localPrefab
        {
            get { return _localPrefab; }
        }

        public Transform m_grabTransform => transform;
	
        #endregion
	
        #region Unity API
        public void Awake()
        {
            _grabItemUIText = _grabItemUI.GetComponentInChildren<TMP_Text>();
        }

        private void OnDisable()
        {
            HideGrabItemUI();
        }

        #endregion
	
        #region MainMethods
	
        public void DisplayGrabItemUI()
        {
            _grabItemUI.gameObject.SetActive(true);
            _grabItemUIText.text = "Press trigger to grab";
            _canBeGrab = true;
        }
	
        public void HideGrabItemUI()
        {
            _grabItemUI.gameObject.SetActive(false);
            _canBeGrab = false;
        }
        #endregion
	
        #region Privates
	
        private bool _canBeGrab;
        private string _grabOwner;
        
        [SerializeField] private Canvas _grabItemUI;
        [SerializeField] private GameObject _worldPrefab;
        [SerializeField] private GameObject _localPrefab;
        
        private TMP_Text _grabItemUIText;
		
        #endregion
    }
}
