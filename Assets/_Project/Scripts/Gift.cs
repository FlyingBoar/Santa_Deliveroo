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
    [SerializeField]
    SpriteRenderer highlight;

    public void Init(GiftData _myData)
    {
        data = _myData;
    }

    public void OnClickOver()
    {
        // highlight + mostra informazioni in UI
        highlight.enabled = true;
    }

    public void OnDeselect()
    {
        // Rimuove highlight
        highlight.enabled = false;
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
