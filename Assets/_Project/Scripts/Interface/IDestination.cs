using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IDestination : IInteractable
{
    public Vector3 GetDestinationPosition();

    public void AgentOnDestination(Santa _agent);

}
