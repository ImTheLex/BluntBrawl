using Mirror;
using UnityEngine;

namespace Player.Runtime
{
    public class PlayerNetwork : NetworkBehaviour
    {
        public override void OnStartLocalPlayer()
        {
            _playerOrigin.SetActive(true);
            _playerLeftController.SetActive(true);
            _playerRightController.SetActive(true);
            _playerAvatar.SetActive(false);
        }

        public override void OnStartClient()
        {
            if (!isLocalPlayer)
            {
                _playerOrigin.SetActive(false);
                _playerLeftController.SetActive(false);
                _playerRightController.SetActive(false);
                _playerAvatar.SetActive(true);
            }
        }

        #region Private and Protected
        
        
        [SerializeField] private GameObject _playerOrigin;
        [SerializeField] private GameObject _playerAvatar;
        [SerializeField] private GameObject _playerLeftController;
        [SerializeField] private GameObject _playerRightController;
        

        #endregion
    }
}
