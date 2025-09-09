using System;
using Mirror;
using UnityEngine;

namespace Rounds.Runtime
{
    public class RoundPlayer : NetworkBehaviour
    {
        public int m_playerCurrentHealth;
        public string m_playerName;
        
        private RoundSystem roundSystem;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
             roundSystem = FindFirstObjectByType<RoundSystem>();
             if(roundSystem) CmdAddPlayer();
        }

        [Command(requiresAuthority = false)]
        public void CmdAddPlayer()
        {
            roundSystem.RegisterPlayer(this);
        }
        
        //[Command(requiresAuthority = false)]
        public void CmdSetDefeat()
        {
            if(!isLocalPlayer) return;
            //roundSystem.EndRound();
            roundSystem.SetRoundLoser(this);
        }
        
        public void CmdSetCurrentPlayerHealth(int currentHealth)
        {
            m_playerCurrentHealth = currentHealth;
        }

        [Command(requiresAuthority = false)]
        public void CmdSetPlayerName(string name)
        {
            m_playerName = name;
        }
    }
}
