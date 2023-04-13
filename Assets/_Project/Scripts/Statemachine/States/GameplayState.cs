using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayState : StateMachineBehaviour
{
    LevelController LC;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (LC == null)
            LC = LevelController.I;

        //LC.GetUIManager().ChangeMenu(UIManager.MenuType.Gameplay);
        LC.GetGiftController().Init();
        LC.GetRTSController().Init(LC.LevelData);
        LC.GetHouseController().Init(LC.LevelData);
        LC.IsGameplayStatus();
    }
}