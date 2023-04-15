using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_ManagerBase : MonoBehaviour
{
    /// <summary>
    /// Lista di menù/pannelli
    /// </summary>
    protected List<UI_MenuBase> menus;
    /// <summary>
    /// Menù attualmente selezionato
    /// </summary>
    protected UI_MenuBase currentMenu;

    /// <summary>
    /// Setup della classe
    /// </summary>
    public void Setup()
    {
        menus = GetComponentsInChildren<UI_MenuBase>(true).ToList();

        foreach (UI_MenuBase menu in menus)
            menu.Setup(this);

        OnSetup();
    }

    /// <summary>
    /// Restituisce il menu attivo
    /// </summary>
    /// <returns></returns>
    public UI_MenuBase GetCurrentMenu()
    {
        return currentMenu;
    }

    /// <summary>
    /// Funzione chiamata al setup
    /// </summary>
    protected virtual void OnSetup() { }
}
