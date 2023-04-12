using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : StateMachineBehaviour
{
    LevelController LC;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (LC == null)
            LC = LevelController.I;

        //GM.GetUIManager().ChangeMenu(UIManager.MenuType.MainMenu);
        LC.GoToNext(); // TODO: da rimuovere, il cambio deve stare sul menù
    }
}