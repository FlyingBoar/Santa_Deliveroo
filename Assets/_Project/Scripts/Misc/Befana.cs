using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Befana : PoolObjectBase
{
    NavMeshAgent agent;


    public override void OnSetup()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void LateUpdate()
    {
        if (agent && !agent.hasPath) 
        { 
            // è arrivato alla fine del percorso, trovo un altro waypoint
        }
    }

}
