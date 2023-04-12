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
    List<GiftData> giftCollected = new List<GiftData>();

    IDestination currentDestination = null;

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

    public void RemoveGifts(List<GiftData> _giftToRemove)
    {
        foreach (var item in _giftToRemove)
        {
            giftCollected.Remove(item);
        }
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
            OnAction(interactablePosition, _isQueuedAction, destination);
        }
    }

    public void OnAction(Vector3 moveTo, bool _isQueueAction = false, IDestination destination = null)
    {
        if (agent != null)
        {
            currentDestination = destination;
            movementInformation movement = new movementInformation { position = moveTo, interactable = destination };

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

    /// <summary>
    /// Add the given gift to collected gift if not already collected
    /// </summary>
    /// <param name="data"></param>
    public void CollectGift(GiftData data)
    {
        if(giftCollected.Contains(data))
        {
            return;
        }

        giftCollected.Add(data);
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: questo controllo deve essere effettuato sia per i regali che per le abitazioni
        IDestination d = other.GetComponentInParent<IDestination>();
        if(d != null && d == currentDestination)
        {
            d.AgentOnDestination(this);
        }
    }

    public List<GiftData> GetCollectedGifts()
    {
        return giftCollected;
    }

    void SelectedUnit(bool _isSelected)
    {
        selected.enabled = _isSelected;
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
        public IDestination interactable;
    }
}
