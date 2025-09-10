using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Animation.Runtime
{
    public class FaceOffset : NetworkBehaviour
    {
        #region Unity API

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            Initialise();
        }

        #endregion

        #region Main methods
        

        public void ChangeFace(string name)=> _faceMaterial.mainTextureOffset = _offsets[name];
        

        #endregion

        #region Utils

        private void Initialise()
        {
            _offsets.Add("normal",Vector2.zero);
            _offsets.Add("hurt1", new Vector2(0.25f,0f));
            _offsets.Add("hurt2", new Vector2(0.5f,0f));
            _offsets.Add("surprise", new Vector2(0.75f,0f));
            _offsets.Add("happy1", new Vector2(0f,-0.5f));
            _offsets.Add("happy2", new Vector2(0.25f,-0.5f));
            _offsets.Add("happy3", new Vector2(0.5f,-0.5f));
            _offsets.Add("taunt", new Vector2(0.75f,-0.5f));

            _faceMaterial = _faceMeshRenderer.materials[1];
        }

        #endregion

        
        
        
        #region Private

        private Dictionary<String, Vector2> _offsets = new Dictionary<String, Vector2>();
        [SerializeField] private SkinnedMeshRenderer _faceMeshRenderer;
        private Material _faceMaterial;

        #endregion
    }
}
