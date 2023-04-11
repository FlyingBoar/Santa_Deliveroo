using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class Destination : MonoBehaviour, IDestination
{
    Transform destinationPoint;

    private void Start()
    {
        destinationPoint = GetComponentsInChildren<Transform>().First(x => x.tag == "HouseDestination");
    }

    public Vector3 GetDestinationPosition()
    {
        return destinationPoint.position;
    }

    public void OnClickOver()
    {
        // Highlight + Get informations
    }

    public void OnDeselect()
    {
        // rimuove Highlight + informations
    }
}
