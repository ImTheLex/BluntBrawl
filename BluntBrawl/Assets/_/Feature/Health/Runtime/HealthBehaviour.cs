using System;
using Interfaces.Runtime;
using Mirror;
using TMPro;
using UnityEngine;

namespace Health.Runtime
{
    public class HealthBehaviour : NetworkBehaviour, IDamageable
    {

        #region Publics

            public int m_maxHealth;
            
            public Renderer m_renderer;
            public Canvas m_canvas;
            public TMP_Text m_text;

        #endregion
        
        
        #region Unity API

            private void Awake()
            {
                name = gameObject.name;
                _health = m_maxHealth;
                
                if(m_renderer != null) baseColor = m_renderer.material.color;
                m_canvas.gameObject.SetActive(true);
            }

            private void Update()
            {
                m_text.text = "Current Health: " + _health;
            }

            #endregion

        
        #region TestDamage
       
            public void TakeDamage(int amount)
            {
                Debug.Log($"Player {name} damaged");
                
                _health -= amount;
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
            private int _health;
            private Color baseColor;
        
        #endregion
    }
}
