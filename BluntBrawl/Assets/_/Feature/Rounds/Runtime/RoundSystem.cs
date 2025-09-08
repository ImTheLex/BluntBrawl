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

            if (_roundStarted)
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
        }
        
        [Server]
        public void EndRound()
        {
            Debug.Log("Round ended!");
            _roundStarted = false;
            _roundBreak = true;
        }

        [Server]
        public void CheckWinner()
        {
            Debug.Log("Check winner...");
        }

        #endregion

        #region Utils

        private void UpdateRoundTimer(float oldValue, float newValue)
        {
            foreach (var text in m_texts)
            {
                text.text = "Time\n" + newValue.ToString("F2");
            }
        }

        #endregion

        #region Player Management

        // Liste des joueurs maintenue par le serveur
        private readonly SyncList<GameObject> _players = new SyncList<GameObject>();

        [Server]
        public void RegisterPlayer(GameObject player)
        {
            if (!_players.Contains(player))
                _players.Add(player);
        }

        [Server]
        public void UnregisterPlayer(GameObject player)
        {
            if (_players.Contains(player))
                _players.Remove(player);
        }

        #endregion

        #region Privates

        [SyncVar] private bool _roundStarted = false;
        [SyncVar] private bool _roundBreak = false;
        [SyncVar(hook = nameof(UpdateRoundTimer))] private float _roundTimer;
        [SerializeField] private List<TMP_Text> m_texts;

        #endregion
    }
}
