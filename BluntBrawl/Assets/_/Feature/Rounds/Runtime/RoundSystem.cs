using System;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

namespace Rounds.Runtime
{
    public class RoundSystem : NetworkBehaviour
    {
        #region Publics

            public static RoundSystem Instance;
            public RoundStats m_roundStats;
            public float m_currentRoundTime => _roundTimer;

        #endregion

        #region Unity API

            private void Awake()
            {
                if (Instance == null)
                {
                    Instance = this;
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
                
                
            }

        [ServerCallback]
        private void Start()
        {
            // Initialisé uniquement côté serveur
            _roundTimer = m_roundStats.m_maxRountTime;
        }

        [ServerCallback]
        private void Update()
        {
            
            if (!_roundBreak && !_roundStarted && _players.Count >= m_roundStats.m_requiredPlayers)
            {
                _roundStarted = true;
                StartRound();
            }

            if (_roundStarted && !_roundBreak)
            {
                _roundTimer -= Time.deltaTime;
                if (_roundTimer <= 0)
                {
                    EndRound();
                }
            }
        }

        #endregion

        #region Main Methods

        [Server]
        public void StartRound()
        {
            Debug.Log("Round started!");
            _roundTimer = m_roundStats.m_maxRountTime;
            foreach (var player in _playersAlive)
            {
                Debug.Log("Player " + player.m_playerName+ " is alive!");
            }
        }
        
        [Server]
        public void EndRound()
        {
            Debug.Log("Round ended!");
            _roundStarted = false;
            _roundBreak = true;
        }

        
        
        [Server]
        public void SetRoundWinner()
        {
            RpcBroadcastWinner(_winnerPlayer.m_playerName);
            Invoke(nameof(EndRound), 3f);
        }
        
        
        [Server]
        public void SetRoundLoser(RoundPlayer player)
        {
            _roundBreak = true;
            RpcBroadcastLoser(player.m_playerName);
            _playersAlive.Remove(player);
            if (_playersAlive.Count == 1)
            {
                _winnerPlayer = _playersAlive[0];
                Invoke(nameof(SetRoundWinner),3f);
            }
            
        }
        
        
        #endregion

        #region Utils

        private void UpdateRoundTimer(float oldValue, float newValue)
        {
            if (_roundBreak) return;
            foreach (var text in m_texts)
            {
                //text.text = "Time\n" + newValue.ToString("F2");
            }
        }

        
        [ClientRpc]
        private void RpcBroadcastLoser(string loserName)
        {
            Debug.Log($"[CLIENT RPC] Broadcast loser: {loserName}");
            foreach (var text in m_texts)
            {
                text.text = "Disconnected :\n" + loserName;
            }
            Invoke(nameof(ClearBreak),4f);
        }


        [ClientRpc]
        private void RpcBroadcastWinner(string winnerName)
        {
            foreach (var text in m_texts)
            {
                text.text = "Winner \n" + winnerName;
            }
        }
        
        [Client]
        private void ClearBreak()
        {
            _roundBreak = false;
        }
        
        #endregion

        #region Player Management

        // Liste des joueurs maintenue par le serveur
        private readonly SyncList<RoundPlayer> _players = new SyncList<RoundPlayer>();
        private SyncList<RoundPlayer> _playersAlive = new SyncList<RoundPlayer>();

        
        [Server]
        public void RegisterPlayer(RoundPlayer player)
        {
            if (!_players.Contains(player)) _players.Add(player);
            if(!_playersAlive.Contains(player)) _playersAlive.Add(player);
            
            string name;
            for (int i = 0; i < _players.Count; i++)
            {
                if (i == 0) name = "Host";
                else name = "Player Client " + i;
                player.CmdSetPlayerName(name);
            }
            
        }

        [Server]
        public void UnregisterPlayer(RoundPlayer player)
        {
            if (_players.Contains(player))
                _players.Remove(player);
        }

        #endregion

        #region Privates
        
        [SyncVar] private RoundPlayer _winnerPlayer;
        [SyncVar] private bool _roundStarted = false;
        [SyncVar] private bool _roundBreak = false;
        [SyncVar] private float _broadCastCurrentTimer;
        [SyncVar(hook = nameof(UpdateRoundTimer))] private float _roundTimer;
        [SyncVar] private float _broadCastTimer = 15f;
        [SerializeField] private List<TMP_Text> m_texts;

        #endregion
    }
}
