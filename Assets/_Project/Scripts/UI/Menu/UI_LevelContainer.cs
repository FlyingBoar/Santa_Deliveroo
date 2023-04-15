using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_LevelContainer : MonoBehaviour
{
    [SerializeField]
    UI_Level levelPrefab;
    List<UI_Level> levelList;
    UI_MainMenu manager;
    public void Init(List<LevelData> _datas, UI_MainMenu _manager)
    {
        levelList = new List<UI_Level>();
        manager = _manager;
        foreach (LevelData data in _datas)
        {
            UI_Level level = Instantiate(levelPrefab,transform);
            level.Init(data, this);
            levelList.Add(level);
        }
    }

    public void LevelSelected(UI_Level _level)
    {
        foreach (UI_Level item in levelList)
        {
            if (item == _level)
                continue;

            item.RemoveSelected();
        }

        manager.SetcurrentLevelSelected(_level.GetLevelData());
    }
}
