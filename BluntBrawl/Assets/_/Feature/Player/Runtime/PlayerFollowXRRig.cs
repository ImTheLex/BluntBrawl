using System;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace Player.Runtime
{
    public class PlayerFollowXRRig : MonoBehaviour
    {
        
        [SerializeField]private Transform _playerOrigin;
        private XROrigin _XRorigin;

        private void Awake()=> _XRorigin = _playerOrigin.GetComponent<XROrigin>();
        


        private void Update()
        {
            transform.position = _XRorigin.Origin.transform.position;
            transform.rotation = _playerOrigin.rotation;
        } 
        
    }
}
