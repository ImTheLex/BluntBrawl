using UnityEngine;

namespace Interfaces.Runtime
{
    public interface IGrabbable
    {
        string m_grabOwner { get; }
        GameObject m_worldPrefab { get; }
        GameObject m_localPrefab { get; }
        Transform m_grabTransform { get; }
        public void DisplayGrabItemUI();
        public void HideGrabItemUI();
    }
}
