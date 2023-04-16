using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    List<Vector3> spawnPositions = new List<Vector3>();
    List<Befana> allUnits = new List<Befana>();

    /// <summary>
    /// called at the start of the level
    /// </summary>
    /// <param name="_levData"></param>
    public void Init(LevelData _levData)
    {
        spawnPositions.Clear();
        spawnPositions = GameObject.FindGameObjectsWithTag("EnemySpawn").Select(x => x.GetComponent<Transform>().position).ToList();
        SpawnUnits(_levData);
    }

    /// <summary>
    /// Ritorna tutte le unità al pooler
    /// </summary>
    public void DeInit()
    {
        foreach (var unit in allUnits) 
        { 
            LevelController.I.GetPoolManager().RetrievePoollable(unit);
        }

        allUnits.Clear();
    }

    public void EnemyHitUnit(Befana _befana, Santa _santa)
    {
        LevelController.I.GetPoolManager().RetrievePoollable(_befana);
        LevelController.I.GetRTSController().UnitHitByEnemy(_santa);
        //LevelController.I.CheckGameStatus();
    }

    /// <summary>
    /// Spawn the unitys on the map
    /// </summary>
    /// <param name="_levData"></param>
    void SpawnUnits(LevelData _levData)
    {
        for (int i = 0; i < _levData.ActiveEnemies; i++)
        {
            Vector3 pos = spawnPositions[Random.Range(0, spawnPositions.Count)];
            if (pos != null)
            {
                Befana unit = LevelController.I.GetPoolManager().GetFirstAvaiableObject<Befana>(transform, pos);
                unit.Init(_levData.EnemySpeed);
                allUnits.Add(unit);
            }
        }
    }
}
