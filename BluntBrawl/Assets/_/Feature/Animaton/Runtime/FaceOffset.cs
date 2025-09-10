using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Animation.Runtime
{
    public class FaceOffset : NetworkBehaviour
    {
        
        #region Main methods


        public void ChangeFace(string name) => _faceMaterial.mainTextureOffset = _offsets[name];


        #endregion

        

        #region Private

        private Dictionary<String, Vector2> _offsets = new Dictionary<String, Vector2>()
        {
            {"normal", Vector2.zero },
            {"hurt1", new Vector2(0.25f, 0f) },
            {"hurt2", new Vector2(0.5f, 0f) },
            {"surprise", new Vector2(0.75f, 0f) },
            {"happy1", new Vector2(0f, -0.5f)},
            {"happy2", new Vector2(0.25f, -0.5f)},
            {"happy3", new Vector2(0.5f, -0.5f)},
            {"taunt", new Vector2(0.75f, -0.5f)}
        };
        
        [SerializeField] private SkinnedMeshRenderer _faceMeshRenderer;
        private Material _faceMaterial => _faceMeshRenderer.sharedMaterials[1];

        #endregion
    }
}
