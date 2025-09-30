using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Skins.Runtime
{
    public class SkinBehaviour : NetworkBehaviour
    {
        public int m_skinIndex;
        
        [ClientRpc]
        public void ApplySkin()
        {
            Debug.Log($"Applying skin {m_skinIndex}");
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.SetTexture("_BaseMap",_skinTextures[m_skinIndex]);
            foreach (var meshRenderer in _skinnedMeshRenderers)
            {
                meshRenderer.SetPropertyBlock(block);
            }
        }
        [SerializeField] private List<SkinnedMeshRenderer> _skinnedMeshRenderers = new List<SkinnedMeshRenderer>();
        [SerializeField] private List<Texture> _skinTextures = new List<Texture>();
    }
}
