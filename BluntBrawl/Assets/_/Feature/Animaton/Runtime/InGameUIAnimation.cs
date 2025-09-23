using UnityEngine;

namespace Animation.Runtime
{
    public class InGameUIAnimation : MonoBehaviour
    {
        #region Main Methods

        [ContextMenu("Send Startround")]
        public void SendStartRound()=> _animator.SetBool("StartRound", true);
        
        
        [ContextMenu("Send EndRound")]
        public void SendEndRound() => _animator.SetBool("EndRound", true);
        
        [ContextMenu("Send Win")]
        public void SendWin() => _animator.SetBool("Win", true);
        
        [ContextMenu("Send Lose")]
        public void SendLose() => _animator.SetBool("Lose", true);
        
        #endregion
        
        
        #region Private and Protected

        private Animator _animator => GetComponent<Animator>();

        #endregion
    }
}
