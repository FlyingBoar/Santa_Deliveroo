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

    public void AgentOnDestination(Santa _agent)
    {
        List<GiftData> droppedGifts;
        if (CheckGift(_agent.GetCollectedGifts(), out droppedGifts) && droppedGifts.Count > 0)
        {
            int pointToScore = droppedGifts.Count;
            LevelController.I.AddVictoryPoints(pointToScore);
            _agent.RemoveGifts(droppedGifts);
        }
    }

    bool CheckGift(List<GiftData> _gifts, out List<GiftData> giftDropped)
    {
        // controlla se nella lista ci sono regali che appartengono a questa casa
        giftDropped = new List<GiftData>();
        foreach (var item in _gifts)
        {
            giftDropped.Add(item);
        }
        return true;
    }
}
