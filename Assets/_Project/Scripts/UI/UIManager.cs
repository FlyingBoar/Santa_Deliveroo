using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : UI_ManagerBase
{
    /// <summary>
    /// Riferimento al pannello del main menu
    /// </summary>
    UI_MainMenu mainMenuPanel;
    /// <summary>
    /// Riferimento al pannello del gameplay
    /// </summary>
    UI_GameplayPanel gameplayPanel;
    /// <summary>
    /// Riferimento al pannello di pausa
    /// </summary>
    UI_Pause pausePanel;

    [SerializeField] CanvasGroup fadePanel;

    /// <summary>
    /// Prende riferimento ai menu
    /// </summary>
    protected override void OnSetup()
    {
        mainMenuPanel = GetComponentInChildren<UI_MainMenu>();
        gameplayPanel = GetComponentInChildren<UI_GameplayPanel>();
        pausePanel = GetComponentInChildren<UI_Pause>();
    }

    /// <summary>
    /// Il tipo di men� attivo
    /// </summary>
    MenuType currentMenuType = MenuType.None;

    public PauseContext CurrentPauseContext { get; set; }

    public void Init(LevelData _data)
    {
        gameplayPanel.Init(_data);
    }

    /// <summary>
    /// Seleziona il tipo di men� da attivare (normalmente da chiamare nella state machine)
    /// </summary>
    /// <param name="_type"></param>
    public void ChangeMenu(MenuType _type)
    {
        if (currentMenuType == _type)
            return;

        for (int i = 0; i < menus.Count; i++)
            menus[i].ToggleMenu(false);

        switch (_type)
        {
            case MenuType.None:
                break;
            case MenuType.MainMenu:
                mainMenuPanel.ToggleMenu(true);
                currentMenu = mainMenuPanel;
                break;
            case MenuType.Gameplay:
                gameplayPanel.ToggleMenu(true);
                currentMenu = gameplayPanel;
                break;
            case MenuType.Pause:
                /// Attiva il men� di pausa
                pausePanel.ToggleMenu(true);
                currentMenu = pausePanel;
                break;
            default:
                break;
        }

        currentMenuType = _type;
    }

    public UI_MainMenu GetMainMenu()
    {
        return mainMenuPanel;
    }

    public UI_GameplayPanel GetGameplayPanel()
    {
        return gameplayPanel;
    }


    public enum MenuType
    {
        None,
        MainMenu,
        Gameplay,
        Pause
    }

}
