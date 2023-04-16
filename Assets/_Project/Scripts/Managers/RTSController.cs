using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.EventSystems;

public class RTSController : MonoBehaviour
{
    List<Santa> allUnits = new List<Santa>();
    List<Vector3> spawnPositions = new List<Vector3>();
    Camera cam;

    public Action OnMassiveDeselect;

    IInteractable selectedUnit = null;

    private static readonly int _TACTICALVIEW_LAYER = 1 << 3;
    private static readonly int _NAVMESH_LAYER = 1 << 7;

    #region API

    /// <summary>
    /// called to setup the main function of the class
    /// </summary>
    public void Setup()
    {
        cam = Camera.main;
    }

    /// <summary>
    /// called at the start of the level
    /// </summary>
    /// <param name="_levData"></param>
    public void Init(LevelData _levData)
    {
        spawnPositions.Clear();
        spawnPositions = GameObject.FindGameObjectsWithTag("UnitSpawn").Select(x => x.GetComponent<Transform>().position).ToList();
        SpawnUnits(_levData);
    }

    /// <summary>
    /// Distrugge logicamente tutte le unità spawnate in scena
    /// </summary>
    public void DeInit()
    {
        foreach (Santa unit in allUnits)
        {
            DestroyUnit(unit, true);
        }

        allUnits.Clear();
    }

    public void UnitHitByEnemy(Santa _santa)
    {
        LevelController.I.GetGiftController().SpawnGiftOnLocation(_santa.transform.position, _santa.GetCollectedGifts());
        allUnits.Remove(_santa);
        DestroyUnit(_santa);
    }

    public bool StillUnitInLevel()
    {
        return allUnits.Any();
    }

    public bool UnitAreCarryingGifts()
    {
        return allUnits.Any(x => x.GetCollectedGifts().Count > 0);
    }

    public void ShowUnitGiftInformations(List<GiftData> gifts)
    {
        LevelController.I.GetUIManager().GetGameplayPanel().UpdateGiftContainer(gifts);
    }

    public void HideGiftInformations()
    {
        LevelController.I.GetUIManager().GetGameplayPanel().DisableGiftcontainer();
    }
    #endregion

    #region Input
    /// <summary>
    /// Function to intercept the input of the user when in RTS mode
    /// </summary>
    public void DetectInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftClickActions();
        }
        if (Input.GetMouseButtonDown(1))
        {
            bool isQueued = false;
            if (Input.GetKey(KeyCode.LeftControl))
            {
                isQueued = true;
            }
            OnRightClickActions(isQueued);
        }
    }

    /// <summary>
    /// Actions to perform on left click
    /// </summary>
    void OnLeftClickActions()
    {
        // Se viene clickato un oggetto in UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50);

        OnMassiveDeselect?.Invoke();

        if (selectedUnit != null)
        {
            selectedUnit.OnDeselect();
            selectedUnit = null;
        }

        if (Physics.Raycast(ray, out hit, 500.0f, _TACTICALVIEW_LAYER))
        {
            IInteractable interactable = hit.collider.GetComponentInChildren<IInteractable>() != null ? hit.collider.GetComponentInChildren<IInteractable>() : hit.collider.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                interactable.OnSelect();
                selectedUnit = interactable;
            }
        }
    }

    /// <summary>
    /// Actions to perform on right click
    /// </summary>
    void OnRightClickActions(bool _isQueued)
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 50);

        // Check se è stato colpito un oggetto interattivo
        if (Physics.Raycast(ray, out hit, 500.0f, _TACTICALVIEW_LAYER))
        {
            IDestination destination = hit.collider.GetComponent<IDestination>() != null ? hit.collider.GetComponent<IDestination>() : hit.collider.GetComponentInParent<IDestination>();

            if (selectedUnit != null)
            {
                if (destination != null)
                {
                    // sposta l'unità sulla destinazione (pacco o casa)
                    (selectedUnit as IMooveAndInteract).OnAction(destination, _isQueued);
                }
            }
        }
        // Se viene colpita la superficie del navmesh
        else if (Physics.Raycast(ray, out hit, 500.0f, _NAVMESH_LAYER))
        {
            var santa = (selectedUnit as IMooveAndInteract);
            if (santa != null)
            {
                santa.OnAction(hit.point, _isQueued);
            }
        }

    }

    #endregion

    /// <summary>
    /// Spawn the unitys on the map
    /// </summary>
    /// <param name="_levData"></param>
    void SpawnUnits(LevelData _levData)
    {
        for (int i = 0; i < _levData.UnitsInLevel; i++)
        {
            Vector3 pos = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)];
            if (pos != null)
            {
                Santa unit = LevelController.I.GetPoolManager().GetFirstAvaiableObject<Santa>(transform, pos);
                unit.Init(_levData.SantaSpeed, this);
                allUnits.Add(unit);
            }
        }
    }

    /// <summary>
    /// Pulisce i dati salvati nell'unità spawnata, la rimuove dalla lista di unità attive e la restituisce al pooler
    /// </summary>
    /// <param name="_unitToDestroy"></param>
    void DestroyUnit(Santa _unitToDestroy, bool _isLevelReset = false)
    {
        _unitToDestroy.ClearUnitData();
        LevelController.I.GetPoolManager().RetrievePoollable(_unitToDestroy);
        if (!_isLevelReset)
            LevelController.I.CheckGameStatus();
    }
}
