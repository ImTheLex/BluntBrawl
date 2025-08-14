using Mirror;
using UnityEngine;

namespace Player.Runtime
{
    public class PlayerNetwork : NetworkBehaviour
    {
        public override void OnStartLocalPlayer()
        {
            _playerOrigin.SetActive(true);
            _playerAvatar.SetActive(false);
            
            //_playerLocomotion.SetActive(true);
        }

        public override void OnStartClient()
        {
            if (!isLocalPlayer)
            {
                _playerOrigin.SetActive(false);
                _playerAvatar.SetActive(true);
            }
        }

        #region Private and Protected
        
        
        [SerializeField] private GameObject _playerOrigin;
        [SerializeField] private GameObject _playerAvatar;

        //[SerializeField] private GameObject _playerLocomotion;
        

        #endregion
    }
}
