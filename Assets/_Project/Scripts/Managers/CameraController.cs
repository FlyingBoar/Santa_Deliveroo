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
    Vector2 mouseRotation;

    Transform tacticalView;
    Vector3 origianlLocation;
    Quaternion origianlRotation;

    public void Setup()
    {
        tacticalView = FindObjectsOfType<Transform>().First(x => x.tag == "TacticalView");
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
            transform.localPosition += (transform.up * Time.deltaTime * movementSpeed);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.localPosition -= (transform.up * Time.deltaTime * movementSpeed);
        }

        // Mouse rotation
        mouseRotation.x += Input.GetAxis("Mouse X") * rotationSpeed;
        transform.localRotation = Quaternion.Euler(0, mouseRotation.x, 0);
    }

    public void SwitchView(bool _isRtsView)
    {
        if (_isRtsView)
        {
            origianlLocation = transform.localPosition;
            origianlRotation = transform.rotation;
            transform.DOMove(tacticalView.position, 1.5f);
            transform.DORotateQuaternion(tacticalView.rotation, 1.5f);
        }
        else
        {
            transform.DORotateQuaternion(origianlRotation, 1.5f);
            transform.DOMove(origianlLocation, 1.5f);
        }

    }

}
