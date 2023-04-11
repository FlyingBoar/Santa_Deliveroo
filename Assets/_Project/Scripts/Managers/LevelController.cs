using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    #region Managers
    PlayerInputController inputCtrl;
    public PlayerInputController GetInputController
    {
        get { return inputCtrl; }
    }

    CameraController cameraCtrl;
    public CameraController GetCameraController
    {
        get { return cameraCtrl; }
    }

    RTSController rtsCtrl;
    public RTSController GetRTSController
    {
        get { return rtsCtrl; }
    }

    NavMeshController navMeshCtrl;
    public NavMeshController GetNavMeshCtrl
    {
        get { return navMeshCtrl; }
    } 

    GiftController giftCtrl;
    public GiftController GetGiftController
    {
        get { return giftCtrl; }
    }

    PoolManager poolManager;
    public PoolManager GetPoolManager
    {
        get { return poolManager; }
    }
    #endregion


    public static LevelController I;

    private void Awake()
    {
        if (I == null)
            I = this;
        else
            DestroyImmediate(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        poolManager = GetComponent<PoolManager>();
        inputCtrl = GetComponent<PlayerInputController>();
        cameraCtrl = GetComponent<CameraController>();
        rtsCtrl = GetComponent< RTSController>();
        navMeshCtrl = GetComponent<NavMeshController>();
        giftCtrl = GetComponent<GiftController>();
        
        poolManager.Setup();
        inputCtrl.Setup(cameraCtrl, rtsCtrl);
        rtsCtrl.Setup();
        navMeshCtrl.Setup();
        cameraCtrl.Setup(GetInputController.isRTSView);
        giftCtrl.Setup();
    }
}
