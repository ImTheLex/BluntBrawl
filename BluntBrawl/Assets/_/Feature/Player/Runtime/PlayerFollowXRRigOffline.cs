using System;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace Player.Runtime
{
    public class PlayerFollowXRRigOffline : MonoBehaviour
    {
        
        [SerializeField]private Transform _playerOrigin;
        [SerializeField]private Transform _playerAvatar;
        private XROrigin _XRorigin;

        //private void Awake()=> _XRorigin = _playerOrigin.GetComponent<XROrigin>();

        public void Awake()
        {
            _XRorigin = _playerOrigin.GetComponent<XROrigin>();

        }

        private void Update()
        {
            transform.position = _XRorigin.Origin.transform.position;
            transform.rotation = _XRorigin.Origin.transform.rotation;
            _playerAvatar.rotation = Quaternion.Euler(RotateCharacterByHeadRotation(_XRorigin.Camera.transform));
        }

        private Vector3 RotateCharacterByHeadRotation(Transform head)
        {
            Vector3 headRotation = new Vector3(0f, head.rotation.eulerAngles.y, 0f);
            return headRotation;
        }
        
    }
}