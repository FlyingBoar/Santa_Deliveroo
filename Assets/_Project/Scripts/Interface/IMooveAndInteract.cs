using UnityEngine;

public interface IMooveAndInteract : IInteractable
{
    public void OnAction(IDestination destination, bool isQueuedAction = false);
    public void OnAction(Vector3 moveTo, bool isQueuedAction = false, IDestination destination = null);
}