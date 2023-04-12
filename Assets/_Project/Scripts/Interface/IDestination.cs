using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IDestination : IInteractable
{
    Vector3 GetDestinationPosition();

    void AgentOnDestination(Santa _agent);
}
