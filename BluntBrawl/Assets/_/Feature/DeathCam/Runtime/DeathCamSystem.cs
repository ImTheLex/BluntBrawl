using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace DeathCam.Runtime
{
    public class DeathCamSystem : MonoBehaviour
    {
        private void Awake()
        {
            _deathCamPositionProvider = DeathCamPositionProvider.Instance;
        }

        private void Update()
        {
            if (_isPendingCamSwipe)
            {
                _timer += Time.deltaTime;
                if (_timer >= _vignetteDelay)
                {
                    _playerCam.transform.SetParent(_pendingCam);
                    _playerCam.transform.localPosition = Vector3.zero;
                    _playerCam.transform.rotation = Quaternion.Euler(0,0,0); 
                    _isLooking = (_pendingCam != _playerCameraParent);
                    _isPendingCamSwipe = false;
                }
            }

            if (_isLooking == true)
            {
                _playerCam.transform.LookAt(_playerCameraParent);

            }
        }   

        public void RegisterPlayer(GameObject player)
        {
            _player = player;
        }

        public void RegisterCamera(Camera camera)
        {
            _playerCam =  camera;
            //_savedCamPosition = _playerCam.transform.localPosition;
            _playerCameraParent = _playerCam.transform.parent;
            _vignette = _playerCam.GetComponent<DeathCamVignette>();
            _vignetteDelay = _vignette.m_FadeInDuration;
        }
        
        [ContextMenu("Swipe Cam")]
        public void SwipeCamera()
        { 
            _pendingCam = _deathCamPositionProvider.FindClosestCameraToPlayer(_player.transform.position);
            _isPendingCamSwipe = true;
            _timer = 0;
            _vignette.DisplayVignette();
        }

        [ContextMenu("UnSwipe Cam")]
        public void UnSwipeCam()
        {
            _pendingCam = _playerCameraParent;
            _isPendingCamSwipe = true;
            _timer = 0;
            _vignette.DisplayVignette();
            //_playerCam.transform.SetParent(_playerCameraParent);
            //_isLooking = false;
        }
        
        private bool _isLooking;
        private bool _isPendingCamSwipe;
        private float _timer;
        private float _vignetteDelay;
        
        private DeathCamVignette _vignette;
        private DeathCamPositionProvider _deathCamPositionProvider;
        private GameObject _player;
        private Transform _playerCameraParent;
        private Transform _pendingCam;
        private Camera _playerCam;
        
        //private Vector3 _savedCamPosition;
        
    
    }

    
}
