using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Befana : PoolObjectBase
{
    NavMeshAgent agent;
    Santa currentTarget;
    bool unitEnabled = false;

    public override void OnSetup()
    {
        unitEnabled = false;
    }

    public void Init(float _speed)
    {
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        agent.speed = _speed;
        unitEnabled = true;
    }

    public override void OnRetrieve()
    {
        unitEnabled = false;
    }

    private void LateUpdate()
    {
        if (!unitEnabled)
            return;

        if (agent)
        {
            // è arrivato alla fine del percorso, trovo un altro waypoint

            if (currentTarget)
            {
                agent.SetDestination(currentTarget.transform.position);
            }

            if (!currentTarget && !agent.hasPath)
            {
                agent.SetDestination(LevelController.I.GetNavMeshCtrl().GetRandomLocation());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
         var santa = other.GetComponentInParent<Santa>();
        if (santa != null && currentTarget == null)
        {
            currentTarget = santa;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var santa = other.GetComponentInParent<Santa>();
        if (santa != null && santa == currentTarget)
        {
            currentTarget = null;
        }
    }

}
