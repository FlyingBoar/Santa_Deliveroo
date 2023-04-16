using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Befana : PoolObjectBase
{
    NavMeshAgent agent;
    Santa currentTarget;
    bool unitEnabled = false;
    UnitHitDetector detector;

    public override void OnSetup()
    {
        unitEnabled = false;
        
        var orienter = GetComponentInChildren<ObjectOrienter>();
        if(orienter) 
        {
            orienter.Init(Camera.main.transform);
        }
    }
    public override void OnRetrieve()
    {
        unitEnabled = false;
    }

    public void Init(float _speed)
    {
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        if(!detector)
        {
            detector = GetComponentInChildren<UnitHitDetector>();
            detector.Init(this);
        }

        agent.speed = _speed;
        unitEnabled = true;
    }

    public void UnitHit(Santa _santa)
    {
        LevelController.I.GetEnemiesController().EnemyHitUnit(this, _santa);
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
                if (!LevelController.I.GetNavMeshCtrl().IsPointOnNavmesh(currentTarget.transform.position))
                {
                    currentTarget = null;
                }
                else
                {
                    agent.SetDestination(currentTarget.transform.position);
                }
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
