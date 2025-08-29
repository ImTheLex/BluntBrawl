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

            private void Update()
            {
                if (_isInvincible) IFrame();
            }

            #endregion

        
        #region Main Methods


            //[Server]
            public void IncreaseVulnerability(int vulnerabilityAmount)
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

            //[Command(requiresAuthority = false)]
            public void CmdIncreaseVulnerability(int vulnerabilityAmount)
            {
                if (_isInvincible) return;
                _isInvincible = true;
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
            private float _invincibilityTimer;
        
            private Color baseColor;
            [SyncVar] private bool _isInvincible;
            private int _currentChanceToDie;

            #endregion
    }
}
