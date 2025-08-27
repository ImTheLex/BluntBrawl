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
                float fps = 1.0f / Time.deltaTime;
                
                if (m_isVerbose)
                {
                    if (m_debugCanvas.gameObject.activeSelf == false) m_debugCanvas.gameObject.SetActive(true);
                    m_debugText.text = "Nombre de fps :" + fps.ToString("F0");
                }
                else
                {
                    m_debugCanvas.gameObject.SetActive(false);
                }
            }

        #endregion
        
        #region Private and Protected
        
        
        
        
        #endregion
    }
}
