using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPathfinding : MonoBehaviour
{
    [SerializeField]
    private Vector3 walkPoint;
    void Start()
    {
        GetComponent<NavMeshAgent>().destination = walkPoint;
    }

}
