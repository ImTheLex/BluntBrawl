using Mirror;
using UnityEngine;

namespace Interfaces.Runtime
{
    public interface IGrabbable
    {
        public WeaponStats m_weaponData=> new WeaponStats();
        Transform m_grabTransform { get; }
        public void CmdHideUI(GameObject grabber, NetworkIdentity identity);

        public void CmdDisplayUI(GameObject grabber, NetworkIdentity identity);

    }
}
