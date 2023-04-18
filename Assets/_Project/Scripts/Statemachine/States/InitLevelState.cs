using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitLevelState : StateMachineBehaviour
{
    LevelController LC;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (LC == null)
            LC = LevelController.I;


        LC.Init();
        LC.GetGiftController().Init();
        LC.GetRTSController().Init(LC.GetDataManager().GetCurrentLevelData());
        LC.GetHouseController().Init(LC.GetDataManager().GetCurrentLevelData());
        LC.GetEnemiesController().Init(LC.GetDataManager().GetCurrentLevelData());
        LC.GetUIManager().Init(LC.GetDataManager().GetCurrentLevelData());
        LC.GetInputController().EnteringGameplay();
        LC.GetCameraController().ResetCameraPosition();

        LC.GoToNext();
    }
}
