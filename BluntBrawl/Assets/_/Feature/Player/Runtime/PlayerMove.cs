using System;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Runtime
{
    public class PlayerMove: NetworkBehaviour
    {
        #region Publics

        public InputActionAsset m_actions;
        public float m_moveSpeed = 1f;

        #endregion
        
        #region Unity API
        private void Awake()
        {
            InputActionMap map = m_actions.FindActionMap("Player"); 
            _moveAction = map.FindAction("Move");
            _moveAction.Enable();
        }

        private void Update()
        {
            Vector2 move = _moveAction.ReadValue<Vector2>();
            float x = transform.position.x + move.x * m_moveSpeed * Time.fixedDeltaTime;
            float z = transform.position.z + move.y * m_moveSpeed * Time.fixedDeltaTime;
            transform.position = new Vector3(x, transform.position.y, z);
            Debug.Log(move);
        }

        #endregion
        
        
        #region Privates

        private InputAction _moveAction;

        #endregion
    }
}
