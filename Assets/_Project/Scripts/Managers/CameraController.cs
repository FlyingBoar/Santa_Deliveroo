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

    Transform camTransform;

    bool isMouseRotationEnable;
    Vector2 mouseRotation;

    Transform tacticalView;
    Vector3 origianlLocation;
    Quaternion origianlRotation;

    public void Setup(bool _isRTSViewEnabled)
    {
        tacticalView = FindObjectsOfType<Transform>().First(x => x.tag == "TacticalView");
        camTransform = Camera.main.GetComponent<Transform>();
        UpdateMouseActivationStatus();
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
    /// Camera animation for the game mode
    /// </summary>
    /// <param name="_isRtsView"></param>
    public void SwitchView(bool _isRtsView)
    {
        if (_isRtsView)
        {
            UpdateMouseActivationStatus();
            origianlLocation = camTransform.localPosition;
            origianlRotation = camTransform.rotation;
            camTransform.DOMove(tacticalView.position, cameraAnimationSpeed);
            camTransform.DORotateQuaternion(tacticalView.rotation, cameraAnimationSpeed);
        }
        else
        {
            camTransform.DOMove(origianlLocation, cameraAnimationSpeed).OnComplete(() => { UpdateMouseActivationStatus(); });
            camTransform.DORotateQuaternion(origianlRotation, cameraAnimationSpeed);
        }

    }

    /// <summary>
    /// Update the status of the mouse related to the current game mode (RTS or Fly)
    /// </summary>
    void UpdateMouseActivationStatus()
    {
        isMouseRotationEnable = !LevelController.I.GetInputController().isRTSView;
    }

}
