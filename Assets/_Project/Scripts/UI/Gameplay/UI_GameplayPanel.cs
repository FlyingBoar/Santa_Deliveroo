using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Non utilizzato al momento, da capire cosa visualizzare durante il gameplay
/// </summary>
public class UI_GameplayPanel : UI_MenuBase
{
    [SerializeField]
    Slider timeSlider;

    [SerializeField]
    TextMeshProUGUI pointsCounter;

    UI_CollectedGiftController collectedGiftController;
    int pointsForLevel;

    public override void OnSetup()
    {
        collectedGiftController = GetComponentInChildren<UI_CollectedGiftController>();
        collectedGiftController.Setup();
    }

    public void Init(LevelData _data)
    {
        timeSlider.maxValue = _data.LevelTimerSec;
        timeSlider.value = _data.LevelTimerSec;
        pointsForLevel = _data.MinScoreToWin;
        UpdatePoints(0);
        DisableGiftcontainer();
    }

    public void UpdateTime(float _time)
    {
        timeSlider.value = _time;
    }

    public void UpdatePoints(int _points)
    {
        pointsCounter.text = _points + "/" + pointsForLevel;
    }

    public void UpdateGiftContainer(List<GiftData> _datas)
    {
        collectedGiftController.ActivateGiftPanel(_datas);
    }

    public void DisableGiftcontainer()
    {
        collectedGiftController.RemoveDatas();
    }
}
