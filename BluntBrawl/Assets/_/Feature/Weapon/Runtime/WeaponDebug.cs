using System;
using TMPro;
using UnityEngine;

namespace Weapon.Runtime
{
    public class WeaponDebug : MonoBehaviour
    {
        #region Publics

            public bool m_isVerbose;
            public TMP_Text m_debugText;
            public WeaponBehaviour m_weaponBehaviour;
            public Canvas m_debugCanvas;

        #endregion

        private void Start()
        {
        }

        #region Unity API

            private void Update()
            {
                if (m_isVerbose)
                {
                    if (m_debugCanvas.gameObject.activeSelf == false) m_debugCanvas.gameObject.SetActive(true);
                    m_debugText.text = "Velocity: " + m_weaponBehaviour.m_velocity.ToString("F2") +"\nRequired: " + m_weaponBehaviour.m_speedRequired;
                }
                else
                {
                    m_debugCanvas.gameObject.SetActive(false);
                }
            }

        #endregion
    }
}
