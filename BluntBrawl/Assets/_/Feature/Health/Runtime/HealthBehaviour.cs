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
        
        
        #region Mirror API
        
            public override void OnStartClient()
            {
                base.OnStartClient();
                // S'assurer que chaque client voit l'état actuel
                //UpdateHealth(_health, _health);
                UpdateVulnerability(_vulnerability,_vulnerability);
            }
            
        #endregion
        
        
        #region Unity API

            private void Awake()
            {
                name = gameObject.name;
                _health = m_maxHealth;
                Debug.Log("CurrentChanceToDIe " + _currentChanceToDie);
                //m_text.text = "Current Health: " + _health;
                
                
                if(m_renderer != null) baseColor = m_renderer.material.color;
                m_canvas.gameObject.SetActive(true);
            }
            

        #endregion

        
        #region Main Methods


            [Server]
            public void IncreaseVulnerability(int vulnerabilityAmount)
            {
                if (_isInvincible) return;
                StartCoroutine(HandleInvincibilityFrame());
                RpcFlash();
                //Pour chaque tranche de x dégat subit, on augmente de y les chances de se faire kick.
                _vulnerability += vulnerabilityAmount;
                var chancesToDie = Mathf.FloorToInt(_vulnerability / _damageTreshold) * _chanceToDiePerTreshold;

                _currentChanceToDie = chancesToDie;
                
                if (chancesToDie >= 100)
                {
                    HandleDamageableDeath();
                    return;
                }
                switch (_vulnerability)
                {
                    case 250:
                        chancesToDie += 100;
                        break;
                    case >= 100:
                        chancesToDie += 10;
                        break;
                    
                }
                UpdateVulnerability(_vulnerability, _vulnerability);
            }

            [Command(requiresAuthority = false)]
            public void CmdIncreaseVulnerability(int vulnerabilityAmount)
            {
                IncreaseVulnerability(vulnerabilityAmount);
            }
            
            [Server]
            public void HandleDamageableDeath()
            {
                //gameObject.SetActive(false);
                //RpcHandleDamageableDeath();
            }

            [ClientRpc]
            public void RpcHandleDamageableDeath()
            {
                gameObject.SetActive(false);
            }
            
            [Server]
            public void TakeDamage(int amount)
            {
                //if (!isServer) return;
                if (_isInvincible) return;
                _health -= amount;
                StartCoroutine(HandleInvincibilityFrame());
                
                if (_health > 0)
                {

                    RpcFlash();

                }
                else
                {
                    m_text.text = "You died : "  + _health;;
                    
                }

                UpdateHealth(_health, _health);
            }

            [Command(requiresAuthority = false)]
            public void CmdTakeDamage(int amount)
            {
                TakeDamage(amount);
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
                m_renderer.material.color = baseColor;
            }
            
            
        #endregion
        
        
        #region Utils


        [ContextMenu("Debug Vulnerability")]
        public void DebugVulnerability()
        {
            IncreaseVulnerability(100);
        }
        
        [ClientRpc]
        private void RpcFlash()
        {
            if (m_renderer == null) return;
            
            m_renderer.material.color = Color.red;
            Invoke(nameof(ResetColor), 0.5f);
        }
        
        private void UpdateHealth(int previousHealth, int currentHealth)
        { 
            m_text.text = "Current Health: " + currentHealth;
        }
        
        private void UpdateVulnerability(int previousVulnerability, int currentVulnerability)
        { 
            m_text.text = "Current Vulnerability: " + currentVulnerability;
        }
        
        #endregion
        
        #region Privates

            [SyncVar(hook = nameof(UpdateVulnerability))]
            private int _vulnerability;
            [SerializeField] private int _damageTreshold = 10;
            
            [SerializeField,SyncVar] private int _chanceToDiePerTreshold = 5;
            
            
            
            [SyncVar(hook = nameof(UpdateHealth))]
            private int _health;
            [SerializeField] private float _invincibilityDuration = 1f;
        
            private Color baseColor;
            [SyncVar] private bool _isInvincible;
            private int _currentChanceToDie;

            #endregion
    }
}
