using System;
using UnityEngine;
using Mirror;
using Mirror.Discovery;

namespace NetworkConnection.Runtime
{
    public class AutoMatchmaking : MonoBehaviour
    {
        #region Publics

        public NetworkDiscovery m_discovery;
        
        
        #endregion
        private void Start()
        {
            m_discovery.OnServerFound.AddListener(OnServerFound);
            Debug.Log("Searching for host");
            m_discovery.StartDiscovery();
            Invoke(nameof(BecomeHost),1f);
        }
        
        #region Utils

        private void OnServerFound(ServerResponse info)
        {
            if (foundServer) return;
            
            foundServer = true;
            Debug.Log("Server found at " + info.EndPoint.Address + ":" + info.EndPoint.Port);
            NetworkManager.singleton.networkAddress = info.uri.Host; 
            NetworkManager.singleton.StartClient();
        }

        private void BecomeHost()
        {
            if (foundServer) return;
            Debug.Log("BecomeHost");
            NetworkManager.singleton.StartHost();
            m_discovery.AdvertiseServer();
            
        }
        #endregion
        
        
        #region Privates
        
        private bool foundServer = false;
        
        #endregion
    }
}
