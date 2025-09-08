using System;
using System.Collections.Generic;
using Interfaces.Runtime;
using Mirror;
using TMPro;
using UnityEngine;

namespace Health.Runtime
{
    public class HealthBehaviour : NetworkBehaviour, IDamageable
    {

        #region Publics

            public Renderer m_renderer;
            public Canvas m_canvas;
            public TMP_Text m_text;
            
            public int m_currentHealth => _currentHealth;
            public int m_maxHealth => _maxHealth;

        #endregion
        
        
        #region Mirror API
        
            public override void OnStartClient()
            {
                base.OnStartClient();
                // S'assurer que chaque client voit l'état actuel
                //UpdateVulnerability(_vulnerability,_vulnerability);
                UpdateHealth(_currentHealth, _currentHealth);
            }
            
        #endregion
        
        
        #region Unity API

            private void Awake()
            {
                name = gameObject.name;
                _currentHealth = _maxHealth;
                
                if(m_renderer != null) baseColor = m_renderer.material.color;
                m_canvas.gameObject.SetActive(true);
            }


            private void Start()
            {
                Debug.Log("Local player : " + isLocalPlayer);
                if (isLocalPlayer == false)
                {
                    foreach (var bar in _bars)
                    {
                        bar.SetActive(false);
                    }
                }

                if (isLocalPlayer == true)
                {
                    foreach (var bar in _bars)
                    {
                        bar.GetComponent<Renderer>().material.color = Color.green;
                    }
                }
            }

            private void Update()
            {
                if (_isInvincible) IFrame();
            }

            
            #endregion

        
        #region Main Methods
        
            //[Server]
            /*public void IncreaseVulnerability(int vulnerabilityAmount)
            {
                //change color material
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
            */

            //[Command(requiresAuthority = false)]
            /*public void CmdIncreaseVulnerability(int vulnerabilityAmount)
            {
                if (_isInvincible) return;
                _isInvincible = true;
                IncreaseVulnerability(vulnerabilityAmount);
            }
            */
            
            public void TakeDamage(int damageAmount)
            {
                _currentHealth -= damageAmount;
                //HandleHealth();
            }
            

            [Command(requiresAuthority = false)]
            public void CmdTakeDamage(int damageAmount)
            {
                if (_isInvincible) return;
                _isInvincible = true;
                TakeDamage(damageAmount);
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
            
            
            public void IFrame()
            {
                _invincibilityTimer += Time.deltaTime;
                if (_invincibilityTimer >= _invincibilityDuration)
                {
                   _isInvincible = false;
                   _invincibilityTimer = 0;
                }
            }
            
            public void ResetColor()
            {
                m_renderer.material.color = baseColor;
            }
            
            public void ResetHealth()
            {
                _currentHealth = _maxHealth;
                HandleHealth();
            }

            [Command(requiresAuthority = false)]
            public void CmdResetHealth()
            {
                ResetHealth();
            }
            
            
        #endregion
        
        
        #region Utils

            /*
            [ContextMenu("Debug Vulnerability")]
            public void DebugVulnerability()
            {
                IncreaseVulnerability(100);
            }
            */
            
            
            [ContextMenu("Debug CmdTakeDamage")]
            public void DebugCmdTakeDamage()
            {
                CmdTakeDamage(30);
            }
            
            [ContextMenu("Debug TakeDamage")]
            public void DebugTakeDamage()
            {
                TakeDamage(10);
            }


            [ContextMenu("Reset CmdHealth")]
            public void DebugCmdResetHealth()
            {
                CmdResetHealth();
            }
            
            [ClientRpc]
            private void RpcFlash()
            {
                if (m_renderer == null) return;
                
                m_renderer.material.color = Color.red;
                Invoke(nameof(ResetColor), 0.5f);
            }
            
            /*private void UpdateVulnerability(int previousVulnerability, int currentVulnerability)
            { 
                m_text.text = "Current Vulnerability: " + currentVulnerability;
            }
            */

            private void HandleHealth()
            {
                if (!isLocalPlayer) return;
                float healthPercentage = (float)_currentHealth / (float)_maxHealth;
                _activeBars = Mathf.CeilToInt(healthPercentage * _maxBars);

                for (int i = 0; i < _bars.Count; i++)
                {
                    if (_currentHealth <= (_maxHealth / 4) * 3)
                    {
                        _bars[i].gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                    }

                    if (_currentHealth <= (_maxHealth / 2))
                    {
                        _bars[i].gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                        
                    }
                    if (_currentHealth <= (_maxHealth / 4))
                    {
                        _bars[i].gameObject.GetComponent<Renderer>().material.color = Color.red;
                    }
                    
                    _bars[i].SetActive(i < _activeBars);
                }
            }
            private void UpdateHealth(int previousHealth, int currentHealth)
            {
                if (!isLocalPlayer) return;
                m_text.text = currentHealth.ToString();
                HandleHealth();
               
            }
        
        #endregion
        
        #region Privates
        
            
            //[SyncVar(hook = nameof(UpdateVulnerability))] private int _vulnerability;
            //[SerializeField] private int _damageTreshold = 10;
            //[SerializeField,SyncVar] private int _chanceToDiePerTreshold = 5;
            //[SerializeField] private float _invincibilityDuration = 1f;
            //private int _currentChanceToDie;
            
            
            [SyncVar(hook = nameof(UpdateHealth))] private int _currentHealth;
            private int _activeBars;
            [SerializeField] private List<GameObject> _bars;
            [SerializeField] private int _maxBars = 5;
            [SerializeField] private int _maxHealth = 100;
            
            [Header("Invincibility")]
            private float _invincibilityTimer;
            [SerializeField] private float _invincibilityDuration;
            [SyncVar] private bool _isInvincible;
            
        
            private Color baseColor;

            #endregion
    }
}
