using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

public class Santa : PoolObjectBase, IMooveAndInteract
{
    [SerializeField]
    SpriteRenderer selected;
    [SerializeField] int maxCollectableGifts = 5;
    NavMeshAgent agent;
    LineRenderer lineRenderer;
    
    List<movementInformation> queuedActions = new List<movementInformation>();
    List<GiftData> collectedGifts = new List<GiftData>();
    bool isUnitSelected;
    bool isUnitActive;
    RTSController controller;
    
    IDestination currentDestination = null;

    private void Update()
    {
        if (!isUnitActive)
            return;

        if (agent && agent.hasPath)
        {
            DrawPath();
        }
        else
        {
            if (queuedActions.Count > 0)
            {
                SetAgentDestination(queuedActions[0]);
                LevelController.I.GetPoolManager().RetrievePoollable(queuedActions[0].lineRenderer);
                queuedActions.RemoveAt(0);
            }
        }
    }

    #region API

    public void Init(float _unitSpeed, RTSController _controller)
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
        var orienter = GetComponentInChildren<ObjectOrienter>();
        if (orienter)
        {
            orienter.Init(Camera.main.transform);
        }

        controller = _controller;
        isUnitActive = true;
        agent.speed = _unitSpeed;
        LevelController.I.GetRTSController().OnMassiveDeselect += OnDeselect;
    }

    public override void OnRetrieve()
    {
        LevelController.I.GetRTSController().OnMassiveDeselect -= OnDeselect;
        isUnitActive = false;
    }

    /// <summary>
    /// Rimuove i regali passati come paratro dalla lista di regali posseduti dall'unità
    /// </summary>
    /// <param name="_giftToRemove"></param>
    public void RemoveGifts(List<GiftData> _giftToRemove)
    {
        foreach (var item in _giftToRemove)
        {
            collectedGifts.Remove(item);
        }
        UpdateSpeed();
        //UpdateLinkedDestinations();
        UpdateUIGiftInformations();
    }

    /// <summary>
    /// Azioni al click sull'unità
    /// </summary>
    public void OnSelect(bool _directSelectin = true)
    {
        SelectedUnit(true);
        if (_directSelectin)
        {
            if(collectedGifts.Count > 0)
            {
                //UpdateLinkedDestinations();
                UpdateUIGiftInformations();
            }
        }
    }

    /// <summary>
    /// Azioni durante il deselect
    /// </summary>
    public void OnDeselect()
    {
        if(isUnitSelected)
            controller.HideGiftInformations();
        SelectedUnit(false);
    }

    /// <summary>
    /// Muove l'unità verso una destinazione interattiva
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
    /// Muove l'unità verso una destinazione, salva nella lista di azioni in coda se _isQueueAction è vera
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
                QueueLineRenderer lr = LevelController.I.GetPoolManager().GetFirstAvaiableObject<QueueLineRenderer>();
                movement.lineRenderer = lr;
                queuedActions.Add(movement);
                if (lr != null) 
                    DrawQueuedPath(lr);
            }
            else
            {
                if (queuedActions.Count > 0)
                {
                    ClearActionQueued();
                }
                SetAgentDestination(movement);
            }
        }
    }

    /// <summary>
    /// Aggiunge il regalo passato come parametro alla lista di regali se non presente
    /// </summary>
    /// <param name="gift"></param>
    public bool CollectGift(Gift gift)
    {
        if(collectedGifts.Count >= maxCollectableGifts || collectedGifts.Contains(gift.GetGiftData()))
        {
            return false;
        }
        collectedGifts.Add(gift.GetGiftData());
        UpdateSpeed();
        //UpdateLinkedDestinations();
        UpdateUIGiftInformations();
        return true;
    }

    /// <summary>
    /// Restituisce la lista dei regali raccolti
    /// </summary>
    /// <returns></returns>
    public List<GiftData> GetCollectedGifts()
    {
        return collectedGifts;
    }

    public void ClearUnitData()
    {
        OnDeselect();
        collectedGifts.Clear();
        lineRenderer.positionCount = 0;
        ClearActionQueued();
    }

    public bool IsAgentSelected()
    {
        return isUnitSelected;
    }
    #endregion

    /// <summary>
    /// Se la destinazione con cui entra in contatto è la destinazione corrente, la destinazione viene avvisata
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
    /// Setta lo stato della grafica che indica se l'unità è selezionata
    /// </summary>
    /// <param name="_isSelected"></param>
    void SelectedUnit(bool _isSelected)
    {
        isUnitSelected = _isSelected;
        selected.enabled = _isSelected;
    }

    /// <summary>
    /// Setta la destinazione dell'unità modificando il colore del linerenderer
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
        DrawPathForLineRenderer(lineRenderer, transform.position, agent.path.corners);
    }

    void DrawQueuedPath(QueueLineRenderer _line)
    {
        _line.SetColor(Color.yellow);
        for (int i = 0; i < queuedActions.Count; i++)
        {
            Vector3 startPos;
            if (i == 0)
            {
                startPos = agent.destination;
            }
            else
            {
                startPos = queuedActions[i - 1].position;
            }

            NavMeshPath path = LevelController.I.GetNavMeshCtrl().GetPathToPoint(startPos, queuedActions[i].position);
            if(path != null)
            {
                DrawPathForLineRenderer(_line.GetLineRenderer(), startPos, path.corners);
            }
            else
            {
                LevelController.I.GetPoolManager().RetrievePoollable(_line);
            }
        }
    }

    void DrawPathForLineRenderer(LineRenderer _line, Vector3 _startPosition ,Vector3[] _cornerMatrix)
    {
        int _corners = _cornerMatrix.Length;
        _line.positionCount = _corners;
        _line.SetPosition(0, _startPosition);

        if (_corners < 2)
        {
            return;
        }

        for (int i = 0; i < _corners; i++)
        {
            Vector3 point = new Vector3(_cornerMatrix[i].x, _cornerMatrix[i].y, _cornerMatrix[i].z);
            _line.SetPosition(i, point);
        }
    }

    /// <summary>
    /// Aggiorna la velocità del navmesh
    /// </summary>
    void UpdateSpeed()
    {
        agent.speed = LevelController.I.GetDataManager().GetCurrentLevelData().SantaSpeed - collectedGifts.Sum(x => x.SlowedAfterPickup);
    }

    void UpdateUIGiftInformations()
    {
        if(isUnitSelected)
            controller.ShowUnitGiftInformations(GetCollectedGifts());
    }

    void ClearActionQueued()
    {
        foreach (var item in queuedActions)
        {
            LevelController.I.GetPoolManager().RetrievePoollable(item.lineRenderer);
        }
        queuedActions.Clear();
    }

    /// <summary>
    /// Struttura contenente le informazioni del movimento
    /// </summary>
    struct movementInformation
    {
        public Vector3 position;
        public IDestination interactable;
        public QueueLineRenderer lineRenderer;
    }
}
