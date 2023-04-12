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
        //per ogni casa
        for (int i = 0; i < giftAmountInLvl;  i++)
        {
            //per ogni regalo che deve avere la casa, genera un regalo (2 cicli innestati)
            Gift g = Spawn(navMeshCtrl.GetRandomLocation());
            g.Init(new GiftData { destination = null, SlowedAfterPickup = slowAfterPickup });
        }
    }

    internal void GifCollected(Gift gift)
    {
        LevelController.I.GetPoolManager().RetrievePoollable(gift);
    }

    Gift Spawn(Vector3 spawnPos)
    {
        return LevelController.I.GetPoolManager().GetFirstAvaiableObject<Gift>(spawnPos);
    }
}
