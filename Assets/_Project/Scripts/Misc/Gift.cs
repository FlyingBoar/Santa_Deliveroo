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
        LevelController.I.GetRTSController().OnMassiveDeselect += OnDeselect;
    }

    public override void OnRetrieve()
    {
        LevelController.I.GetRTSController().OnMassiveDeselect -= OnDeselect;
    }

    public void OnSelect(bool _directSelectin = true)
    {
        // highlight + mostra informazioni in UI
        highlight.enabled = true;
        if(_directSelectin)
        {
            LevelController.I.GetHouseController().SetHighlight(GetGiftData());
        }
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
        if (_agent.CollectGift(this)){
            LevelController.I.GetGiftController().GifCollected(this);
        }
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
