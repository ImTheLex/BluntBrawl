using UnityEngine;

namespace Player.Runtime
{
    public class PlayerFollowXRRig : MonoBehaviour
    {
        
        [SerializeField]private Transform _playerOrigin;

        private void Update()=> transform.position = _playerOrigin.position;
        
    }
}
