using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MenuBase : MonoBehaviour
{
    /// <summary>
    /// Riferimento al prorpio manager
    /// </summary>
    protected UI_ManagerBase manager;

    /// <summary>
    /// Stato di attivo o disattivo del menù
    /// </summary>
    protected bool isActive;

    /// <summary>
    /// Setup del menu
    /// </summary>
    public void Setup(UI_ManagerBase _manager)
    {
        manager = _manager;
        isActive = true;
        OnSetup();
    }

    /// <summary>
    /// Funzione chimata al setup della classe base
    /// </summary>
    public virtual void OnSetup() { }

    /// <summary>
    /// Funzione che ritorna true se il menù è attivo, false altrimenti
    /// </summary>
    /// <returns></returns>
    public bool IsActive()
    {
        return isActive;
    }

    /// <summary>
    /// Funzion che attiva o disattiva il GameObject del menù
    /// </summary>
    /// <param name="_value"></param>
    public virtual void ToggleMenu(bool _value)
    {
        if (isActive == _value)
            return;

        isActive = _value;
        gameObject.SetActive(isActive);
    }
}
