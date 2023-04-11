using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    PlayerInputController inputCtrl;
    CameraController cameraCtrl;
    RTSController rtsCtrl;

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
        inputCtrl = FindObjectOfType<PlayerInputController>();
        cameraCtrl = FindObjectOfType<CameraController>();
        rtsCtrl = FindObjectOfType< RTSController>();

        inputCtrl.Setup(cameraCtrl, rtsCtrl);
        rtsCtrl.Setup();
        cameraCtrl.Setup(GetInputController.isRTSView);
    }


    public CameraController GetCameraController
    {
        get { return cameraCtrl; }
    }

    public PlayerInputController GetInputController
    {
        get { return inputCtrl; }
    }

    public RTSController GetRTSController
    {
        get { return rtsCtrl; }
    }
}
