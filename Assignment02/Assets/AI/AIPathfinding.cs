using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPathfinding : MonoBehaviour
{
    [SerializeField]
    private Vector3 walkPoint;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<NavMeshAgent>().destination = walkPoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
