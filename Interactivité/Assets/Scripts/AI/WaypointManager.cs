using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance { get; protected set; }

    public GameObject[] Waypoints;
    

    void Start()
    {
        if (!Instance)
            Instance = this;
    }

    public static Vector3 GetRandomWaypoint()
    {
        if (!Instance)
            return Vector3.zero;
        
        if (Instance.Waypoints.Length == 0)
            return Vector3.zero;

        int randomIndex = Random.Range(0, Instance.Waypoints.Length - 1);
        return Instance.Waypoints[randomIndex].transform.position;
    }
}
