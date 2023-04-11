using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    bool isRTSView = false;
    CameraController cameraCtrl;
    RTSController rtsCtrl;

    // Start is called before the first frame update
    public void Setup(CameraController _cameraCtrl, RTSController _rtsCtrl)
    {
        cameraCtrl = _cameraCtrl;
        rtsCtrl = _rtsCtrl;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchView();
        }

        if (!isRTSView)
        {
            cameraCtrl.DetectInput();
        }
        else
        {
            rtsCtrl.DetectInput();
        }

    }

    void SwitchView()
    {
        isRTSView = !isRTSView;

        cameraCtrl.SwitchView(isRTSView);
    }
}