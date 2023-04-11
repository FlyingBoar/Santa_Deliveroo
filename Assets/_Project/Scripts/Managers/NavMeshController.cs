using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class NavMeshController : MonoBehaviour
{
    Bounds floor;
    float navMeshPanelHeigh;
    NavMeshAgent agent;

    public void Setup()
    {
        var navMeshPanel = GameObject.FindGameObjectWithTag("NavMeshPanel");
        floor = navMeshPanel.GetComponent<Renderer>().bounds;
        navMeshPanelHeigh = navMeshPanel.GetComponent<Transform>().position.y;
        agent = LevelController.I.GetRTSController.GetFirstAvailableAgent().GetComponent<NavMeshAgent>();
    }

    public Vector3 GetRandomLocation()
    {
        Vector3 randomSpot;
        do
        {
            randomSpot = new Vector3(Random.Range(floor.min.x, floor.max.x), navMeshPanelHeigh, Random.Range(floor.min.z, floor.max.z));
        } while (!isPointOnNavmesh(randomSpot));

        return randomSpot;
    }

    bool isPointOnNavmesh(Vector3 _pos)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        bool cancomplete = agent.CalculatePath(_pos, navMeshPath);
        if (cancomplete && navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            // the target can be reached
            return true;
        }
        else
        {
            // no path for target
            return false;
        }
    }
}
