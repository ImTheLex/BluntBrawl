using Mirror;
using UnityEngine;

namespace Rounds.Runtime
{
    public class RoundPlayer : NetworkBehaviour
    {
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
            roundSystem.RegisterPlayer(gameObject);
        }
        
        [Command(requiresAuthority = false)]
        public void SetDefeat()
        {
            roundSystem.EndRound();
        }
    }
}
