using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Numerics;

public class RTSController : MonoBehaviour
{
    List<Santa> allUnits = new List<Santa>();
    Camera cam;
    IInteractable selectedUnit = null;

    private static readonly int _INTERACTABLE_LAYER = 1 << 6;
    private static readonly int _NAVMESH_LAYER = 1 << 7;

    // Start is called before the first frame update
    public void Setup()
    {
        cam = Camera.main;
        allUnits = FindObjectsOfType<Santa>().ToList();

        // Non più necessario
        foreach (Santa unit in allUnits)
        {
            unit.Setup();
        }
    }

    public void DetectInput()
    {
        if (Input.GetMouseButton(0))
        {
            OnLeftClickActions();
        }
        if (Input.GetMouseButton(1))
        {
            bool isQueued = false;
            if (Input.GetKey(KeyCode.LeftControl))
            {
                isQueued = true;
            }
            OnRightClickActions(isQueued);
        }
    }

    public Santa GetFirstAvailableAgent() 
    {
        return allUnits.First();
    }


    /// <summary>
    /// Actions to perform on left click
    /// </summary>
    void OnLeftClickActions()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50);

        if (selectedUnit != null)
        {
            selectedUnit.OnDeselect();
            selectedUnit = null;
        }

        if (Physics.Raycast(ray, out hit, 500.0f, _INTERACTABLE_LAYER))
        {
            IInteractable interactable = hit.collider.GetComponentInChildren<IInteractable>() != null ? hit.collider.GetComponentInChildren<IInteractable>() : hit.collider.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                selectedUnit = interactable;
                selectedUnit.OnClickOver();
            }
        }
    }

    /// <summary>
    /// Actions to perform on right click
    /// </summary>
    void OnRightClickActions(bool _isQueued)
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50);

        // Check se è stato colpito un oggetto interattivo
        if (Physics.Raycast(ray, out hit, 500.0f, _INTERACTABLE_LAYER))
        {
            IDestination destination = hit.collider.GetComponent<IDestination>() != null ? hit.collider.GetComponent<IDestination>() : hit.collider.GetComponentInParent<IDestination>();

            if (selectedUnit != null)
            {
                if (destination != null)
                {
                    // sposta l'unità sulla destinazione (pacco o casa)
                    (selectedUnit as IMooveAndInteract).OnAction(destination, _isQueued);
                }
            }
        }
        // Se viene colpita la superficie del navmesh
        else if(Physics.Raycast(ray, out hit, 500.0f, _NAVMESH_LAYER))
        {
            if (selectedUnit != null)
            {
                (selectedUnit as IMooveAndInteract).OnAction(hit.point,_isQueued);
            }
        }

    }
}
