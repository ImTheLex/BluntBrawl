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

        #endregion

        #region Unity API

            private void Update()
            {
                if (m_isVerbose)
                {
                    m_debugText.text = "Velocity: " + m_weaponBehaviour.m_velocity.ToString("F2");
                }
            }

        #endregion
    }
}
