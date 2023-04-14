using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Destination : MonoBehaviour, IDestination
{
    Transform destinationPoint;
    public bool IsDestinationActive { get; private set; }
    
    [SerializeField]
    SpriteRenderer MapIcon;
    
    [SerializeField]
    SpriteRenderer highlight;
    BoxCollider spriteBoxCollider;

    List<GiftData> giftsToBeDelivered = new List<GiftData>();

    #region API
    public void Setup()
    {
        destinationPoint = GetComponentsInChildren<Transform>().First(x => x.tag == "HouseDestination");
        MapIcon = GetComponentInChildren<SpriteRenderer>();
        spriteBoxCollider = MapIcon.GetComponent<BoxCollider>();
    }

    public Vector3 GetDestinationPosition()
    {
        return destinationPoint.position;
    }

    public void OnSelect()
    {
        // highlight + mostra informazioni in UI
        highlight.enabled = true;
    }

    public void OnDeselect()
    {
        // Rimuove highlight
        highlight.enabled = false;
    }

    public void AgentOnDestination(Santa _agent)
    {
        List<GiftData> droppedGifts = GetRightGiftsFromList(_agent.GetCollectedGifts());
        if (droppedGifts.Count > 0)
        {
            LevelController.I.AddVictoryPoints(droppedGifts.Count);
            _agent.RemoveGifts(droppedGifts);
        }
    }

    /// <summary>
    /// Setta la destinazione come attiva durante il livello
    /// </summary>
    /// <param name="_isActive"></param>
    public void SetDestinationisActive(bool _isActive)
    {
        IsDestinationActive = _isActive;
        MapIcon.enabled = _isActive;
        spriteBoxCollider.enabled = _isActive;
    }

    /// <summary>
    /// Aggiunge il regalo passato alla lista dei regali da consegnare
    /// </summary>
    /// <param name="_gift"></param>
    public void AddGiftToDeliverable(GiftData _gift)
    {
        giftsToBeDelivered.Add(_gift);
    }

    #endregion

    /// <summary>
    /// Controlla se nella lista di Gift passata sono presenti dei regali corretti
    /// </summary>
    /// <param name="_gifts"></param>
    /// <param name="giftDropped"></param>
    /// <returns></returns>
    List<GiftData> GetRightGiftsFromList(List<GiftData> _gifts)
    {
        // controlla se nella lista ci sono regali che appartengono a questa casa
        List<GiftData> giftDropped = new List<GiftData>();
        foreach (var item in _gifts)
        {
            if (giftsToBeDelivered.Contains(item))
            {
                giftDropped.Add(item);
                giftsToBeDelivered.Remove(item);
            }
        }
        return giftDropped;
    }

}
