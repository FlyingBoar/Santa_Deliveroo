using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Level : MonoBehaviour
{
    LevelData level;
    Selectable selectable;
    UI_LevelContainer container;
    Toggle toggle;

    public bool IsSelected { get; private set; }

    public void Init(LevelData _data, UI_LevelContainer _container)
    {
        if(toggle == null)
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(delegate { OnValueChanged(); });
        }

        level = _data;
        selectable = GetComponent<Selectable>();
        container = _container;
        IsSelected = false;
        var text = GetComponentInChildren<TextMeshProUGUI>();
        if(text) 
            text.text = "Level" + level.level;
    }

    void OnValueChanged()
    {
        if(toggle.isOn)
        {
            container.LevelSelected(this);
            IsSelected = true;
        }
    }

    public void isFirstSelected()
    {
        toggle.isOn = true;
    }

    public void RemoveSelected()
    {
        toggle.isOn = false;
        IsSelected = false;
    }

    public LevelData GetLevelData()
    {
        return level;
    }
}
