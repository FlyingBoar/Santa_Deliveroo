using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void OnSelect(bool _directSelectin = true);
    public void OnDeselect();
}