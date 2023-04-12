using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class SetupState : StateMachineBehaviour
{
    LevelController LC;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (LC == null)
            LC = LevelController.I;

        LC.SetPoolManager(FindObjectOfType<PoolManager>());
        LC.SetInputController(FindObjectOfType<PlayerInputController>());
        LC.SetCameraController(FindObjectOfType<CameraController>());
        LC.SetRtsController(FindObjectOfType<RTSController>());
        LC.SetNavMeshController(FindObjectOfType<NavMeshController>());
        LC.SetGiftController(FindObjectOfType<GiftController>());

        LC.GetPoolManager().Setup();
        LC.GetInputController().Setup(LC.GetCameraController(), LC.GetRTSController());
        LC.GetCameraController().Setup(LC.GetInputController().isRTSView);
        LC.GetRTSController().Setup();
        LC.GetNavMeshCtrl().Setup();
        
        LC.GoToNext();
    }

}
