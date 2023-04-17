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

    /// <summary>
    /// Restituisce il livello corrente, se non è stato selezionato carica il primo della lista dei livelli
    /// </summary>
    /// <returns></returns>
    public LevelData GetCurrentLevelData()
    {
        if(currentLevelData == null)
            currentLevelData = allLevels.FirstOrDefault();
        return currentLevelData;
    }

    /// <summary>
    /// Setta il livello che deve essere caricato nel livello
    /// </summary>
    /// <param name="_levelData"></param>
    public void SetCurrentLevelData( LevelData _levelData)
    {
        currentLevelData = _levelData;
    }

    /// <summary>
    /// Restituisce tutti i livelli ordinati in base al loro ID
    /// </summary>
    /// <returns></returns>
    public List<LevelData> GetAllLevels()
    {
        return allLevels.OrderBy(y => y.level).ToList();
    }

    /// <summary>
    /// Carica il livello successivo a quello corrente, se non ci riesce restituisce lo stesso livello
    /// </summary>
    public void LoadNextLevel() 
    {
        LevelData data = allLevels.OrderBy(y => y.level).FirstOrDefault(x => x.level > currentLevelData.level);
        if(data != null)
            currentLevelData = data;
    }
}
