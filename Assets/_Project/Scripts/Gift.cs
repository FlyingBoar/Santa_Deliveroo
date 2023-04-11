using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : PoolObjectBase, IDestination
{
    public void OnClickOver()
    {
        // highlight + mostra informazioni in UI
    }

    public void OnDeselect()
    {
        // Rimuove highlight
    }

    public Vector3 GetDestinationPosition()
    {
        return transform.position;
    }
}
