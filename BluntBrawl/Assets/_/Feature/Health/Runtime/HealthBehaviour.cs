using System;
using Interfaces.Runtime;
using UnityEngine;

namespace Health.Runtime
{
    public class HealthBehaviour : MonoBehaviour, IDamageable
    {

        #region Publics

            public int m_health;
            public Renderer m_renderer;

        #endregion
        
        
        #region Unity API

        private void Awake()
        {
            name = gameObject.name;
            if(m_renderer != null) baseColor = m_renderer.material.color;
        }

        #endregion

        
        #region TestDamage
       
            public void TakeDamage()
            {
                Debug.Log($"Player {name} damaged");
                if (m_renderer is null)
                {
                    m_renderer = gameObject.GetComponentInChildren<Renderer>();
                    
                }
                m_renderer.material.color = Color.red;
                
                
                Invoke(nameof(ResetColor), 1f);
                
            }

            public void ResetColor()
            {
                GetComponentInChildren<Renderer>().material.color = baseColor;
            }
        #endregion
        
        
        #region Privates
        
            private string name;
            private Color baseColor;
        
        #endregion
    }
}
