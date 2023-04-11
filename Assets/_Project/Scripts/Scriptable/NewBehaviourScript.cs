using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Level", menuName = "NewLevel")]
public class NewBehaviourScript : ScriptableObject
{
    public int ActiveHouses;
    public int ActiveEnemies;
    public float SantaSpeed;
    public float SantaSlowedAfterPickup;
    public float EnemySpeed;
}
