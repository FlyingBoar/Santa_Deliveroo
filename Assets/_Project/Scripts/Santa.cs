using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Santa : MonoBehaviour, IMooveAndInteract
{
    [SerializeField]
    Image selected;
    NavMeshAgent agent;
    public void Setup()
    {
        selected.enabled = false;
        agent = GetComponent<NavMeshAgent>();
    }

    void SelectedUnit(bool _isSelected)
    {
        selected.enabled = _isSelected;
    }

    public void OnClickOver()
    {
        SelectedUnit(true);
    }

    public void OnDeselect()
    {
        SelectedUnit(false);
    }

    public void OnAction(IInteractable interactable = null)
    {
        
    }

    public void OnAction(Vector3 moveTo)
    {
        if(agent != null) 
        {
            agent.destination = moveTo;
        }
    }
}
