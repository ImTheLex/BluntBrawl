using System;
using System.Collections.Generic;
using System.Linq;
using HealthBox.Runtime;
using Mirror;
using MisteryBox.Runtime;
using PrimeTween;
using Skins.Runtime;
using Sounds.Runtime;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace Rounds.Runtime
{
    public class RoundSystem : NetworkBehaviour
    {
        #region Publics

            public enum RoundState
            {
                WaitingForPlayers,
                BeginPreStartRound,
                PreStartRound,
                StartRound,
                RoundLive,
                BeginPreRoundBreak,
                PreRoundBreak,
                RoundBreak,
                BeginPreEndRound,
                PreEndRound,
                EndRound,
                BeginPreEndMatch,
                PreEndMatch,
                EndMatch,
                BeginPreQuitMatch,
                PreQuitMatch,
                QuitMatch,
            }
            
            [SyncVar] public RoundState m_roundState = RoundState.WaitingForPlayers;
            public static RoundSystem Instance;
            public RoundStats m_roundStats;
            public float m_currentRoundTime => _roundTimer;
            public float m_preStartRoundTimer => _preStartRoundTimer;
            
            //public bool m_isPreStartingRound => _isPreStartingRound;

            //[SyncVar] public bool m_isPlayingRound;

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
                _waitForPlayerTimer = m_roundStats.m_waitForPlayerTimer;
                _preRoundBreakTimer = m_roundStats.m_preRoundBreakTimer;
                _preQuitMatchTimer = m_roundStats.m_preQuitMatchTimer;
                _preEndRoundTimer = m_roundStats.m_preStartRoundTimer;
            }
            

            [ServerCallback]
            private void Update()
            {

                switch (m_roundState)
                {
                    case RoundState.WaitingForPlayers:
                        WaitForPlayers();
                        break;
                    case RoundState.BeginPreStartRound:
                        BeginPreStartRound();
                        break;
                    case RoundState.PreStartRound:
                        PreStartRound();
                        break;
                    case RoundState.StartRound:
                        StartRound();
                        break;
                    case RoundState.RoundLive:
                        RoundLive();
                        break;
                    case RoundState.BeginPreRoundBreak:
                        BeginPreRoundBreak();
                        break;
                    case RoundState.PreRoundBreak:
                        PreRoundBreak();
                        break;
                    case RoundState.RoundBreak:
                        RoundBreak();
                        break;
                    case RoundState.BeginPreEndRound:
                        BeginPreEndRound();
                        break;
                    case RoundState.PreEndRound:
                        PreEndRound();
                        break;
                    case RoundState.EndRound:
                        EndRound();
                        break;
                    case RoundState.BeginPreEndMatch:
                        BeginPreEndMatch();
                        break;
                    case RoundState.PreEndMatch:
                        PreEndMatch();
                        break;
                    case RoundState.EndMatch:
                        EndMatch();
                        break;
                    case RoundState.BeginPreQuitMatch:
                        BeginPreQuitMatch();
                        break;
                    case RoundState.PreQuitMatch:
                        PreQuitMatch();
                        break;
                    case RoundState.QuitMatch:
                        QuitMatch();
                        break;
                    default:
                        break;
                }
            }

        #endregion

        #region Main Methods
        
        [Server]
        public void SetRoundWinner()
        {
            _winnerPlayer = _playersAlive[0];
            _winnerPlayer.m_roundsWon++;
            
            RpcBroadcastWinner(_winnerPlayer.m_playerName);
            RpcBroadcastTimer("");
            List<int> indexes = GetPanelIndexesForPlayer(_winnerPlayer);
            RpcBroadcastWinnerPoints(indexes.ToArray(),_winnerPlayer.m_roundsWon);
            SendWinnerAnim(_playersAlive[0].netIdentity.connectionToClient,_playersAlive[0]);
            
            m_roundState = RoundState.BeginPreEndRound;

        }

        private void OnDisable()
        {
            Destroy(_mysteryBoxSystem.gameObject);
            Destroy(gameObject);
        }

        [Server]
        public void SetRoundLoser(RoundPlayer player)
        {
            RpcBroadcastLoser(player.m_playerName);
            RpcBroadcastTimer("");
            _playersAlive.Remove(player);
            SendLoserAnim(player.netIdentity.connectionToClient, player);
            player.m_isInputActive = false;
            Tween.Delay(1.5f,onComplete: () => TargetRpcSendLoserToSpectate(player.netIdentity.connectionToClient,player));
            Tween.Delay(2.5f, onComplete: player.CancelDeathAnimation);
            
            m_roundState = RoundState.BeginPreRoundBreak;
            

        }
        
        
        #endregion

        #region Round Flow
        
            [Server]
            private void WaitForPlayers()
            {
                _waitForPlayerTimer -= Time.deltaTime;
                RpcBroadcastCommunication($"Waiting for players ({_players.Count}/{m_roundStats.m_requiredPlayers})");
                RpcBroadcastTimer($"{_waitForPlayerTimer:F2}");
                
                if (_waitForPlayerTimer <= 0)
                {
                    if (_players.Count < m_roundStats.m_requiredPlayers)
                    {
                        _waitForPlayerTimer = m_roundStats.m_waitForPlayerTimer;
                        return;
                    }
                    
                    m_roundState = RoundState.BeginPreStartRound;
                }
            }
            
            [Server]
            private void BeginPreStartRound()
            {
                _preStartRoundTimer = m_roundStats.m_preStartRoundTimer;
                
                ResetPlayers();
                RepopulatePlayers();
                RespawnProps();
                RpcBroadcastSkin();
                RpcActivePlayerPanels(_playersAlive.Count);
                
                if (!_soundsWaitingRoom)
                {
                    _soundsWaitingRoom = true;
                    ClearWaitingRoom();
                }
                Invoke(nameof(SendStartRoundAnim),2f);
                
                m_roundState = RoundState.PreStartRound;
            }
            
            [Server]
            private void PreStartRound()
            {
                _preStartRoundTimer -= Time.deltaTime;
                RpcBroadcastCommunication("Round begins in:");
                RpcBroadcastTimer($"{_preStartRoundTimer:F2}");
                if (_preStartRoundTimer <= 0)
                {
                    m_roundState = RoundState.StartRound;
                }
            }
            
            [Server]
            public void StartRound()
            {
                _currentRound++;
                _roundTimer = m_roundStats.m_maxRoundTime;
                StartCombatMusic();
                
                m_roundState = RoundState.RoundLive;

            }
            
            
            [Server]
            public void RoundLive()
            {
                _roundTimer -= Time.deltaTime;
                RpcBroadcastCommunication($"Round {_currentRound} ends in");
                RpcBroadcastTimer($"{_roundTimer:F2}");
                CheckWinners();
            
                if (_roundTimer <= 0)
                {
                    m_roundState = RoundState.BeginPreEndRound;
                }
            }
            
            [Server]
            public void BeginPreRoundBreak()
            {
                _preRoundBreakTimer = m_roundStats.m_preRoundBreakTimer;
                m_roundState = RoundState.PreRoundBreak;
            }

            [Server]
            public void PreRoundBreak()
            {
                _preRoundBreakTimer -= Time.deltaTime;
                if (_preRoundBreakTimer <= 0)
                {
                    m_roundState = RoundState.RoundBreak;
                }
                    
            }
            
            [Server]
            public void RoundBreak()
            {
                m_roundState = RoundState.RoundLive;
            }

            [Server]
            private void BeginPreEndRound()
            {
                _preEndRoundTimer = m_roundStats.m_preEndRoundTimer;

                if (_playersAlive.Count > 1)
                {
                    RpcBroadcastMatchNull();
                    RpcBroadcastTimer("");
                }
                
                StopCombatMusic();
                SendEndRoundAnim();
                m_roundState = RoundState.PreEndRound;

            }
            
            [Server]
            private void PreEndRound()
            {
                _preEndRoundTimer -= Time.deltaTime;
                if (_preEndRoundTimer <= 0)
                {
                    m_roundState = RoundState.EndRound;
                }
            }
            
            
            [Server]
            public void EndRound()
            {
                Debug.Log("Round ended!");
                
                if (_currentRound >= m_roundStats.m_maxRounds)
                {
                    m_roundState = RoundState.BeginPreEndMatch;
                    return;
                }

                m_roundState = RoundState.BeginPreStartRound;
            }

            [Server]
            public void BeginPreEndMatch()
            {
                _preEndMatchTimer = m_roundStats.m_preEndMatchTimer;
                _matchWinner = _playersAlive.OrderByDescending(p => p.m_roundsWon).First();
                m_roundState = RoundState.PreEndMatch;
            }

            [Server]
            public void PreEndMatch()
            {
                _preEndMatchTimer -= Time.deltaTime;
                RpcBroadcastCommunication($"Match won by {_matchWinner.m_playerName}");
                RpcBroadcastTimer("");
                
                if (_preEndMatchTimer <= 0)
                {
                    m_roundState = RoundState.EndMatch;
                }
            }

            public void EndMatch()
            {
                Reset();
                m_roundState = RoundState.BeginPreQuitMatch;
            }
            
            [Server]
            public void BeginPreQuitMatch()
            {
                _preQuitMatchTimer = m_roundStats.m_preQuitMatchTimer;
                m_roundState = RoundState.PreQuitMatch;
            }
            
            [Server]
            public void PreQuitMatch()
            {
                _preQuitMatchTimer -= Time.deltaTime;
                RpcBroadcastCommunication("Quit match in");
                RpcBroadcastTimer($"{_preQuitMatchTimer:F2}");
                if (_preQuitMatchTimer <= 0)
                {
                    m_roundState = RoundState.QuitMatch;
                }
            }
        
            
            [Server]
            private void QuitMatch()
            {
                
                NetworkManager.singleton.StopHost();
            }
            
        #endregion
        
        
        #region Utils
        
            [Server]
            private void RpcBroadcastSkin()
            {
                foreach (var player in _players)
                {
                    player.m_skinBehaviour.ApplySkin();
                }
            }

            [Server]
            private void RespawnProps()
            {
                if (_safetyCounter > 10) return;
                _safetyCounter++;
                _healthBoxSystem.Reset();
                _healthBoxSystem.RestartCycle();
                _mysteryBoxSystem.Reset();
                _mysteryBoxSystem.SpawnBox();
            }
        
            [Server]
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
                    player.m_combatSFX.StopCombatMusic(player.netIdentity.connectionToClient);
                }
            }

            private void Reset()
            {
                _currentRound = 0;
                _players.Clear();
                _playersAlive.Clear();
            }
        
        
        private void ForEachTimerText(string message)
        {
            foreach (var text in m_roundPanelTimer)
            {
                text.text = message;
            }
        }
        private void ForEachCommText(string message)
        {
            foreach (var text in m_roundPanelCommunication)
            {
                text.text = message;
            }
        }
        private void CheckWinners()
        {
            if (_playersAlive.Count == 1)
            {
                SetRoundWinner();
                //RpcBroadcastMatchNull();
            }
        }

        [ClientRpc]
        private void RpcBroadcastCommunication(string message)
        {
            ForEachCommText(message);
        }

        [ClientRpc]
        private void RpcBroadcastTimer(string message)
        {
            ForEachTimerText(message);
        }
        
        [ClientRpc]
        private void RpcBroadcastMatchNull()
        {
            var message = "Match Null";
            ForEachCommText(message);
        }
        
        [ClientRpc]
        private void RpcBroadcastLoser(string loserName)
        {
            Debug.Log($"[CLIENT RPC] Broadcast loser: {loserName}");
            var message = "Disconnected : " + loserName;
            ForEachCommText(message);
        }


        [ClientRpc]
        private void RpcBroadcastWinner(string winnerName)
        {
            var message = "Winner : " + winnerName;
            ForEachCommText(message);
        }

        [ClientRpc]
        private void RpcBroadcastWinnerPoints(int[] panelIndexes, int points)
        {
            
            foreach (int index in panelIndexes)
            {
                if (index >= 0 && index < m_roundPlayerPanels.Count)
                {
                    var panel = m_roundPlayerPanels[index];
                    var text = panel.GetComponentInChildren<TMPro.TMP_Text>();
                    text.text = points.ToString();
                }
            }
        }
        
        [Server]
        private List<int> GetPanelIndexesForPlayer(RoundPlayer winner)
        {
            var indexes = new List<int>();

            if (m_roundPlayerPanelsLink.TryGetValue(winner, out var panels))
            {
                for (int i = 0; i < panels.Count; i++)
                {
                    // On récupère l'index du panel dans ta liste globale m_roundPlayerPanels
                    int panelIndex = m_roundPlayerPanels.IndexOf(panels[i]);
                    if (panelIndex >= 0)
                        indexes.Add(panelIndex);
                }
            }

            return indexes;
        }
        
        
        [Client]
        private void ClearBreak()
        {
            //_roundBreak = false;
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

        [Server]
        private void StartCombatMusic()
        {
            foreach (var player in _players)
            {
                
                player.m_combatSFX.StartCombatMusic(player.netIdentity.connectionToClient,_currentRound-1);
            }
        }

        [Server]
        private void StopCombatMusic()
        {
            foreach (var player in _players)
            {
                player.m_combatSFX.StopCombatMusic(player.netIdentity.connectionToClient);
            }
        }
        
        #endregion

        #region Player Management

        private readonly SyncList<RoundPlayer> _players = new SyncList<RoundPlayer>();
        private SyncList<RoundPlayer> _playersAlive = new SyncList<RoundPlayer>();

        
        [Server]
        public void RegisterPlayer(RoundPlayer player)
        {
            if (!_players.Contains(player))
            {
                _players.Add(player);
                AddRoundPlayerPanel(player);
            }
            if (!_playersAlive.Contains(player))
            {
                _playersAlive.Add(player);
            }
            _waitForPlayerTimer = m_roundStats.m_waitForPlayerTimer;
            //_isWaitingForPlayers = true;
            
            string name;
                if(_playersAlive.Count == 1)
                  name = "Host";
                else name = "Player Client " + (_playersAlive.Count - 1);
                
           //NetworkIdentity identity = player.GetComponent<NetworkIdentity>();
           //player.RpcSetPlayerName(identity.connectionToClient,name);

           player.m_playerName = name;
           AssignSkin(player,_players.Count-1);

        }

        [Server]
        public void AddRoundPlayerPanel(RoundPlayer player)
        {
            var playerPanels = new List<GameObject>();

            int index = _players.Count - 1;

            var panel1 = m_roundPlayerPanels[index];
            var panel2 = m_roundPlayerPanels[index + 4];

            playerPanels.Add(panel1);
            playerPanels.Add(panel2);

            m_roundPlayerPanelsLink[player] = playerPanels;
            /*
            RpcActivePannels(index);
            RpcActivePannels(index + 4);
            */
        }

        [Server]
        public void AssignSkin(RoundPlayer player, int skinIndex)
        {
            player.m_skinIndex = skinIndex;
        }
        
        
        [ClientRpc]
        public void RpcActivePannels(int index)
        {
            m_roundPlayerPanels[index].SetActive(true);
        }

        [ClientRpc]
        public void RpcActivePlayerPanels(int numberOfPlayers)
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                m_roundPlayerPanels[i].SetActive(true);
                m_roundPlayerPanels[i+4].SetActive(true);
            }
        }
        [Server]
        public void UnregisterPlayer(RoundPlayer player)
        {
            if (_players.Contains(player))
                _players.Remove(player);
        }
        
        [TargetRpc]
        public void TargetRpcSendLoserToSpectate(NetworkConnectionToClient target,RoundPlayer player)
        {
            var playerXr = player.gameObject.GetComponentInChildren<XROrigin>();
            playerXr.transform.position = _spectateArea.position;
            player.RestoreVision();
            player.m_isInputActive = true;
            
        }
        
        #endregion

        #region Privates
        
        [SerializeField] HealthBoxSystem _healthBoxSystem;
        [SerializeField] MisteryBoxSystem _mysteryBoxSystem;
        [SerializeField] Transform _spectateArea;
        
        [SyncVar] private RoundPlayer _winnerPlayer;
        [SyncVar] private RoundPlayer _matchWinner;

        //[SyncVar] private bool _isWaitingForPlayers = true;
        [SyncVar] private float _waitForPlayerTimer;
        
        //[SyncVar] private bool _isPreStartingRound = false;
        [SyncVar] private float _preStartRoundTimer;
        
        [SyncVar] private float _preRoundBreakTimer;
        [SyncVar] private float _preEndRoundTimer;
        
        [SyncVar] private float _preEndMatchTimer;
        
        
        //[SyncVar] private bool _roundStarted = false;
        //[SyncVar] private bool _roundBreak = false;
        [SyncVar] private float _roundTimer;
        [SyncVar] private int _currentRound = 0;
        [SyncVar] private float _broadCastCurrentTimer;
        
        //[SyncVar] private bool _endMatch = false;
        [SyncVar] private float _preQuitMatchTimer;
        
        [SerializeField] private List<TMP_Text> m_roundPanelCommunication;
        [SerializeField] private List<TMP_Text> m_roundPanelTimer;
        [SerializeField] private List<GameObject> m_roundPlayerPanels = new List<GameObject>();
        private Dictionary<RoundPlayer,List<GameObject>> m_roundPlayerPanelsLink = new Dictionary<RoundPlayer, List<GameObject>>();
        
        private bool _soundsWaitingRoom = false;


        private int _safetyCounter;

        #endregion
    }
}
