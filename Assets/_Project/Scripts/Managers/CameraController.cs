using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float movementSpeed = 10;
    [SerializeField]
    float rotationSpeed = 10;

    [SerializeField]
    float cameraAnimationSpeed = 1.5f;

    [SerializeField]
    Camera overlaycam;
    Transform camTransform;
    Camera cam;
    bool isMouseRotationEnable;
    Vector2 mouseRotation;

    Transform tacticalView;
    Vector3 origianlLocation;
    Quaternion origianlRotation;
    MatrixBlender blender;

    public void Setup(bool _isRTSViewEnabled, MatrixBlender _blender)
    {
        
        tacticalView = FindObjectsOfType<Transform>().First(x => x.tag == "TacticalView");
        cam = Camera.main;
        camTransform = cam.GetComponent<Transform>();
        UpdateMouseActivationStatus();
        blender = _blender;
        overlaycam.enabled = false;
        SavePreviousPositionAndRotation();
        SwitchView(_isRTSViewEnabled);
    }

    public void EnteringGameplay(bool _isRTSViewEnabled)
    {
        UpdateMouseActivationStatus(!_isRTSViewEnabled);
        SwitchView(_isRTSViewEnabled);
    }

    public void DetectInput()
    {
        // Movement input
        if (Input.GetKey(KeyCode.W))
        {
            camTransform.localPosition += (camTransform.forward * Time.deltaTime * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            camTransform.localPosition -= (camTransform.forward * Time.deltaTime * movementSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            camTransform.localPosition += (camTransform.right * Time.deltaTime * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            camTransform.localPosition -= (camTransform.right * Time.deltaTime * movementSpeed);
        }

        // Altitude input
        if (Input.GetKey(KeyCode.E))
        {
            camTransform.localPosition += (Vector3.up * Time.deltaTime * movementSpeed);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            camTransform.localPosition -= (Vector3.up * Time.deltaTime * movementSpeed);
        }

        // Mouse rotation
        if (isMouseRotationEnable)
        {
            mouseRotation.x += Input.GetAxis("Mouse X") * rotationSpeed;
            mouseRotation.y -= Input.GetAxis("Mouse Y") * rotationSpeed;

            mouseRotation.y = Mathf.Clamp(mouseRotation.y, -90, 90);

            camTransform.localRotation = Quaternion.Euler(mouseRotation.y, mouseRotation.x, 0);
        }
    }

    /// <summary>
    /// Cambio visuale della videocamera
    /// </summary>
    /// <param name="_isRtsView"></param>
    public void SwitchView(bool _isRtsView)
    {
        if (_isRtsView)
        {
            UpdateMouseActivationStatus();
            blender.BlendToMatrix(MatrixBlender.OrthoMatrix, 1.5f);
            SavePreviousPositionAndRotation();
            camTransform.DOMove(tacticalView.position, cameraAnimationSpeed).OnComplete(() => { overlaycam.enabled = true; });
            camTransform.DORotateQuaternion(tacticalView.rotation, cameraAnimationSpeed);
        }
        else
        {
            overlaycam.enabled = false;

            if(transform.position != camTransform.position)
            {
                camTransform.DOMove(origianlLocation, cameraAnimationSpeed).OnComplete(() => { UpdateMouseActivationStatus(); });
                camTransform.DORotateQuaternion(origianlRotation, cameraAnimationSpeed);
            }
            else
            {
                UpdateMouseActivationStatus();
            }

            blender.BlendToMatrix(MatrixBlender.PerspectiveMatrix, 1f);
        }

    }

    void SavePreviousPositionAndRotation()
    {
        origianlLocation = camTransform.localPosition;
        origianlRotation = camTransform.rotation;
    }

    /// <summary>
    /// Aggiorna lo stato del mouse in base al tipo dicontrollo attuale
    /// </summary>
    void UpdateMouseActivationStatus(bool? _forceStatus = null)
    {
        isMouseRotationEnable = _forceStatus != null ? _forceStatus.Value : (!LevelController.I.GetInputController().isRTSView) && LevelController.I.IsGameplay;
    }

}
