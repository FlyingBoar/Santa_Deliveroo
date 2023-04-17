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

    /// <summary>
    /// Restituisce una posizione casuale raggiungibile dagli agenti
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomLocation()
    {
        Vector3 randomSpot;
        do
        {
            randomSpot = new Vector3(Random.Range(floor.min.x, floor.max.x), navMeshPanelPos.y, Random.Range(floor.min.z, floor.max.z));
        } while (!IsPointOnNavmesh(randomSpot));

        return randomSpot;
    }

    /// <summary>
    /// Controlla se la posizione data si trova all'interno del navmesh
    /// </summary>
    /// <param name="_pos"></param>
    /// <returns></returns>
    public bool IsPointOnNavmesh(Vector3 _pos)
    {
        NavMeshPath navMeshPath = GetPathToPoint(navMeshPanelPos, _pos);
        return navMeshPath != null ? true : false;
    }

    public NavMeshPath GetPathToPoint(Vector3 _startPos, Vector3 _endPos)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        bool cancomplete = NavMesh.CalculatePath(_startPos, _endPos, 1, navMeshPath);
        if (cancomplete && navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            return navMeshPath;
        }
        else
        {
            return null;
        }
    }
}
