using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GiftController : MonoBehaviour
{
    NavMeshController navMeshCtrl;
    float slowAfterPickup;

    public void Init()
    {
        navMeshCtrl = LevelController.I.GetNavMeshCtrl();

        int giftAmountInLvl = LevelController.I.LevelData.MinScoreToWin;
        slowAfterPickup = LevelController.I.LevelData.SantaSlowedAfterPickup;
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
        return g.GetGiftData();
    }

    /// <summary>
    /// Restituisce l'oggetto regalo al pooler
    /// </summary>
    /// <param name="gift"></param>
    public void GifCollected(Gift gift)
    {
        LevelController.I.GetPoolManager().RetrievePoollable(gift);
    }

}
