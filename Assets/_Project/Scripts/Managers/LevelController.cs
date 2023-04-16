using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    PlayerInputController inputCtrl;
    CameraController cameraCtrl;
    RTSController rtsCtrl;
    NavMeshController navMeshCtrl;
    GiftController giftCtrl;
    PoolManager poolManager;
    HouseController houseCtrl;
    MatrixBlender blender;
    EnemiesController enemiesCtrl;
    UIManager UIManager;
    DataManager dataManager;

    #region Get & Set

    // ------------------------ GET ----------------------------- \\

    /// <summary>
    /// Ritorna il riferimento al controllore degli input
    /// </summary>
    /// <returns></returns>
    public PlayerInputController GetInputController()
    {
        return I.inputCtrl;
    }

    /// <summary>
    /// Ritorna il riferimento al controllore della camera
    /// </summary>
    /// <returns></returns>
    public CameraController GetCameraController()
    {
        return I.cameraCtrl;
    }

    /// <summary>
    /// Ritorna il riferimento al controllore della modalità RTS
    /// </summary>
    /// <returns></returns>
    public RTSController GetRTSController()
    {
        return I.rtsCtrl;
    }

    /// <summary>
    /// Ritorna il riferimento al controllore del nav mesh
    /// </summary>
    /// <returns></returns>
    public NavMeshController GetNavMeshCtrl()
    {
        return I.navMeshCtrl;
    }

    /// <summary>
    /// Ritorna il riferimento al controllore dei Gift
    /// </summary>
    /// <returns></returns>
    public GiftController GetGiftController()
    {
        return I.giftCtrl;
    }

    /// <summary>
    /// Ritorna il riferimento al Pool manager
    /// </summary>
    /// <returns></returns>
    public PoolManager GetPoolManager()
    {
        return I.poolManager;
    }


    /// <summary>
    /// Ritorna il riferimento al Pool manager
    /// </summary>
    /// <returns></returns>
    public HouseController GetHouseController()
    {
        return I.houseCtrl;
    }

    /// <summary>
    /// Ritorna il riferimento al blender per la proiezione della camera
    /// </summary>
    /// <returns></returns>
    public MatrixBlender GetMatrixBlender()
    {
        return I.blender;
    }

    /// <summary>
    /// Ritorna il riferimento al controllore delle befane
    /// </summary>
    /// <returns></returns>
    public EnemiesController GetEnemiesController()
    {
        return I.enemiesCtrl;
    }

    /// <summary>
    /// Ritorna il riferimento allo UIManager
    /// </summary>
    /// <returns></returns>
    public UIManager GetUIManager()
    {
        return I.UIManager;
    }

    /// <summary>
    /// Restituisce il riferimento al manager dei dati
    /// </summary>
    /// <returns></returns>
    public DataManager GetDataManager()
    {
        return I.dataManager;
    }

    //// ------------------------ SET ----------------------------- \\

    /// <summary>
    /// Setta il riferimento al controller degli input
    /// </summary>
    /// <param name="_inputController"></param>
    public void SetInputController(PlayerInputController _inputController)
    {
        I.inputCtrl = _inputController;
    }
    /// <summary>
    /// Setta il riferimento al controller della camera
    /// </summary>
    /// <param name="_cameraController"></param>
    public void SetCameraController(CameraController _cameraController)
    {
        I.cameraCtrl = _cameraController;
    }

    /// <summary>
    /// Setta il riferimento al controller della modalità RTS
    /// </summary>
    /// <param name="_rtsCtrl"></param>
    public void SetRtsController(RTSController _rtsCtrl)
    {
        I.rtsCtrl = _rtsCtrl;
    }

    /// <summary>
    /// Setta il riferimento al controller del nav mesh
    /// </summary>
    /// <param name="_navMeshController"></param>
    public void SetNavMeshController(NavMeshController _navMeshController)
    {
        I.navMeshCtrl = _navMeshController;
    }

    /// <summary>
    /// Setta il riferimento al controller dei Gift
    /// </summary>
    /// <param name="_giftController"></param>
    public void SetGiftController(GiftController _giftController)
    {
        I.giftCtrl = _giftController;
    }

    /// <summary>
    /// Setta il riferimento al pool manager
    /// </summary>
    /// <param name="_poolManager"></param>
    public void SetPoolManager(PoolManager _poolManager)
    {
        I.poolManager = _poolManager;
    }

    /// <summary>
    /// Setta il riferimento all'house controller
    /// </summary>
    /// <param name="_houseController"></param>
    public void SetHouseController(HouseController _houseController)
    {
        I.houseCtrl = _houseController;
    }

    /// <summary>
    /// Setta il riferimento al blender della prospettiva per la camera
    /// </summary>
    /// <param name="_matrixBlender"></param>
    public void SetMatrixBlender(MatrixBlender _matrixBlender)
    {
        I.blender = _matrixBlender;
    }

    /// <summary>
    /// Setta il riferimento al controllore delle befane
    /// </summary>
    /// <param name="_enemiesController"></param>
    public void SetEnemiesController(EnemiesController _enemiesController)
    {
        I.enemiesCtrl = _enemiesController;
    }

    /// <summary>
    /// Setta il riferimento allo UIManager
    /// </summary>
    /// <param name="_uiManager"></param>
    public void SetUIManager(UIManager _uiManager)
    {
        I.UIManager = _uiManager;
    }

    /// <summary>
    /// Setta il riferimento al manager dei dati
    /// </summary>
    /// <param name="_dataManager"></param>
    public void SetDataManager(DataManager _dataManager)
    {
        dataManager = _dataManager;
    }

    #endregion

    public static LevelController I;

    private int _victoryPoints;
    private int victoryPoints
    {
        get { return _victoryPoints; }
        set
        {
            _victoryPoints = value;
            GetUIManager().GetGameplayPanel().UpdatePoints(victoryPoints);
        }
    }

    float timer;
    Coroutine timerCoroutine;

    public bool IsGameplay { get; private set; }

    Animator SM;

    private void Awake()
    {
        if (I == null)
            InternalSetup();
        else
            DestroyImmediate(this);
    }

    /// <summary>
    /// Inizializza il singleton, prende riferimento della state machine e la fa partire
    /// </summary>
    void InternalSetup()
    {
        I = this;
        SM = GetComponent<Animator>();
        StartSM();
    }

    public void Init()
    {
        victoryPoints = 0;
        timer = dataManager.GetCurrentLevelData().LevelTimerSec;
        timerCoroutine = StartCoroutine("GameTimer");
    }

    /// <summary>
    /// Informazione se il gioco si trova nello stato di gameplay
    /// </summary>
    public void IsEnteringGameplayStatus()
    {
        IsGameplay = true;
    }

    public void LeavingGameplay()
    {
        IsGameplay = false;
    }

    /// <summary>
    /// Aggiunge i punti vittoria ottenuti
    /// </summary>
    /// <param name="_pointsToAdd"></param>
    public void AddVictoryPoints(int _pointsToAdd)
    {
        victoryPoints += _pointsToAdd;
        // check per le condizioni di vittoria (se tutti i regali sono stati raccolti)
        CheckGameStatus();
    }

    public void GoToContextualMenu(PauseContext _context)
    {
        if (IsGameplay)
        {
            GetUIManager().CurrentPauseContext = _context;
            GoToNext();
        }
    }

    public void GameWon()
    {
        GoToContextualMenu(PauseContext.Won);
        StopCoroutine(timerCoroutine);
    }

    public void GameLost()
    {
        GoToContextualMenu(PauseContext.Lost);
        StopCoroutine(timerCoroutine);
    }

    public void CheckGameStatus()
    {
        if (!IsGameplay)
            return;

        if (timer <= 0)
        {
            // check se i punti raccolti sono maggiori del minimo per vincere
            if (victoryPoints < GetDataManager().GetCurrentLevelData().MinScoreToWin)
            {
                GameLost();
            }
            else
            {
                GameWon();
            } 
        }else if (!GetRTSController().StillUnitInLevel())        
        {
            //Check se il numero di Santa nel livello è > 0
            GameLost();
        }
        else if (IsMathematicalDefeat())
        {
            // Se il numero di regali in gioco non è abbastanza per completare il livello
            GameLost();
        }
        else if (!giftCtrl.StillGiftsInLevel() && !GetRTSController().UnitAreCarryingGifts()) 
        {
            // Check se il numero di regali attivi nel livello è <= 0 e nessuna unità ne sta trasportando allora tutti i regali sono stati consegnati (Super vittoria)
            GameWon();
        }

    }

    bool IsMathematicalDefeat()
    {
        if(GetGiftController().GiftInLevelCount() + GetRTSController().GetGiftCollectedCount() < GetDataManager().GetCurrentLevelData().MinScoreToWin - victoryPoints)
        {
            return true;
        }
        return false;
    }

    IEnumerator GameTimer()
    {
        while (timer > 0)
        {
            GetUIManager().GetGameplayPanel().UpdateTime(timer);
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (timer <= 0)
        {
            CheckGameStatus();
        }

        yield return new WaitForEndOfFrame();
    }

    #region SM triggers
    /// <summary>
    /// Triggera la partenza della State machine
    /// </summary>
    private void StartSM()
    {
        SM.SetTrigger("StartSM");
    }
    /// <summary>
    /// Triggera il cambio di stato per andare nel Setup
    /// </summary>
    public void GoToNext()
    {
        SM.SetTrigger("GoToNext");
    }

    /// <summary>
    /// Triggera il cambio di stato per andare nel menù (dal menù di pausa)
    /// </summary>
    public void GoToMenu()
    {
        SM.SetTrigger("GoToMenu");
    }

    /// <summary>
    /// Triggera il cambio di stato per andare nel Gameplay (dal menù di pausa)
    /// </summary>
    public void GoToGameplay()
    {
        SM.SetTrigger("GoToGameplay");
    }

    /// <summary>
    /// Triggera il cambio di stato per andare nello stato di nuovo livello (dal menù di pausa)
    /// </summary>
    public void GoToChangingLevel()
    {
        SM.SetTrigger("GoToChangingLevel");
    }

    /// <summary>
    /// Triggera il cambio di stato per andare nello stato di pulizia livello (dal menù di pausa)
    /// </summary>
    public void GoToClearLevel()
    {
        SM.SetTrigger("GoToClearLevel");
    }

    #endregion
}
