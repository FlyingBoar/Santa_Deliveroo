using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void OnClickOver();
    public void OnDeselect();
    public void OnAction(IInteractable interactable = null);
}

public interface IMooveAndInteract : IInteractable
{
    public void OnAction(Vector3 moveTo);
}