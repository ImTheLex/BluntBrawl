using UnityEngine;

namespace Interfaces.Runtime
{
    public interface IGrabbable
    {
        public WeaponStats m_weaponData=> new WeaponStats();
        Transform m_grabTransform { get; }
        public void DisplayGrabItemUI();
        public void HideGrabItemUI();
    }
}
