using System.Collections;
using System.Collections.Generic;
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
    
    #endregion

    public static LevelController I;
    public LevelData LevelData; //TODO: caricare dinamicamente

    private int _victoryPoints;

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

    public void IsGameplayStatus()
    {
        IsGameplay = true;
    }

    public void AddVictoryPoints(int _pointsToAdd)
    {
        _victoryPoints += _pointsToAdd;
        // check per le condizioni di vittoria
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

    #endregion
}
