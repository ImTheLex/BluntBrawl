using System;
using System.Collections;
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
                m_text.text = "Current Health: " + _health;
                
                
                if(m_renderer != null) baseColor = m_renderer.material.color;
                m_canvas.gameObject.SetActive(true);
            }
            

            #endregion

        
        #region Main Methods
        
            public void TakeDamage(int amount)
            {
                //if (!isServer) return;
                if (_isInvincible) return;
                Debug.Log($"Player {name} damaged");

                
                    _health -= amount;
                if (_health > 0)
                {
                    m_text.text = "Current Health: " + _health;

                    if (m_renderer is null)
                    {
                        m_renderer = gameObject.GetComponentInChildren<Renderer>();

                    }

                    m_renderer.material.color = Color.red;

                    Invoke(nameof(ResetColor), 1f);
                    StartCoroutine(HandleInvincibilityFrame());

                }
                else
                {
                    m_text.text = "You died : "  + _health;;
                    
                }

            }

            public IEnumerator HandleInvincibilityFrame()
            {
                _isInvincible = true;

                var counter = 0f;
                while (counter < _invincibilityDuration)
                {
                    counter += 0.1f;

                    yield return new WaitForSeconds(0.1f);
                    if (counter > 20f) break;
                }
                
                _isInvincible = false;
            }
            public void ResetColor()
            {
                GetComponentInChildren<Renderer>().material.color = baseColor;
            }
            
            
        #endregion
        
        
        #region Privates
        
        
            private int _health;
            [SerializeField] private float _invincibilityDuration = 1f;
        
            private string name;
            private Color baseColor;
            private bool _isInvincible;
        
        #endregion
    }
}
