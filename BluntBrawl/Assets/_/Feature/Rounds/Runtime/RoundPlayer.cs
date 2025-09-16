using System;
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
            deathCam = DeathCamSystem.Instance;
            deathCam.RegisterPlayer(gameObject);
            deathCam.RegisterCamera(_playerCam);
             //roundSystem = FindFirstObjectByType<RoundSystem>();
            if(roundSystem) CmdAddPlayer();
            _spawnPosition = _xrPosition.transform.position;
        }

        [ClientRpc]
        public void InitializePlayer()
        {
            deathCam.UnSwipeCam();
            _xrPosition.transform.position = _spawnPosition;
        }
        
        //[Command(requiresAuthority = false)]
        public void CmdAddPlayer()
        {
            roundSystem.RegisterPlayer(this);
        }
        
        [Command(requiresAuthority = false)]
        public void CmdSetDefeat()
        {
            deathCam.SwipeCamera();
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

            
            private DeathCamSystem deathCam;
            [SyncVar] private Vector3 _spawnPosition;
            [SerializeField] private Camera _playerCam;
            [SerializeField] private GameObject _xrPosition;

        #endregion
    }
}
