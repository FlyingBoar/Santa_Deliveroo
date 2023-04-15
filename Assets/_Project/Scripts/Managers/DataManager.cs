using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager
{
    List<LevelData> allLevels = new List<LevelData>();
    LevelData currentLevelData;

    public void Setup()
    {
        allLevels = Resources.LoadAll<LevelData>("Data").ToList();
    }

    public LevelData GetCurrentLevelData()
    {
        if(currentLevelData == null)
            currentLevelData = allLevels.FirstOrDefault();
        return currentLevelData;
    }

    public void SetCurrentLevelData( LevelData _levelData)
    {
        currentLevelData = _levelData;
    }

    public List<LevelData> GetAllLevels()
    {
        return allLevels.OrderBy(y => y.level).ToList();
        //return allLevels;
    }

    public void LoadNextLevel() 
    {
        LevelData data = allLevels.OrderBy(y => y.level).FirstOrDefault(x => x.level > currentLevelData.level);
        if(data != null)
            currentLevelData = data;
    }
}
