using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class UI_Pause : UI_MenuBase
{
    [SerializeField] TextMeshProUGUI contextInformation;
    [SerializeField] Button NextRestartButton;
    [SerializeField] GameObject ResumeButton;

    public override void ToggleMenu(bool _value)
    {
        base.ToggleMenu(_value);
        SetContext((manager as UIManager).CurrentPauseContext);
    }

    public void SetContext(PauseContext _context)
    {
        ResumeButton.SetActive(false);
        RemoveListenerFromButton();
        switch (_context) 
        {
            case PauseContext.Pause:
                contextInformation.text = "PAUSE";
                ResumeButton.SetActive(true);
                SetRetryAction();
                break;
            case PauseContext.Won:
                contextInformation.text = "GAME WON";
                NextRestartButton.onClick.AddListener(delegate { OnNextLevel(); });
                NextRestartButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next Level";
                break;
            case PauseContext.Lost:
                contextInformation.text = "GAME LOST";
                SetRetryAction();
                break;
            default:
                contextInformation.text = "PAUSE";
                break;
        }

    }


    private void Update()
    {
        if(Input.GetKey(KeyCode.N)) 
        {
            OnNextLevel();
        }
    }

    public void RemoveListenerFromButton()
    {
        NextRestartButton.onClick.RemoveAllListeners();
    }

    void SetRetryAction()
    {
        NextRestartButton.onClick.AddListener(delegate { OnRetry(); });
        NextRestartButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retry";
    }


    public void OnBackToMenu()
    {
        /// Chiama al funzione nel manager per far andare la state machine nello stato di menù
        LevelController.I.GoToMenu();
    }
    void OnResume()
    {
        /// Chiama al funzione nel manager per far andare la state machine nello stato di 
        LevelController.I.GoToGameplay();
    }
    void OnNextLevel()
    {
        /// Chiama al funzione nel manager per far andare la state machine nello stato di Clear for next level
        LevelController.I.GoToChangingLevel();
    }
    public void OnRetry()
    {
        /// Chiama al funzione nel manager per far andare la state machine nello stato di Clear per ricaricare lo stesso livello
        LevelController.I.GoToClearLevel();
    }

}

public enum PauseContext
{
    Pause,
    Won,
    Lost
}
