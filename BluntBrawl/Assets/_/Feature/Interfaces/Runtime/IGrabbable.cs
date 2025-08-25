using UnityEngine;

namespace Interfaces.Runtime
{
    public interface IGrabbable
    {
        string m_grabOwner { get; }
        Transform m_grabTransform { get; }
        public void DisplayGrabItemUI();
        public void HideGrabItemUI();
    }
}
