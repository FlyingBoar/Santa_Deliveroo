using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : StateMachineBehaviour
{
    LevelController LC;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (LC == null)
            LC = LevelController.I;

        LC.GetUIManager().ChangeMenu(UIManager.MenuType.Pause);
        LC.LeavingGameplay();
        Time.timeScale = 0;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Time.timeScale = 1;
    }
}
