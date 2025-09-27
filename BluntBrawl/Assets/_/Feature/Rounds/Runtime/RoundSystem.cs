using System.Collections.Generic;
using System.Linq;
using HealthBox.Runtime;
using Mirror;
using MisteryBox.Runtime;
using Sounds.Runtime;
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
            public float m_preStartRoundTimer => _preStartRoundTimer;
            
            public bool m_isPreStartingRound => _isPreStartingRound;

            [SyncVar] public bool m_isPlayingRound;

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

                if (_mysteryBoxSystem is null) _mysteryBoxSystem = FindFirstObjectByType<MisteryBoxSystem>();

            }

        [ServerCallback]
        private void Start()
        {
            // Initialisé uniquement côté serveur
            _roundTimer = m_roundStats.m_maxRoundTime;
        }

        [ServerCallback]
        private void Update()
        {
            
            if (_currentRound > m_roundStats.m_maxRounds)
            {
                EndMatch();
                return;
            }

            if (!_isPreStartingRound && _playersAlive.Count >= m_roundStats.m_requiredPlayers && !_roundStarted)
            {
                PreStartRound();
            }
            
            if (_isPreStartingRound)
            {
                
                _preStartRoundTimer -= Time.deltaTime;
                RpcUpdatePreStartRoundTimer();
                
                if (_preStartRoundTimer <= 0)
                {
                    StartRound();
                }
            }
            
            if (_roundStarted && !_roundBreak)
            {
                _roundTimer -= Time.deltaTime;
                RpcUpdateRoundTimer();
                if (_roundTimer <= 0)
                {
                    EndRound();
                }
            }
        }

        #endregion

        #region Main Methods


        [Server]
        public void PreStartRound()
        {
            ResetPlayers();
            RespawnProps();
            _isPreStartingRound = true;
            _preStartRoundTimer = m_roundStats.m_preStartRoundTimer;
            if (!_soundsWaitingRoom)
            {
                _soundsWaitingRoom = true;
                ClearWaitingRoom();
            }
            Invoke(nameof(SendStartRoundAnim),2f);
        }
        
        [Server]
        public void StartRound()
        {
            Debug.Log("Round started!");
            _isPreStartingRound = false;
            m_isPlayingRound = true;
            _roundStarted = true;
            _roundTimer = m_roundStats.m_maxRoundTime;
            StartCombatMusic();

        }
        
        [Server]
        public void EndRound()
        {
            Debug.Log("Round ended!");
            
            _roundStarted = false;
            m_isPlayingRound = false;
            _roundBreak = true;
            CheckWinners();
            _currentRound++;
            RepopulatePlayers();
            Invoke(nameof(ClearBreak), 3f);
            StopCombatMusic();
            SendEndRoundAnim();

        }

        public void EndMatch()
        {
           _matchWinner = _playersAlive.OrderByDescending(p => p.m_roundsWon).First();
           RpcBroadcast($"Match won by\n{_matchWinner.m_playerName}");
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
            SendLoserAnim(player.netIdentity.connectionToClient, player);
            SendWinnerAnim(_playersAlive[0].netIdentity.connectionToClient,_playersAlive[0]);
            if (_playersAlive.Count == 1)
            {
                _winnerPlayer = _playersAlive[0];
                _winnerPlayer.m_roundsWon++;
                Invoke(nameof(SetRoundWinner),3f);
            }
            Invoke(nameof(ClearBreak),6f);
        }
        
        
        #endregion

        #region Utils


        private void RespawnProps()
        {
            if (_safetyCounter > 10) return;
            _safetyCounter++;
            _healthBoxSystem.Reset();
            _healthBoxSystem.RestartCycle();
            _mysteryBoxSystem.Reset();
            _mysteryBoxSystem.SpawnBox();
        }
        
        private void RepopulatePlayers()
        {
            foreach (var player in _players)
            {
                if(!_playersAlive.Contains(player)) _playersAlive.Add(player);
            }
        }
        
        [Server]
        private void ResetPlayers()
        {
            foreach (var player in _players)
            {
                player.InitializePlayer();
                player.m_combatSFX.StopCombatMusic();
            }
        }
        
        private void ForEachTextType(string message)
        {
            foreach (var text in m_texts)
            {
                text.text = message;
            }
        }
        private void CheckWinners()
        {
            if (_playersAlive.Count > 1)
            {
                RpcBroadcastMatchNull();
            }
        }
        
        [ClientRpc]
        private void RpcUpdatePreStartRoundTimer()
        {
            //if (_roundBreak) return;
            var message = "Time\n" + m_preStartRoundTimer.ToString("F2");
            ForEachTextType(message);
        }
        
        [ClientRpc]
        private void RpcUpdateRoundTimer()
        {
            if (_roundBreak) return;
            var message = "Time\n" + m_currentRoundTime.ToString("F2");
            ForEachTextType(message);
        }

        [ClientRpc]
        private void RpcBroadcast(string message)
        {
            ForEachTextType(message);
        }
        
        [ClientRpc]
        private void RpcBroadcastMatchNull()
        {
            var message = "Match Null";
            ForEachTextType(message);
        }
        
        [ClientRpc]
        private void RpcBroadcastLoser(string loserName)
        {
            Debug.Log($"[CLIENT RPC] Broadcast loser: {loserName}");
            var message = "Disconnected :\n" + loserName;
            ForEachTextType(message);
        }


        [ClientRpc]
        private void RpcBroadcastWinner(string winnerName)
        {
            var message = "Winner \n" + winnerName;
            ForEachTextType(message);
        }
        
        [Client]
        private void ClearBreak()
        {
            _roundBreak = false;
        }

        
        [ClientRpc]
        private void SendStartRoundAnim()
        {
            foreach (var player in _players)
            {
                player.m_inGameUIAnimation.SendStartRound();
                player.m_combatSFX.StartRoundSFX();
            }
        }
        
        [ClientRpc]
        private void SendEndRoundAnim()
        {
            foreach (var player in _players)
            {
                player.m_inGameUIAnimation.SendEndRound();
                player.m_combatSFX.EndRoundSFX();
            }
        }
        
        [TargetRpc]
        private void SendWinnerAnim(NetworkConnectionToClient target,RoundPlayer player)
        {
                player.m_inGameUIAnimation.SendWin();
                player.m_combatSFX.WinRoundSFX();
        }
        
        [TargetRpc]
        private void SendLoserAnim(NetworkConnectionToClient target,RoundPlayer player)
        {
                player.m_inGameUIAnimation.SendLose();
                player.m_combatSFX.LoseRoundSFX();
        }

        [ClientRpc]
        private void ClearWaitingRoom()
        {
            FindFirstObjectByType<WaitingRoomSFX>().DestroyWaitingRoomSFX();
        }

        [ClientRpc]
        private void StartCombatMusic()
        {
            foreach (var player in _players)
            {
                player.m_combatSFX.StartCombatMusic(_currentRound-1);
            }
        }

        [ClientRpc]
        private void StopCombatMusic()
        {
            foreach (var player in _players)
            {
                player.m_combatSFX.StopCombatMusic();
            }
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
                if(_playersAlive.Count == 1)
                  name = "Host";
                else name = "Player Client " + (_playersAlive.Count - 1);
                
           //NetworkIdentity identity = player.GetComponent<NetworkIdentity>();
           //player.RpcSetPlayerName(identity.connectionToClient,name);

           player.m_playerName = name;

        }

        [Server]
        public void UnregisterPlayer(RoundPlayer player)
        {
            if (_players.Contains(player))
                _players.Remove(player);
        }

        #endregion

        #region Privates
        
        [SerializeField] HealthBoxSystem _healthBoxSystem;
        [SerializeField] MisteryBoxSystem _mysteryBoxSystem;
        
        [SyncVar] private RoundPlayer _winnerPlayer;
        [SyncVar] private RoundPlayer _matchWinner;

        [SyncVar] private bool _isPreStartingRound = false;
        [SyncVar] private float _preStartRoundTimer;
        
        [SyncVar] private bool _roundStarted = false;
        [SyncVar] private bool _roundBreak = false;
        [SyncVar] private float _roundTimer;
        [SyncVar] private int _currentRound = 1;
        [SyncVar] private float _broadCastCurrentTimer;
        
        [SerializeField] private List<TMP_Text> m_texts;
        
        private bool _soundsWaitingRoom = false;


        private int _safetyCounter;

        #endregion
    }
}
