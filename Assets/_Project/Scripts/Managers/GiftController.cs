using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GiftController : MonoBehaviour
{
    [SerializeField]
    Gift GiftPrefab;
    NavMeshController navMeshCtrl;

    public void Init()
    {
        navMeshCtrl = LevelController.I.GetNavMeshCtrl();
        
        //per ogni casa
        for(int i = 0; i < 5;  i++)
        {
            //per ogni regalo che deve avere la casa, genera un regalo (2 cicli innestati)
            Spawn(navMeshCtrl.GetRandomLocation());
        }
    }

    void Spawn(Vector3 spawnPos)
    {
        //Gift newGift = Instantiate(GiftPrefab, spawnPos, Quaternion.identity);
        Gift newGift = LevelController.I.GetPoolManager().GetFirstAvaiableObject<Gift>(spawnPos);
    }
}
