using System;
using System.Collections.Generic;
using Mirror;
using NetworkConnection.Runtime;
using UnityEngine;

namespace Structs.Runtime
{
    
    public class PlayerSkinsStruct : MonoBehaviour
    {
        private void Awake()
        {
            SkinProvider.Instance.RegisterStruct(this);
            
        }

        public SkinnedMeshRenderer m_jacketRenderer;
        public SkinnedMeshRenderer m_sleevesRenderer;
        public SkinnedMeshRenderer m_shoesRenderer;
        public SkinnedMeshRenderer m_hatRenderer;
    }
}
