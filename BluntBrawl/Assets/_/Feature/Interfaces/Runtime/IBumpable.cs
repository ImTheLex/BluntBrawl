using Mirror;
using UnityEngine;

namespace Interfaces.Runtime
{   
    public interface IBumpable
    {
        public void PlayerBumpOnHit(Vector3 direction, float force);
        public void TargetPlayerBumpOnHit(NetworkConnectionToClient target, Vector3 direction, float force);
    }
}
