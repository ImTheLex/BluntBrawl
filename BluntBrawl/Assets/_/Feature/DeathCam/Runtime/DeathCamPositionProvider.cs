using System;
using UnityEngine;

namespace DeathCam.Runtime
{
    public class DeathCamPositionProvider : MonoBehaviour
    {
        public static DeathCamPositionProvider Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else 
            {
                Destroy(gameObject);
            }
        }

        public Transform FindClosestCameraToPlayer(Vector3 playerPosition)
        {
            Transform closestCam = null;
            float shortestDistance = Mathf.Infinity;

            foreach (var cam in _cameraTransforms)
            {
                float distance = Vector3.Distance(playerPosition, cam.position);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestCam = cam;
                }
            }
            return closestCam;
        }
        
        [SerializeField] private Transform[] _cameraTransforms;
        
    }
}
