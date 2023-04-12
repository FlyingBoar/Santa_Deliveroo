using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Level", menuName = "NewLevel")]
public class LevelData : ScriptableObject
{
    [Header("Level")]
    public int ActiveHouses;
    public int MinScoreToWin;
    public float LevelTimerSec;
    [Header("Ememy")]
    public float EnemySpeed;
    public int ActiveEnemies;
    [Header("Units")]
    public int UnitsInLevel;
    public float SantaSpeed;
    public float SantaSlowedAfterPickup;
}
