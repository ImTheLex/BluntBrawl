using UnityEngine;

namespace Interfaces.Runtime
{   
    public interface IBumpable
    {
        public void PlayerBumpOnHit(Vector3 direction, float force);
    }
}
