using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSceneState : StateMachineBehaviour
{
    LevelController LC;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (LC == null)
            LC = LevelController.I;

        LC.GetGiftController().DeInit();
        LC.GetRTSController().DeInit();
        LC.GetHouseController().DeInit();
        LC.GetEnemiesController().DeInit();

        LC.GoToNext();
    }
}
