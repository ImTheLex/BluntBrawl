using UnityEngine;

namespace Animation.Runtime
{
    public class AutonomousAnimation : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            animator.SetBool("StartRound",false);
            animator.SetBool("EndRound",false);
            animator.SetBool("Win",false);
            animator.SetBool("Lose",false);
            animator.SetBool("RecoverDash", false);
        }
    }
}
