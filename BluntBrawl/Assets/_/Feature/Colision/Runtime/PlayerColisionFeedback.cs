using Health.Runtime;
using Interfaces.Runtime;
using UnityEngine;

namespace Colision.Runtime
{
    [RequireComponent(typeof(HealthBehaviour))]
    public class PlayerColisionFeedback : MonoBehaviour, IBumpable
    {
        #region Publics Varia
        

        

        #endregion
        
        #region Utils


        public void PlayerBumpOnHit(Vector3 direction, float force)
        {
            
        }

        #endregion


        #region Privates

        
        [SerializeField] private Rigidbody _playerRigidbody;
        private HealthBehaviour _playerHealth => GetComponent<HealthBehaviour>();
        

        #endregion
    }
}
