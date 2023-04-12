using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Gift : PoolObjectBase, IDestination
{
    GiftData data { get; set; }

    public void Init(GiftData _myData)
    {
        data = _myData;
    }

    public void OnClickOver()
    {
        // highlight + mostra informazioni in UI
    }

    public void OnDeselect()
    {
        // Rimuove highlight
    }

    public Vector3 GetDestinationPosition()
    {
        return transform.position;
    }

    public void AgentOnDestination(Santa _agent)
    {
        _agent.CollectGift(data);
        LevelController.I.GetGiftController().GifCollected(this);
    }
}

public class GiftData
{
    public Destination destination;
    public float SlowedAfterPickup;
}
