using System.Collections.Generic;
using Mirror;
using Structs.Runtime;
using UnityEngine;

namespace NetworkConnection.Runtime
{
    public class SkinProvider : NetworkBehaviour
    {
        public static SkinProvider Instance;
        public PlayerSkinsStruct m_skinReferences;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Instance._requestedSkin = _requestedSkin;
                Destroy(gameObject);
            }
        }

        public void RegisterStruct(PlayerSkinsStruct reference)
        {
            m_skinReferences = reference;
        }

        public void RequestSkin(int skinRequested)
        {
            _requestedSkin = skinRequested;
        }
        
        public void ApplySkin()
        {
            m_skinReferences.m_hatRenderer.sharedMesh = _hatsVariant[_requestedSkin];
            m_skinReferences.m_jacketRenderer.sharedMesh = _jacketVariant[_requestedSkin];
            m_skinReferences.m_sleevesRenderer.sharedMesh = _sleevesVariant[_requestedSkin];
            m_skinReferences.m_shoesRenderer.sharedMesh = _shoesVariant[_requestedSkin];
        }
        
        [TargetRpc]
        public void RpcApplySkin(NetworkConnectionToClient target)
        {
            ApplySkin();
        }
        
        [ContextMenu("Request Skin 0")]
        public void DebugRequestSkin0()
        {
            _requestedSkin = 0;
        }
        
        [ContextMenu("Request Skin 1")]
        public void DebugRequestSkin1()
        {
            _requestedSkin = 1;
        }
        
        [ContextMenu("Request Skin 2")]
        public void DebugRequestSkin2()
        {
            _requestedSkin = 2;
        }
        
        [ContextMenu("Request Skin 3")]
        public void DebugRequestSkin3()
        {
            _requestedSkin = 3;
        }
        
        
        [SyncVar] private int _requestedSkin;

        
        [SerializeField] private List<Mesh>_hatsVariant;
        [SerializeField] private List<Mesh>_jacketVariant;
        [SerializeField] private List<Mesh>_sleevesVariant;
        [SerializeField] private List<Mesh>_shoesVariant;
    }
}

