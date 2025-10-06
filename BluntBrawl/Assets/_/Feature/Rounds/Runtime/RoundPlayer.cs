using System;
using Animation.Runtime;
using DeathCam.Runtime;
using Item.Runtime;
using Mirror;
using Skins.Runtime;
using Sounds.Runtime;
using UnityEngine;

namespace Rounds.Runtime
{
    public class RoundPlayer : NetworkBehaviour
    {
        //public int m_playerCurrentHealth;
        [SyncVar(hook = nameof(OnRoundWonChanged))] public int m_roundsWon;
        [SyncVar(hook = nameof(OnNameChanged))] public string m_playerName;
        [SyncVar(hook = nameof(OnSkinIndexChanged))] public int m_skinIndex;

        
        public InGameUIAnimation m_inGameUIAnimation => _inGameUIAnimation;
        
        public CombatSFX m_combatSFX => GetComponent<CombatSFX>();

        public SkinBehaviour m_skinBehaviour;

        [SyncVar] public bool m_playerInitialized = false;
        
        public bool m_isInputActive = true;
        private void OnNameChanged(string oldName, string newName)
        {
            Debug.Log($"Player Name: {newName}");
        }
        
        private void OnRoundWonChanged(int oldRound, int newRound)
        {
            Debug.Log($"Round won: {newRound}");
        }

        private void OnSkinIndexChanged(int oldSkinIndex, int newSkinIndex)
        {
            m_skinBehaviour.m_skinIndex = newSkinIndex;
            //m_skinBehaviour.ApplySkin();
        }

        private void Awake()
        {
            m_skinBehaviour = _skinBehaviour;
        }

        private RoundSystem roundSystem;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public override void OnStartClient()
        {
            base.OnStartClient();
            roundSystem = RoundSystem.Instance;
            if (isLocalPlayer)
            {   
                //deathCam = FindFirstObjectByType<DeathCamSystem>();
                //deathCam = DeathCamSystem.Instance;
                //_deathCamSystem.RegisterPlayer(gameObject);
                //_deathCamSystem.RegisterCamera(_playerCam);
            }
           
            //roundSystem = FindFirstObjectByType<RoundSystem>();
            if(roundSystem) CmdAddPlayer();
            _spawnPosition = _xrPosition.transform.position;
        }

        [ClientRpc]
        public void InitializePlayer()
        {
            Debug.Log($"Player Initialized at " + m_playerName);
            //deathCam.UnSwipeCam(connectionToClient);
            _xrPosition.transform.position = _spawnPosition;
            m_playerInitialized = true;
            _itemGrabber.CmdResetWeapon();
            
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        
        public void ResetPosition()
        {
            _xrPosition.transform.position = _spawnPosition;
        }
        
        public void UnsetInitialize()
        {
            m_playerInitialized = false;
        }
        
        [ClientRpc]
        public void CancelDeathAnimation()
        {
            Debug.Log($"Player cancelled death animation at " + m_playerName);
            _playerAnimator.SetBool("death", false);
        }
        
        //[Command(requiresAuthority = false)]
        public void CmdAddPlayer()
        {
            roundSystem.RegisterPlayer(this);
        }
        
        [Command(requiresAuthority = false)]
        public void CmdSetDefeat()
        {
            //_deathCamSystem.SwipeCamera();
            _itemGrabber.CmdDropWeapon();
            roundSystem.SetRoundLoser(this);
            //UnsetInitialize();
        }
        
        public void CmdSetCurrentPlayerHealth(int currentHealth)
        {
            //m_playerCurrentHealth = currentHealth;
        }

        [TargetRpc]
        public void RpcSetPlayerName(NetworkConnection target, string name)
        {
            m_playerName = name;
        }

        public Camera GetCam()
        {
            return _playerCam;
        }

        public void RestoreVision()
        {
            //_playerAnimator.SetBool("death",false);
            _deathCamVignette.RestoreVignette();
        }
        
        #region Privates

            
            [SerializeField] private DeathCamSystem _deathCamSystem;
            [SerializeField] private DeathCamVignette _deathCamVignette;
            [SerializeField] private Animator _playerAnimator;
            
            [SyncVar] private Vector3 _spawnPosition;
            [SerializeField] private Camera _playerCam;
            [SerializeField] private GameObject _xrPosition;
            
            [SerializeField] private InGameUIAnimation _inGameUIAnimation;

            [SerializeField] private ItemGrabber _itemGrabber; 
            
            [SerializeField] private SkinBehaviour _skinBehaviour;
        #endregion
    }
}
