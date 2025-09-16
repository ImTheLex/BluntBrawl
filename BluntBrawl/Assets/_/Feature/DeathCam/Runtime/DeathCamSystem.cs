using Mirror;
using UnityEngine;

namespace DeathCam.Runtime
{
    public class DeathCamSystem : NetworkBehaviour
    {
        public static DeathCamSystem Instance;
        
        private void Awake()
        {
            if(Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void RegisterPlayer(GameObject player)
        {
            _player = player;
        }

        public void RegisterCamera(Camera camera)
        {
            _playerCam =  camera;
            _savedCamPosition = _playerCam.transform.localPosition;
        }
        
        [ContextMenu("Swipe Cam")]
        public void SwipeCamera()
        {
            //if (!isLocalPlayer) return;
            Transform closestCam = null;
            float shortestDistance = Mathf.Infinity;

            foreach (var cam in _cameraTransforms)
            {
                float distance = Vector3.Distance(_player.transform.position, cam.position);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestCam = cam;
                }
            }

            if (closestCam != null)
            {
                _playerCam.transform.localPosition = closestCam.position;
                //_playerCam.transform.LookAt(_player.transform);
            }
        }

        [ContextMenu("UnSwipe Cam")]
        public void UnSwipeCam()
        {
            //if (!isLocalPlayer) return;
            _playerCam.transform.localPosition = _savedCamPosition;
            //_playerCam.transform.rotation = Quaternion.Euler(0,0,0); 
        }
        
        
        private GameObject _player;
        private Camera _playerCam;
        private Vector3 _savedCamPosition;
        [SerializeField] private Transform[] _cameraTransforms;
    
    }
}
