using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GiftController : MonoBehaviour
{
    NavMeshController navMeshCtrl;
    float slowAfterPickup;

    List<Gift> spawnedGifts = new List<Gift>();

    public void Init()
    {
        navMeshCtrl = LevelController.I.GetNavMeshCtrl();
        slowAfterPickup = LevelController.I.GetDataManager().GetCurrentLevelData().SantaSlowedAfterPickup;
    }

    /// <summary>
    /// Restituisce l'unità al pooler e pulisce la lista dei Gift presenti nel livello
    /// </summary>
    public void DeInit()
    {
        foreach (Gift gift in spawnedGifts)
        {
            LevelController.I.GetPoolManager().RetrievePoollable(gift);
        }

        spawnedGifts.Clear();
    }

    /// <summary>
    /// Spawna un regalo e lo associa  alla destinazione che viene passata
    /// </summary>
    /// <param name="_destination"></param>
    public GiftData SpawnGiftOnRandomLocation(Destination _destination)
    {
        return SpawnGiftOnLocation(navMeshCtrl.GetRandomLocation(), _destination);
    }

    /// <summary>
    /// Spawna il regalo nella posizione indicata
    /// </summary>
    /// <param name="_spawnPosition"></param>
    /// <param name="_destination"></param>
    public GiftData SpawnGiftOnLocation(Vector3 _spawnPosition, Destination _destination)
    {
        // TODO: funzione da chiamare alla morte dell'unità Santa
        Gift g = LevelController.I.GetPoolManager().GetFirstAvaiableObject<Gift>(_spawnPosition);
        GiftData data = new GiftData { Destination = _destination, SlowedAfterPickup = slowAfterPickup };
        g.Init(data);
        spawnedGifts.Add(g);
        return g.GetGiftData();
    }

    public GiftData SpawnGiftOnLocation(Vector3 _spawnPosition, GiftData _data)
    {
        // TODO: funzione da chiamare alla morte dell'unità Santa
        Gift g = LevelController.I.GetPoolManager().GetFirstAvaiableObject<Gift>(_spawnPosition);
        g.Init(_data);
        spawnedGifts.Add(g);
        return g.GetGiftData();
    }

    public void SpawnGiftOnLocation(Vector3 _spawnPosition, List<GiftData> _gifts)
    {
        foreach (var data in _gifts)
        {
            Vector2 location = UnityEngine.Random.insideUnitCircle * 10f;
            Vector3 pos = new Vector3(_spawnPosition.x - location.x, _spawnPosition.y, _spawnPosition.z - location.y);
            SpawnGiftOnLocation(pos, data);
        }
    }

    /// <summary>
    /// Restituisce l'oggetto regalo al pooler
    /// </summary>
    /// <param name="gift"></param>
    public void GifCollected(Gift gift)
    {
        LevelController.I.GetPoolManager().RetrievePoollable(gift);
        spawnedGifts.Remove(gift);
        LevelController.I.CheckGameStatus();
    }

    public void SetHighlight(Destination _destination)
    {
        foreach (var gift in spawnedGifts)
        {
            if(gift.GetGiftData().Destination == _destination)
                gift.OnSelect(false);
            else
                gift.OnDeselect();
        }   
    }

    public bool StillGiftsInLevel()
    {
        return spawnedGifts.Any();
    }
}
