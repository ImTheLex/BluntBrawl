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
            _networkManager = NetworkManager.singleton;
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
            _networkManager.networkAddress = info.uri.Host; 
            _networkManager.StartClient();
        }

        private void BecomeHost()
        {
            if (foundServer) return;
            Debug.Log("BecomeHost");
            _networkManager.StartHost();
            m_discovery.AdvertiseServer();
            
        }
        #endregion
        
        
        #region Privates
        
        private NetworkManager _networkManager;
        private bool foundServer = false;
        
        #endregion
    }
}
