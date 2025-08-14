using System;
using UnityEngine;

namespace Player.Runtime
{
    public class PlayerFolowXRRig : MonoBehaviour
    {
        
        [SerializeField]private Transform _playerOrigin;


        private void Update()
        {
            transform.position = _playerOrigin.position;
        }
    }
}
