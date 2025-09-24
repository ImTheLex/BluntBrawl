using Animation.Runtime;
using DeathCam.Runtime;
using Mirror;
using UnityEngine;

namespace Rounds.Runtime
{
    public class RoundPlayer : NetworkBehaviour
    {
        //public int m_playerCurrentHealth;
        [SyncVar(hook = nameof(OnRoundWonChanged))] public int m_roundsWon;
        [SyncVar(hook = nameof(OnNameChanged))] public string m_playerName;
        
        public InGameUIAnimation m_inGameUIAnimation => _inGameUIAnimation;


        public bool m_playerInitialized = false;
        
        private void OnNameChanged(string oldName, string newName)
        {
            Debug.Log($"Player Name: {newName}");
        }
        
        private void OnRoundWonChanged(int oldRound, int newRound)
        {
            Debug.Log($"Round won: {newRound}");
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
            //deathCam.UnSwipeCam(connectionToClient);
            _xrPosition.transform.position = _spawnPosition;
            m_playerInitialized = true;
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
            roundSystem.SetRoundLoser(this);
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
        
        #region Privates

            
            [SerializeField]private DeathCamSystem _deathCamSystem;
            [SyncVar] private Vector3 _spawnPosition;
            [SerializeField] private Camera _playerCam;
            [SerializeField] private GameObject _xrPosition;
            
            [SerializeField] private InGameUIAnimation _inGameUIAnimation;

        #endregion
    }
}
