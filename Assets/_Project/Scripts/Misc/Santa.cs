using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Santa : PoolObjectBase, IMooveAndInteract
{
    [SerializeField]
    SpriteRenderer selected;
    NavMeshAgent agent;
    LineRenderer lineRenderer;

    List<movementInformation> queuedActions = new List<movementInformation>();
    List<GiftData> giftCollected = new List<GiftData>();
    bool isUnitSelected;
    IDestination currentDestination = null;

    private void Update()
    {
        if (agent && agent.hasPath)
        {
            DrawPath();
        }
        else
        {
            if (queuedActions.Count > 0)
            {
                SetAgentDestination(queuedActions[0]);
                queuedActions.RemoveAt(0);
            }
        }
    }

    #region API

    public void Init(float _unitSpeed)
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

        LevelController.I.GetRTSController().OnMassiveDeselect += OnDeselect;
    }

    public override void OnRetrieve()
    {
        LevelController.I.GetRTSController().OnMassiveDeselect -= OnDeselect;
    }

    /// <summary>
    /// Rimuove i regali passati come paratro dalla lista di regali posseduti dall'unit�
    /// </summary>
    /// <param name="_giftToRemove"></param>
    public void RemoveGifts(List<GiftData> _giftToRemove)
    {
        foreach (var item in _giftToRemove)
        {
            giftCollected.Remove(item);
        }
        UpdateSpeed();
        UpdateLinkedDestinations();
    }

    /// <summary>
    /// Azioni al click sull'unit�
    /// </summary>
    public void OnSelect(bool _directSelectin = true)
    {
        SelectedUnit(true);
        if (_directSelectin)
        {
            if(giftCollected.Count > 0)
            {
                UpdateLinkedDestinations();
            }
        }
    }

    /// <summary>
    /// Azioni durante il deselect
    /// </summary>
    public void OnDeselect()
    {
        SelectedUnit(false);
    }

    /// <summary>
    /// Muove l'unit� verso una destinazione interattiva
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="_isQueuedAction"></param>
    public void OnAction(IDestination destination, bool _isQueuedAction = false)
    {
        if (agent != null)
        {
            Vector3 interactablePosition = destination.GetDestinationPosition();
            OnAction(interactablePosition, _isQueuedAction, destination);
        }
    }

    /// <summary>
    /// Muove l'unit� verso una destinazione, salva nella lista di azioni in coda se _isQueueAction � vera
    /// </summary>
    /// <param name="moveTo"></param>
    /// <param name="_isQueueAction"></param>
    /// <param name="destination"></param>
    public void OnAction(Vector3 moveTo, bool _isQueueAction = false, IDestination destination = null)
    {
        if (agent != null)
        {
            movementInformation movement = new movementInformation { position = moveTo, interactable = destination };

            if (_isQueueAction)
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

    /// <summary>
    /// Aggiunge il regalo passato come parametro alla lista di regali se non presente
    /// </summary>
    /// <param name="gift"></param>
    public void CollectGift(Gift gift)
    {
        if (giftCollected.Contains(gift.GetGiftData()))
        {
            return;
        }

        giftCollected.Add(gift.GetGiftData());
        UpdateSpeed();
        UpdateLinkedDestinations();
    }

    /// <summary>
    /// Restituisce la lista dei regali raccolti
    /// </summary>
    /// <returns></returns>
    public List<GiftData> GetCollectedGifts()
    {
        return giftCollected;
    }
    #endregion

    /// <summary>
    /// Se la destinazione con cui entra in contatto � la destinazione corrente, la destinazione viene avvisata
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        IDestination d = other.GetComponentInParent<IDestination>();
        if (d != null && d == currentDestination)
        {
            d.AgentOnDestination(this);
        }
    }

    /// <summary>
    /// Setta lo stato della grafica che indica se l'unit� � selezionata
    /// </summary>
    /// <param name="_isSelected"></param>
    void SelectedUnit(bool _isSelected)
    {
        isUnitSelected = _isSelected;
        selected.enabled = _isSelected;
    }

    /// <summary>
    /// Setta la destinazione dell'unit� modificando il colore del linerenderer
    /// </summary>
    /// <param name="_movement"></param>
    void SetAgentDestination(movementInformation _movement)
    {
        if (_movement.interactable != null)
        {
            lineRenderer.material.color = Color.cyan;
        }
        else
        {
            lineRenderer.material.color = Color.green;
        }
        currentDestination = _movement.interactable;
        agent.SetDestination(_movement.position);
    }

    /// <summary>
    /// Diresegna il path del movimento corrente
    /// </summary>
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

    /// <summary>
    /// Aggiorna la velocit� del navmesh
    /// </summary>
    void UpdateSpeed()
    {
        agent.speed = LevelController.I.LevelData.SantaSpeed - giftCollected.Sum(x => x.SlowedAfterPickup);
    }

    void UpdateLinkedDestinations()
    {
        if(isUnitSelected)
            LevelController.I.GetHouseController().SetHighlight(giftCollected);
    }

    /// <summary>
    /// Struttura contenente le informazioni del movimento
    /// </summary>
    struct movementInformation
    {
        public Vector3 position;
        public IDestination interactable;
    }
}