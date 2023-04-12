using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class NavMeshController : MonoBehaviour
{
    Bounds floor;
    Vector3 navMeshPanelPos;

    public void Setup()
    {
        var navMeshPanel = GameObject.FindGameObjectWithTag("NavMeshPanel");
        floor = navMeshPanel.GetComponent<Renderer>().bounds;
        navMeshPanelPos = navMeshPanel.GetComponent<Transform>().position;
    }

    public Vector3 GetRandomLocation()
    {
        Vector3 randomSpot;
        do
        {
            randomSpot = new Vector3(Random.Range(floor.min.x, floor.max.x), navMeshPanelPos.y, Random.Range(floor.min.z, floor.max.z));
        } while (!isPointOnNavmesh(randomSpot));

        return randomSpot;
    }

    bool isPointOnNavmesh(Vector3 _pos)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        bool cancomplete = NavMesh.CalculatePath(navMeshPanelPos, _pos, 1,navMeshPath);
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
