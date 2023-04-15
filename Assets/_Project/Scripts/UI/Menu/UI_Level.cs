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

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    public void Init(LevelData _data, UI_LevelContainer _container)
    {
        level = _data;
        selectable = GetComponent<Selectable>();
        container = _container;
        IsSelected = false;
        var text = GetComponentInChildren<TextMeshProUGUI>();
        if(text) 
            text.text = level.name;
    }

    void OnValueChanged()
    {
        if(toggle.isOn)
        {
            container.LevelSelected(this);
            IsSelected = true;
        }
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
