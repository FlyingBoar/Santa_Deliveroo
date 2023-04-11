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

    bool isMouseRotationEnable;
    Vector2 mouseRotation;

    Transform tacticalView;
    Vector3 origianlLocation;
    Quaternion origianlRotation;

    public void Setup(bool _isRTSViewEnabled)
    {
        tacticalView = FindObjectsOfType<Transform>().First(x => x.tag == "TacticalView");
        isMouseRotationEnable = !_isRTSViewEnabled;
    }

    public void DetectInput()
    {
        // Movement input
        if (Input.GetKey(KeyCode.W))
        {
            transform.localPosition += (transform.forward * Time.deltaTime * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.localPosition -= (transform.forward * Time.deltaTime * movementSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.localPosition += (transform.right * Time.deltaTime * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.localPosition -= (transform.right * Time.deltaTime * movementSpeed);
        }

        // Altitude input
        if (Input.GetKey(KeyCode.E))
        {
            transform.localPosition += (Vector3.up * Time.deltaTime * movementSpeed);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.localPosition -= (Vector3.up * Time.deltaTime * movementSpeed);
        }

        // Mouse rotation
        if (isMouseRotationEnable)
        {
            mouseRotation.x += Input.GetAxis("Mouse X") * rotationSpeed;
            mouseRotation.y -= Input.GetAxis("Mouse Y") * rotationSpeed;
            transform.localRotation = Quaternion.Euler(mouseRotation.y, mouseRotation.x, 0); 
        }
    }

    public void SwitchView(bool _isRtsView)
    {
        if (_isRtsView)
        {
            isMouseRotationEnable = false;
            origianlLocation = transform.localPosition;
            origianlRotation = transform.rotation;
            transform.DOMove(tacticalView.position, 1.5f);
            transform.DORotateQuaternion(tacticalView.rotation, 1.5f);
        }
        else
        {
            transform.DOMove(origianlLocation, 1.5f).OnComplete(() => { isMouseRotationEnable = true; });
            transform.DORotateQuaternion(origianlRotation, 1.5f);
        }

    }

}
