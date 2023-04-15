using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_MainMenu : UI_MenuBase
{
    UI_LevelContainer levelContainer;

    LevelData currentLevelSelected;

    public override void OnSetup()
    {
        levelContainer = GetComponentInChildren<UI_LevelContainer>();
        levelContainer.Init(Resources.LoadAll<LevelData>("Data").ToList(), this);
    }

    public LevelData GetCurrentLevelSelected() {
        return currentLevelSelected;
    }

    public void SetcurrentLevelSelected(LevelData levelSelected)
    {
        currentLevelSelected = levelSelected;
    }

    public void GoToGameplay()
    {
        LevelController.I.GoToNext();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
