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
        LC.SetHouseController(FindObjectOfType<HouseController>());
        LC.SetMatrixBlender(FindObjectOfType<MatrixBlender>());
        LC.SetEnemiesController(FindObjectOfType<EnemiesController>());
        LC.SetUIManager(FindObjectOfType<UIManager>());

        LC.GetPoolManager().Setup();
        LC.GetInputController().Setup(LC.GetCameraController(), LC.GetRTSController());
        LC.GetCameraController().Setup(LC.GetInputController().isRTSView, LC.GetMatrixBlender());
        LC.GetRTSController().Setup();
        LC.GetNavMeshCtrl().Setup();
        LC.GetHouseController().Setup(LC.GetGiftController());
        LC.GetMatrixBlender().Setup();
        LC.GetUIManager().Setup();

        LC.GoToNext();
    }

}
