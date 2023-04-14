using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    List<Destination> allDestinations = new List<Destination>();

    GiftController giftController;

    public void Setup(GiftController _giftController)
    {
        allDestinations = FindObjectsOfType<Destination>().ToList();
        giftController = _giftController;

        foreach (Destination destination in allDestinations)
        {
            destination.Setup();
        }

        DisalbeAllDestinations();
    }

    /// <summary>
    /// Operazioni dell'inizio livello.
    /// Set del numero di regali per le destinazioni e delle stesse attive nel livello
    /// </summary>
    /// <param name="_currentLevel"></param>
    public void Init(LevelData _currentLevel)
    {
        // Determina il numero di regali per ogni casa in base ai minimi punti necessari per il livello
        int giftPerHouse = (_currentLevel.MinScoreToWin + _currentLevel.ActiveHouses - 1 ) / _currentLevel.ActiveHouses;
        // Sceglie le case che devono essere attive per il livello
        for (int i = 0; i < _currentLevel.ActiveHouses; i++)
        {
            var inactiveDestinations = allDestinations.Where(x => x.IsDestinationActive == false).ToList();
            inactiveDestinations[Random.Range(0, inactiveDestinations.Count)].SetDestinationisActive(true);
        }

        // Per ogni casa attiva richiede la creazione dei regali associati
        var activeHouses = allDestinations.Where(x => x.IsDestinationActive == true).ToList();
        for (int j = 0; j < activeHouses.Count(); j++)
        {
            for (int k = 0; k < giftPerHouse; k++)
            {
                var gift = giftController.SpawnGiftOnRandomLocation(activeHouses[j]);
                activeHouses[j].AddGiftToDeliverable(gift);
            }
        }
    }

    public void SetHighlight(List<GiftData> gifts)
    {
        foreach (var destination in allDestinations.Where(x => x.isActiveAndEnabled == true)) 
        {
            if (destination.ChkDestinationContainGift(gifts))
                destination.OnSelect(false);
            else
                destination.OnDeselect();
        }
    }

    public void SetHighlight(GiftData gifts)
    {
        foreach (var destination in allDestinations.Where(x => x.isActiveAndEnabled == true))
        {
            if (destination.ChkDestinationContainGift(gifts))
                destination.OnSelect(false);
            else
                destination.OnDeselect();
        }
    }

    /// <summary>
    /// Disabilita tutte le icone delle destinazioni
    /// </summary>
    void DisalbeAllDestinations()
    {
        foreach (var destination in allDestinations)
        {
            destination.SetDestinationisActive(false);
        }
    }
}
