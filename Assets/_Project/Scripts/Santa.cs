using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Santa : PoolObjectBase, IMooveAndInteract
{
    [SerializeField]
    Image selected;
    NavMeshAgent agent;
    LineRenderer lineRenderer;

    List<movementInformation> queuedActions = new List<movementInformation>();

    public void Setup(float _unitSpeed)
    {
        selected.enabled = false;
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        if (!lineRenderer)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        agent.speed = _unitSpeed;
    }

    private void LateUpdate()
    {
        if (agent && agent.hasPath)
        { 
            DrawPath(); 
        }
        else
        {
            if(queuedActions.Count > 0)
            {
                SetAgentDestination(queuedActions[0]);
                queuedActions.RemoveAt(0);
            }
        }
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

    public void OnAction(IDestination destination, bool _isQueuedAction = false)
    {
        if (agent != null)
        {
            Vector3 interactablePosition = destination.GetDestinationPosition();
            movementInformation movement = new movementInformation { position = interactablePosition, interactable = destination };
            if (_isQueuedAction)
            {
                queuedActions.Add(movement);
            }
            else
            {
                if (queuedActions.Count > 0)
                {
                    queuedActions.Clear();
                }
                SetAgentDestination(movement);
            }
        }
    }

    public void OnAction(Vector3 moveTo, bool _isQueueAction = false)
    {
        if (agent != null)
        {
            movementInformation movement = new movementInformation { position = moveTo, interactable = null };

            if (_isQueueAction)
            {
                queuedActions.Add(movement);
            }
            else
            {
                if(queuedActions.Count > 0)
                {
                    queuedActions.Clear();
                }
                SetAgentDestination(movement);
            }
        }
    }

    void SetAgentDestination(movementInformation _movement)
    {
        if(_movement.interactable != null)
        {
            lineRenderer.material.color = Color.cyan;
        }
        else
        {
            lineRenderer.material.color = Color.green;
        }
        agent.SetDestination(_movement.position);
    }

    void DrawPath()
    {
        int corners = agent.path.corners.Length;
        lineRenderer.positionCount = corners;
        lineRenderer.SetPosition(0, transform.position);

        if (corners < 2)
        {
            return;
        }

        for (int i = 0; i < corners; i++)
        {
            Vector3 point = new Vector3(agent.path.corners[i].x, agent.path.corners[i].y, agent.path.corners[i].z);
            lineRenderer.SetPosition(i, point);
        }
    }

    struct movementInformation
    {
        public Vector3 position;
        public IInteractable interactable;
    }
}
