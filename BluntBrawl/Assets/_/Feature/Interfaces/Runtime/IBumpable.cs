using Mirror;
using UnityEngine;

namespace Interfaces.Runtime
{   
    public interface IBumpable
    {
        public void PlayerBumpOnHit(Vector3 direction, float force, float verticalForce, float horizontalForce);
        public void TargetPlayerBumpOnHit(NetworkConnectionToClient target, Vector3 direction, float force, float verticalForce, float horizontalForce);
        
    }
}
