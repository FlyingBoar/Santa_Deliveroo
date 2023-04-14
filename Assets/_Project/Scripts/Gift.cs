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
    GiftData data;
    [SerializeField]
    SpriteRenderer highlight;
    

    public void Init(GiftData _myData)
    {
        SetGiftData(_myData);
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

    public Vector3 GetDestinationPosition()
    {
        return transform.position;
    }

    public void AgentOnDestination(Santa _agent)
    {
        _agent.CollectGift(this);
        LevelController.I.GetGiftController().GifCollected(this);
    }

    public GiftData GetGiftData()
    {
        return data;
    }

    public void SetGiftData(GiftData _data)
    {
        _data.Gift = this;
        data = _data;
    }
}

public class GiftData
{
    public Destination Destination;
    public Gift Gift;
    public float SlowedAfterPickup;
}
